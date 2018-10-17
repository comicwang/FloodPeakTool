using FloodPeakUtility;
using FloodPeakUtility.UI;
using iTelluro.DataTools.Utility.Img;
using iTelluro.DataTools.Utility.SHP;
using iTelluro.Explorer.Raster;
using iTelluro.GlobeEngine.DataSource.Geometry;
using iTelluro.GlobeEngine.Graphics3D;
using iTelluro.GlobeEngine.MapControl3D;
using iTelluro.GlobeEngine.Mathematics;
using OSGeo.GDAL;
using OSGeo.OGR;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace FloodPeakToolUI
{
    public class Hydrology
    {
        private static string _path = Path.Combine(Application.StartupPath, ConfigNames.Colors);

        /// <summary>
        /// 记录坑地
        /// </summary>
        private List<Grid> noFlowList = new List<Grid>();

        /// <summary>
        /// 获取高程矩阵
        /// </summary>
        public double[,] GetElevation(RasterReader read)
        {
            int rowCount = read.RowCount;
            int colCount = read.ColumnCount;
            double[,] result = new double[rowCount, colCount];//源数据高程值
            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < colCount; col++)
                {
                    result[row, col] = ReadBand(read, row, col);
                }
            }
            return result;
        }

        public int[,] GetIntElevation(RasterReader read)
        {
            int rowCount = read.RowCount;
            int colCount = read.ColumnCount;
            int[,] result = new int[rowCount, colCount];//源数据高程值
            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < colCount; col++)
                {
                    result[row, col] =(int)ReadBand(read, row, col);
                }
            }
            return result;
        }

        /// <summary>
        /// 读取特定像元值
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        private double ReadBand(RasterReader read, int row, int col)
        {
            double[] d = null;
            read.ReadBand(col, row, 1, 1, out d);
            return d[0];
        }

        /// <summary>
        /// 根据原DEM计算流向
        /// </summary>
        /// <param name="srcPath"></param>
        /// <param name="savePath"></param>
        /// <returns></returns>
        public bool FlowDirection(string srcPath, string savePath)
        {
            try
            {
                RasterReader read = new RasterReader(srcPath);
                int row = read.RowCount;
                int col = read.ColumnCount;

                double[,] src = GetElevation(read);

                //判断存储路径是否存在
                RasterWriter writer = new RasterWriter(savePath, 1, col, row, DataType.GDT_Float32);
                writer.SetProjection(read.DataSet.GetProjectionRef());
                writer.SetPosition(read.MapRectEast, read.MapRectWest, read.CellSizeX, read.CellSizeY);

                //按行计算流向并写入
                double[] data = null;


                ColorRamps ramps = XmlHelper.Deserialize<ColorRamps>(_path);
                FormCalView.SetAllSize(row, col);

                Dictionary<string, Color> dic = new Dictionary<string, Color>();
                dic.Add("右-1", ramps.GetColor(1));
                dic.Add("右下-2", ramps.GetColor(2));
                dic.Add("下-4", ramps.GetColor(4));
                dic.Add("左下-8", ramps.GetColor(8));
                dic.Add("左-16", ramps.GetColor(16));
                dic.Add("左上-32", ramps.GetColor(32));
                dic.Add("上-64", ramps.GetColor(64));
                dic.Add("右上-128", ramps.GetColor(128));

                FormCalView.SetLegend(dic);
                for (int i = 0; i < row; i++)
                {
                    data = new double[col];
                    for (int j = 0; j < col; j++)
                    {
                        int flow = CalFlowDirection(row, col, i, j, src);
                        data[j] = flow;
                        FormCalView.SetColor(i, j, flow);
                    }
                    writer.WriteBand(1, 0, i, col, 1, data);
                }
                writer.Dispose();
                read.Dispose();
            }
            catch(Exception ex)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 填充洼地
        /// </summary>
        /// <param name="srcPath"></param>
        /// <param name="savePath"></param>
        /// <returns></returns>
        public bool Fill(string srcPath, string savePath)
        {
            try
            {
                RasterReader read = new RasterReader(srcPath);
                int rowCount = read.RowCount;
                int colCount = read.ColumnCount;
                double[,] src = GetElevation(read);

                //判断存储路径是否存在
                RasterWriter writer = new RasterWriter(savePath, 1, colCount, rowCount, DataType.GDT_Float32);
                writer.SetProjection(read.DataSet.GetProjectionRef());
                writer.SetPosition(read.MapRectEast, read.MapRectWest, read.CellSizeX, read.CellSizeY);

                for (int row = 0; row < rowCount; row++)
                {
                    int[] data = new int[colCount];
                    for (int col = 0; col < colCount; col++)
                    {
                        double maxValue = double.MinValue;
                        //遍历每个像元
                        Grid temp = GetMin(rowCount, colCount, row, col, src, ref maxValue);
                        int flow = 0;
                        if (temp != null)
                        {
                            if (temp.i == rowCount)
                            {
                                temp.i = temp.i - 1;
                            }
                            if (temp.j == colCount)
                            {
                                temp.j = temp.j - 1;
                            }
                            if (temp.i == -1)
                            {
                                temp.i = 0;
                            }
                            if (temp.j == -1)
                            {
                                temp.j = 0;
                            }

                            flow = (int)ReadBand(read, temp.i, temp.j);
                            if (flow != -1)
                            {
                                data[col] = flow;
                            }
                            //data[col] = flow;
                        }
                    }
                    writer.WriteBand(1, 0, row, colCount, 1, data);
                    data = new int[colCount];
                }
                writer.Dispose();
                read.Dispose();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 计算汇流数据
        /// </summary>
        public bool FlowAccumulation(string srcPath, string savePath, int value)
        {
            try
            {
                RasterReader read = new RasterReader(srcPath);
                int rowCount = read.RowCount;
                int colCount = read.ColumnCount;
               
                int[,] result = Accumulation(read);

                List<double> data = new List<double>();
                int setValue = value;//汇流阈值

                for (int i = 0; i < result.GetLength(0); i++)
                {
                    for (int j = 0; j < result.GetLength(1); j++)
                    {
                        int acc = result[i, j];
                        if (acc > setValue)
                        {
                            data.Add(acc);
                        }
                        else
                        {
                            data.Add(-1);
                        }
                    }
                }

                //重新生成汇流栅格
                RasterWriter writer = new RasterWriter(savePath, 1, colCount, rowCount, DataType.GDT_Float32);
                writer.SetProjection(read.DataSet.GetProjectionRef());
                writer.SetPosition(read.MapRectEast, read.MapRectWest, read.CellSizeX, read.CellSizeY);
                writer.SetNodata(1, -1);//设置无数据

                double[] buffer = data.ToArray();
                for (int i = 0; i < result.GetLength(1) / 25; i++)
                {
                    for (int j = 0; j < result.GetLength(0) / 25; j++)
                    {
                        writer.WriteBand(1, i, j, colCount, rowCount, buffer);
                    }
                }
                writer.Dispose();

                ////判断存储路径是否存在
                //RasterWriter writer = new RasterWriter(savePath, 1, col, row, DataType.GDT_Float32);
                //writer.SetProjection(read.DataSet.GetProjectionRef());
                //writer.SetPosition(read.MapRectEast, read.MapRectWest, read.CellSizeX, read.CellSizeY);

                ////按行计算流向并写入
                //double[] data = new double[col];

                //for (int i = 0; i < row; i++)
                //{
                //    for (int j = 0; j < col; j++)
                //    {
                //        if ((data[j] = GetVector(i, j)) == -1)
                //        {
                //            noFlowList.Add(new Grid(i, j));
                //        }

                //    }
                //    writer.WriteBand(1, 0, i, col, 1, data);
                //    data = new double[col];
                //}

                //writer.Dispose();
                read.Dispose();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Raster2Polyline(string srcPath,string outputPath)
        {
            return false;
        }

        #region 算法

        /// <summary>
        /// 根据流域面积文件来获取流域面
        /// </summary>
        /// <param name="shppath"></param>
        /// <returns></returns>
        public iTelluroLib.GeoTools.Geometries.Polygon CalLimitAreaPolygon(string shppath, ref GeoRect _analysisRect)
        {
            try
            {
                ShpReader shp = new ShpReader(shppath);
                OSGeo.OGR.Feature ofea;
                List<Point2d> pointlist = new List<Point2d>();
                while (((ofea = shp.layer.GetNextFeature()) != null))
                {
                    int count = ofea.GetGeometryRef().GetPointCount();
                    string a = "";
                    ofea.GetGeometryRef().ExportToWkt(out a);
                    Regex reg = new Regex(@"\(([^)&^(]*)\)");
                    Match m = reg.Match(a);
                    string pointstr = "";
                    if (m.Success)
                    {
                        pointstr = m.Result("$1").TrimStart('(').TrimEnd(')');
                    }
                    for (int i = 0; i < pointstr.Split(',').Length; i++)
                    {
                        string[] point = pointstr.Split(',')[i].Split(' ');
                        double x = double.Parse(point[0]);
                        double y = double.Parse(point[1]);
                        Point2d p = new Point2d(x, y);
                        pointlist.Add(p);
                    }
                }
                //释放资源
                shp.Dispose();

                if (pointlist.Count >= 3)
                {
                    // 外接矩阵，边界取经纬度范围的反值            
                    List<double> xPoint = new List<double>();
                    List<double> yPoint = new List<double>();
                    for (int i = 0; i < pointlist.Count; i++)
                    {
                        Point2d p = pointlist[i];
                        // 外接矩阵计算
                        xPoint.Add(p.X);
                        yPoint.Add(p.Y);
                    }
                    _analysisRect.North = yPoint.Max();
                    _analysisRect.South = yPoint.Min();
                    _analysisRect.East = xPoint.Max();
                    _analysisRect.West = xPoint.Min();

                    // NTS多边形
                    iTelluroLib.GeoTools.Geometries.Coordinate[] coords = new iTelluroLib.GeoTools.Geometries.Coordinate[pointlist.Count + 1];

                    for (int i = 0; i < pointlist.Count; i++)
                    {
                        coords[i] = new iTelluroLib.GeoTools.Geometries.Coordinate(pointlist[i].X, pointlist[i].Y);
                    }
                    coords[pointlist.Count] = new iTelluroLib.GeoTools.Geometries.Coordinate(pointlist[0].X, pointlist[0].Y);

                    //创建多边形
                    iTelluroLib.GeoTools.Geometries.LinearRing ring = new iTelluroLib.GeoTools.Geometries.LinearRing(coords);
                    return new iTelluroLib.GeoTools.Geometries.Polygon(ring);
                }
                
            }
            catch(Exception ex)
            {
                FormOutput.AppendLog("获取流域面的形状失败：" + ex.Message);
            }
            return null;
        }

        private void CaculateOne(int[,] src, Grid index, ref Dictionary<Grid, List<Grid>> caled)
        {
            if (caled.ContainsKey(index))
                return;
            else
            {
                List<Grid> lstTemp = new List<Grid>();
                Grid temp = null;
                //搜索八个方向
                bool result = this.IsFlowTo(src, index, Direct.left, out temp);
                if (result)
                {
                    lstTemp.Add(temp);
                }
                result = this.IsFlowTo(src, index, Direct.leftBottom, out temp);
                if (result)
                {
                    lstTemp.Add(temp);
                }
                result = this.IsFlowTo(src, index, Direct.leftTop, out temp);
                if (result)
                {
                    lstTemp.Add(temp);
                }
                result = this.IsFlowTo(src, index, Direct.right, out temp);
                if (result)
                {
                    lstTemp.Add(temp);
                }
                result = this.IsFlowTo(src, index, Direct.rightTop, out temp);
                if (result)
                {
                    lstTemp.Add(temp);
                }
                result = this.IsFlowTo(src, index, Direct.rigthBottom, out temp);
                if (result)
                {
                    lstTemp.Add(temp);
                }
                result = this.IsFlowTo(src, index, Direct.top, out temp);
                if (result)
                {
                    lstTemp.Add(temp);
                }
                result = this.IsFlowTo(src, index, Direct.bottom, out temp);
                if (result)
                {
                    lstTemp.Add(temp);
                }
                caled.Add(index, lstTemp);
            }
        }

        /// <summary>
        /// 判断当前单元格指定方向的单元格是否流向它
        /// </summary>
        /// <param name="src"></param>
        /// <param name="index"></param>
        /// <param name="direct"></param>
        /// <returns></returns>
        private bool IsFlowTo(int[,] src,Grid index,Direct direct,out Grid outPutGrid)
        {
            switch (direct)
            {
                case Direct.left:
                    //判断边界
                    if (index.i - 1 >= 0)
                    {
                        int x = src[index.i - 1, index.j];
                        if (x == (int)direct)
                        {
                            outPutGrid = new Grid(index.i - 1, index.j);
                            return true;
                        }
                    }
                    break;
                case Direct.leftTop:
                    //判断边界
                    if (index.i - 1 >= 0 && index.j - 1 >= 0)
                    {
                        int x = src[index.i - 1, index.j - 1];
                        if (x == (int)direct)
                        {
                            outPutGrid = new Grid(index.i - 1, index.j - 1);
                            return true;
                        }
                    }
                    break;
                case Direct.top:
                    //判断边界
                    if (index.j - 1 >= 0)
                    {
                        int x =src[index.i, index.j - 1];
                        if (x == (int)direct)
                        {
                            outPutGrid = new Grid(index.i, index.j - 1);
                            return true;
                        }
                    }
                    break;
                case Direct.rightTop:
                    //判断边界
                    if (index.i + 1 < src.GetLength(0) && index.j - 1 >= 0)
                    {
                        int x = src[index.i + 1, index.j - 1];
                        if (x == (int)direct)
                        {
                            outPutGrid = new Grid(index.i + 1, index.j - 1);
                            return true;
                        }
                    }
                    break;
                case Direct.right:
                    //判断边界
                    if (index.i + 1 < src.GetLength(0))
                    {
                        int x = src[index.i + 1, index.j];
                        if (x == (int)direct)
                        {
                            outPutGrid = new Grid(index.i + 1, index.j);
                            return true;
                        }
                    }
                    break;
                case Direct.rigthBottom:
                    //判断边界
                    if (index.i + 1 < src.GetLength(0) && index.j + 1 < src.GetLength(1))
                    {
                        int x = src[index.i + 1, index.j + 1];
                        if (x == (int)direct)
                        {
                            outPutGrid = new Grid(index.i + 1, index.j + 1);
                            return true;
                        }
                    }
                    break;
                case Direct.bottom:
                    //判断边界
                    if (index.j + 1 < src.GetLength(1))
                    {
                        int x = src[index.i, index.j + 1];
                        if (x == (int)direct)
                        {
                            outPutGrid = new Grid(index.i, index.j + 1);
                            return true;
                        }
                    }
                    break;
                case Direct.leftBottom:
                    //判断边界
                    if (index.i - 1 >= 0 && index.j + 1 < src.GetLength(1))
                    {
                        int x = src[index.i - 1, index.j + 1];
                        if (x == (int)direct)
                        {
                            outPutGrid = new Grid(index.i - 1, index.j + 1);
                            return true;
                        }
                    }
                    break;
                default:
                    break;
            }
            outPutGrid = null;
            return false;
        }

        /// <summary>
        /// 计算汇流累积
        /// </summary>
        public int[,] Accumulation(RasterReader read)
        {
            int rowCount = read.RowCount;
            int colCount = read.ColumnCount;
            int[,] src = GetIntElevation(read);
            int[,] result = new int[rowCount, colCount];
            double[,] r = new double[rowCount, colCount];
            Queue<Grid> queueGrid = new Queue<Grid>();
            //获取方程数据源
            Dictionary<Grid, List<Grid>> dicCaledAll = new Dictionary<Grid, List<Grid>>();
            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < colCount; col++)
                {
                    try
                    {
                        Grid tempGrid = new Grid(row, col);
                        queueGrid.Enqueue(tempGrid);
                        CaculateOne(src, tempGrid, ref dicCaledAll);
                    }
                    catch(Exception ex)
                    {

                    }
                }
            }
            FormCalView.SetAllSize(rowCount, colCount);
            Dictionary<Grid,int> findOutGrid = new Dictionary<Grid,int>();
            Dictionary<Grid, int> unFindOutGrid = new Dictionary<Grid, int>();

            FormOutput.AppendProress(true);
            //开始解方程
            while (queueGrid.Count > 0)
            {
                FormOutput.AppendProress(findOutGrid.Count * 100 / dicCaledAll.Count);
                Grid tGrid = queueGrid.Dequeue();
                List<Grid> lstGrid = dicCaledAll[tGrid];
                if (lstGrid.Count == 0)
                {
                    findOutGrid.Add(tGrid, 1);

                    FormCalView.SetValue(tGrid.i, tGrid.j, 1);
                }
                //临时添加河口
                else if(tGrid.i==0)
                {
                    findOutGrid.Add(tGrid, 1);
                    FormCalView.SetValue(tGrid.i, tGrid.j, 1);
                }
                else
                {
                    List<Grid> unFindGrid =new List<Grid>();
                    int value = 1;
                    //继续上次未完成的计算
                    if(unFindOutGrid.ContainsKey(tGrid))
                    {
                        value = unFindOutGrid[tGrid];
                    }

                    //循环寻找当前找出的Grid
                    for (int i = 0; i < lstGrid.Count; i++)
                    {
                        if(findOutGrid.ContainsKey(lstGrid[i]))
                        {
                            value += findOutGrid[lstGrid[i]];
                        }
                        else
                        {
                            unFindGrid.Add(lstGrid[i]);
                        }
                    }

                    //如果还有未确定的继续添加计算
                    if (unFindGrid.Count > 0)
                    {
                        dicCaledAll[tGrid] = unFindGrid;
                        if (unFindOutGrid.ContainsKey(tGrid))
                            unFindOutGrid[tGrid] = value;
                        else
                            unFindOutGrid.Add(tGrid, value);

                        queueGrid.Enqueue(tGrid);
                    }
                    else
                    {
                        findOutGrid.Add(tGrid, value);
                        FormCalView.SetValue(tGrid.i, tGrid.j,value);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 计算特定位置像元水流方向(根据高程差)
        /// 算法说明：计算某个单元格附近八个（不足的不考虑）的最小高程（最大高程差）
        /// 结果说明：若有一个最小的，流向指向最小的；若有多个最小的继续分别计算几个最小值附近的最小值
        /// 一直递归找到最小值的那一项
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="col">列</param>
        /// <returns></returns>
        public int CalFlowDirection(int rowCount, int colCount, int row, int col, double[,] src)
        {
            double Sqrt2 = Math.Sqrt(2);
            double S = 0, N = 0, E = 0, SE = 0, NE = 0, NW = 0, W = 0, SW = 0;

            S = (row != (rowCount - 1)) ? (src[row, col] - src[row + 1, col]) : -1;
            SE = (row != (rowCount - 1) && col != (colCount - 1)) ? (src[row, col] - src[row + 1, col + 1]) / Sqrt2 : -1;
            N = (row != 0) ? (src[row, col] - src[row - 1, col]) : -1;
            E = (col != (colCount - 1)) ? (src[row, col] - src[row, col + 1]) : -1;
            NE = (row != 0 && col != (colCount - 1)) ? (src[row, col] - src[row - 1, col + 1]) / Sqrt2 : -1;
            NW = (row != 0 && col != 0) ? (src[row, col] - src[row - 1, col - 1]) / Sqrt2 : -1;
            W = (col != 0) ? (src[row, col] - src[row, col - 1]) : -1;
            SW = (row != (rowCount - 1) && col != 0) ? (src[row, col] - src[row + 1, col - 1]) / Sqrt2 : -1;

            //判断其中最小值
            double M = new List<double>() { S, SE, N, E, NE, NW, W, SW }.Where(t => t != -1).Min();

            //洼地
            if (M < 0)
            {
                noFlowList.Add(new Grid(row, col));
            }

            ////取最大的坡降
            Dictionary<string, int> dicMin = new Dictionary<string, int>();
            if (M == S)
            {
                dicMin.Add("S", 4);
            }
            if (M == SE)
            {
                dicMin.Add("SE", 2);
            }
            if (M == N)
            {
                dicMin.Add("N", 64);
            }
            if (M == E)
            {
                dicMin.Add("E", 1);
            }
            if (M == NE)
            {
                dicMin.Add("NE", 128);
            }
            if (M == NW)
            {
                dicMin.Add("NW", 32);
            }
            if (M == W)
            {
                dicMin.Add("W",16);
            }
            if (M == SW)
            {
                dicMin.Add("SW", 8);
            }
            if (dicMin.Count==1)
            {
                return dicMin.Values.FirstOrDefault();
            }
            return 0;
        }

        public Grid GetMin(int rowCount, int colCount, int row, int col, double[,] src, ref double max)
        {
            List<Grid> minGrids = new List<Grid>();
            Grid minGrid = null;
            //double max = double.MinValue;
            double temp = double.MinValue;
            double Sqrt2 = Math.Sqrt(2);
            //S
            if (row != (rowCount - 1))
            {
                Grid tempGrid = new Grid(row + 1, col);
                temp = src[row, col] - src[row + 1, col];
                if (temp > max)
                {
                    minGrid = tempGrid;
                    minGrids.Clear();
                    minGrids.Add(minGrid);
                    max = temp;
                }
                else if (temp == max)
                {
                    minGrids.Add(tempGrid);
                }
            }
            //SE
            if (row != (rowCount - 1) && col != (colCount - 1))
            {
                Grid tempGrid = new Grid(row + 1, col + 1);
                temp = (src[row, col] - src[row + 1, col + 1]) / Sqrt2;
                if (temp > max)
                {
                    minGrid = tempGrid;
                    minGrids.Clear();
                    minGrids.Add(minGrid);
                    max = temp;
                }
                else if (temp == max)
                {
                    minGrids.Add(tempGrid);
                }
            }

            //N 
            if (row != 0)
            {
                Grid tempGrid = new Grid(row - 1, col);
                temp = src[row, col] - src[row - 1, col];
                if (temp > max)
                {
                    minGrid = tempGrid;
                    minGrids.Clear();
                    minGrids.Add(minGrid);
                    max = temp;
                }
                else if (temp == max)
                {
                    minGrids.Add(tempGrid);
                }
            }

            //E
            if (col != (colCount - 1))
            {
                Grid tempGrid = new Grid(row, col + 1);
                temp = src[row, col] - src[row, col + 1];
                if (temp > max)
                {
                    minGrid = tempGrid;
                    minGrids.Clear();
                    minGrids.Add(minGrid);
                    max = temp;
                }
                else if (temp == max)
                {
                    minGrids.Add(tempGrid);
                }
            }
            //NE
            if (row != 0 && col != (colCount - 1))
            {
                Grid tempGrid = new Grid(row - 1, col + 1);
                temp = (src[row, col] - src[row - 1, col + 1]) / Sqrt2;
                if (temp > max)
                {
                    minGrid = tempGrid;
                    minGrids.Clear();
                    minGrids.Add(minGrid);
                    max = temp;
                }
                else if (temp == max)
                {
                    minGrids.Add(tempGrid);
                }
            }
            //NW
            if (row != 0 && col != 0)
            {
                Grid tempGrid = new Grid(row - 1, col - 1);
                temp = (src[row, col] - src[row - 1, col - 1]) / Sqrt2;
                if (temp > max)
                {
                    minGrid = tempGrid;
                    minGrids.Clear();
                    minGrids.Add(minGrid);
                    max = temp;
                }
                else if (temp == max)
                {
                    minGrids.Add(tempGrid);
                }
            }
            //W
            if (col != 0)
            {
                Grid tempGrid = new Grid(row, col - 1);
                temp = src[row, col] - src[row, col - 1];
                if (temp > max)
                {
                    minGrid = tempGrid;
                    minGrids.Clear();
                    minGrids.Add(minGrid);
                    max = temp;
                }
                else if (temp == max)
                {
                    minGrids.Add(tempGrid);
                }
            }
            //SW
            if (row != (rowCount - 1) && col != 0)
            {
                Grid tempGrid = new Grid(row + 1, col - 1);
                temp = (src[row, col] - src[row + 1, col - 1]) / Sqrt2;
                if (temp > max)
                {
                    minGrid = tempGrid;
                    minGrids.Clear();
                    minGrids.Add(minGrid);
                    max = temp;
                }
                else if (temp == max)
                {
                    minGrids.Add(tempGrid);
                }
            }

            return minGrid;

            //if (minGrids.Count == 1)
            //{
            //    return minGrid;
            //}
            //else
            //{
            //    double max1=double.MinValue;
            //    foreach (Grid grid in minGrids)
            //    {
            //        double maxValue = double.MinValue;
            //        Grid tempGrid = GetMin(rowCount, colCount, grid.i, grid.j, src, ref maxValue);
            //        if (maxValue > max1)
            //        {
            //            max1 = maxValue;
            //        }
            //        minGrid = tempGrid;
            //    }
            //}
        }

        #endregion

        #region 河网相关参数计算
        private void GetMainRiver()
        {
            RasterReader reader = new RasterReader(@"E:\Project\河网\DEM.tif");

            ShpReader shp = new ShpReader(@"E:\Project\新建文件夹\河网矢量clip.shp");
            List<OSGeo.OGR.Feature> maxFeature = new List<OSGeo.OGR.Feature>();
            List<Geometry> geoList = null;
            List<Point2d> pts1 = GetRiverPoints(out geoList);
            List<Dictionary<Point2d, Geometry>> dic = new List<Dictionary<Point2d, Geometry>>();
            GetLineByPoint(pts1[0], geoList, ref dic);
            Dictionary<Point2d, Geometry> maxGeoLength = null;
            double maxLength = 0;
            List<Point3d> gradientPoints = new List<Point3d>();
            List<Point3d> gradientPoints1 = new List<Point3d>();
            foreach (var geoLength in dic)
            {
                double tempLength = 0;
                List<Point2d> pointListLength = new List<Point2d>();
                gradientPoints = new List<Point3d>();
                foreach (var length in geoLength)
                {
                    int pcount = length.Value.GetPointCount();
                    for (int i = 0; i < pcount; i++)
                    {
                        Point2d temp = new Point2d(length.Value.GetX(i), length.Value.GetY(i));
                        if (!pointListLength.Contains(temp))
                        {
                            pointListLength.Add(temp);
                            gradientPoints.Add(new Point3d(length.Value.GetX(i), length.Value.GetY(i), GetElevationByPointInDEM(temp, reader)));
                        }
                    }
                }
                for (int i = 1; i < gradientPoints.Count; i++)
                {
                    tempLength += GetLength(gradientPoints[i - 1], gradientPoints[i]);
                }
                if (maxLength < tempLength)
                {
                    maxLength = tempLength;
                    maxGeoLength = geoLength;
                    gradientPoints1 = gradientPoints;
                }
            }
            List<Point2d> pointList = new List<Point2d>();
            gradientPoints = new List<Point3d>();
            foreach (var item in maxGeoLength)
            {
                for (int i = 0; i < item.Value.GetPointCount(); i++)
                {
                    Point2d temp = new Point2d(item.Value.GetX(i), item.Value.GetY(i));
                    if (!pointList.Contains(temp))
                    {
                        pointList.Add(temp);
                        gradientPoints.Add(new Point3d(item.Value.GetX(i), item.Value.GetY(i), GetElevationByPointInDEM(temp, reader)));
                    }
                }
            }
            double J = GetLonGradient(gradientPoints1);
            CreateShp(maxGeoLength, shp.SpatialRef);

            List<Point3d> slopePoints = new List<Point3d>();
            foreach (var singleGeo in maxGeoLength)
            {
                Point3d p = new Point3d(singleGeo.Key.X, singleGeo.Key.Y, GetElevationByPointInDEM(new Point2d(singleGeo.Key.X, singleGeo.Key.Y), reader));
                slopePoints.Add(p);
            }

            //ShpReader polygon = new ShpReader(@"E:\Project\demTest\流域面3.shp");
            //List<Point2d> polygonList = GetPointsByPolygon(polygon);
            //double[,] re = ResetRaster(polygonList,reader);
            double avgLength = 0;
            double avgSlope = 0;
            CalAvgSlopeLength(slopePoints, reader, ref avgLength, ref avgSlope);
            //WriteText("主河长：" + maxLength.ToString("f4") + "；纵降比：" + J.ToString("f8"));
            //WriteText("平均坡长：" + avgLength.ToString("f3") + "；平均坡度：" + avgSlope.ToString("f6"));
        }

        private void CreateShp(Dictionary<Point2d, Geometry> geoList, OSGeo.OSR.SpatialReference srt)
        {
            //注册ogr库
            string pszDriverName = "ESRI Shapefile";
            OSGeo.OGR.Ogr.RegisterAll();

            //调用对shape文件读写的Driver接口
            OSGeo.OGR.Driver poDriver = OSGeo.OGR.Ogr.GetDriverByName(pszDriverName);
            if (poDriver == null)
            {
                //MessageBox.Show("驱动错误！");
            }
            //创建河网shp
            string shpPath = "E:\\Project\\demTest\\test" + Guid.NewGuid() + ".shp";

            OSGeo.OGR.DataSource poDs;
            poDs = ShpHelp.GetShpDriver().CreateDataSource(shpPath, null);

            //创建图层
            OSGeo.OGR.Layer poLayer;
            poLayer = poDs.CreateLayer("rivernetline.shp", srt, wkbGeometryType.wkbLineString, null);
            //创建一个Feature
            OSGeo.OGR.Feature feature = new OSGeo.OGR.Feature(poLayer.GetLayerDefn());
            foreach (var geo in geoList)
            {
                feature.SetGeometry(geo.Value);
                poLayer.CreateFeature(feature);
            }
            feature.Dispose();
            poDs.Dispose();
        }

        private double GetElevationByPointInDEM(Point2d sourcePoint, RasterReader raster)
        {
            double[] adfGeoTransform = new double[6];
            raster.DataSet.GetGeoTransform(adfGeoTransform);
            double dTemp = adfGeoTransform[1] * adfGeoTransform[5] - adfGeoTransform[2] * adfGeoTransform[4];
            double dRow = 0;
            double dCol = 0;
            dCol = (adfGeoTransform[5] * (sourcePoint.X - adfGeoTransform[0]) - adfGeoTransform[2] * (sourcePoint.Y - adfGeoTransform[3])) / dTemp + 0.5;
            dRow = (adfGeoTransform[1] * (sourcePoint.Y - adfGeoTransform[3]) - adfGeoTransform[4] * (sourcePoint.X - adfGeoTransform[0])) / dTemp + 0.5;
            int c = Convert.ToInt32(dCol);
            int r = Convert.ToInt32(dRow);
            return ReadBand(raster, c, r);
        }

        private double GetLonGradient(List<Point3d> points)
        {
            double length = 0;
            double z = 0;
            double minElevation = points[0].Z;
            for (int i = 1; i < points.Count; i++)
            {
                if (minElevation > points[i].Z) minElevation = points[i].Z;
                Point3d point1 = points[i - 1];
                Point3d point2 = points[i];
                length += Length(point1, point2);
                z += Length(point1, point2) * (point1.Z + point2.Z);
            }
            //z = z - 2 * minElevation * length;
            return z / (length * length);
        }

        private List<Point2d> GetRiverPoints(out List<OSGeo.OGR.Geometry> featureList)
        {
            ShpReader shp = new ShpReader(@"E:\Project\新建文件夹\河网矢量clip.shp");
            OSGeo.OGR.Feature ofea;
            List<Point2d> linePoint = new List<Point2d>();
            featureList = new List<OSGeo.OGR.Geometry>();
            while (((ofea = shp.layer.GetNextFeature()) != null))
            {
                int count = ofea.GetGeometryRef().GetPointCount();
                Point2d start = new Point2d(ofea.GetGeometryRef().GetX(0), ofea.GetGeometryRef().GetY(0));
                Point2d end = new Point2d(ofea.GetGeometryRef().GetX(count - 1), ofea.GetGeometryRef().GetY(count - 1));
                if (!linePoint.Contains(start))
                {
                    linePoint.Add(start);
                }
                if (!linePoint.Contains(end))
                {
                    linePoint.Add(end);
                }

                featureList.Add(ofea.GetGeometryRef());
            }
            return linePoint.OrderBy(t => t.Y).ToList();
        }

        private void GetLineByPoint(Point2d sourcePoint, List<Geometry> geoList, ref List<Dictionary<Point2d, Geometry>> dic)
        {
            Geometry geo = null;
            Dictionary<Point2d, Geometry> temp = null;
            if (dic != null && dic.Count > 0)
            {
                foreach (var dicItem in dic)
                {
                    foreach (var dicGeo in dicItem)
                    {
                        if (dicGeo.Key.X == sourcePoint.X && dicGeo.Key.Y == sourcePoint.Y)
                        {
                            geo = dicGeo.Value;
                            temp = dicItem;
                            break;
                        }
                    }
                }
            }
            if (temp == null)
            {
                temp = new Dictionary<Point2d, Geometry>();
                dic.Add(temp);
            }
            Dictionary<Point2d, Geometry> temp1 = CopyDicGeometry(temp);
            int index = 0;
            List<Geometry> copyGeoList = new List<Geometry>();
            copyGeoList.AddRange(geoList);
            foreach (var GeoItem in copyGeoList)
            {
                Point2d findOutPoint = new Point2d(0, 0);
                Point2d startp = new Point2d(GeoItem.GetX(0), GeoItem.GetY(0));
                Point2d endp = new Point2d(GeoItem.GetX(GeoItem.GetPointCount() - 1), GeoItem.GetY(GeoItem.GetPointCount() - 1));
                if (GeometryIsContainPoint(sourcePoint, GeoItem))
                {
                    if (startp == sourcePoint && ((geo != null && !GeometryIsContainPoint(endp, geo)) || geo == null))
                    {
                        findOutPoint = endp;
                        index++;
                    }
                    else if (sourcePoint == endp && ((geo != null && !GeometryIsContainPoint(startp, geo)) || geo == null))
                    {
                        findOutPoint = startp;
                        index++;
                    }
                    if (findOutPoint != new Point2d(0, 0) && !temp.ContainsKey(findOutPoint))
                    {
                        Dictionary<Point2d, Geometry> copyTemp = CopyDicGeometry(temp1);
                        copyTemp.Add(findOutPoint, GeoItem);
                        geoList.Remove(GeoItem);
                        if (dic.Contains(temp))
                        {
                            dic.Remove(temp);
                        }
                        dic.Add(copyTemp);
                        GetLineByPoint(findOutPoint, geoList, ref dic);
                    }
                }
            }
        }

        /// <summary>
        /// 计算平均坡度和坡长
        /// </summary>
        /// <param name="point"></param>
        /// <param name="reader"></param>
        /// <param name="avglength"></param>
        /// <param name="avgslope"></param>
        public void CalAvgSlopeLength(List<Point3d> points, RasterReader reader, ref double avglength, ref double avgslope)
        {
            double totalLength = 0;
            double totalSlope = 0;
            List<Point3d> outPoints = new List<Point3d>();
            if (points != null && points.Count > 0)
            {
                int count = 0;
                for (int i = 0; i < points.Count; i++)
                {
                    Point3d startpt = points[i];
                    List<Point3d> resultPt = GetMaxSlopePoints(points[i], reader);
                    double singleLength = 0;
                    double singleSlope = 0;
                    for (int j = 0; j < resultPt.Count; j++)
                    {
                        Point3d point1 = resultPt[i];
                        Point3d point2 = resultPt[resultPt.Count - 1];
                        singleLength += GetLength(point1, point2);
                    }
                    singleSlope = Math.Abs((startpt.Z - resultPt[resultPt.Count - 1].Z) / (Length(startpt, resultPt[resultPt.Count - 1])));
                    totalSlope += singleSlope;
                    totalLength += singleLength;
                    count = points.Count * resultPt.Count;
                }
                avglength = totalLength / count;
                avgslope = totalSlope / points.Count;
            }
        }

        /// <summary>
        /// 求两点距离
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        private double Length(Point3d from, Point3d to)
        {
            //double l = SpatialAnalysis.CaculateGCDistance(from.Y, from.X, to.Y, to.X);
            double l = Math.Sqrt((to.X - from.X) * (to.X - from.X) + (to.Y - from.Y) * (to.Y - from.Y));
            return l;
        }

        private double GetLength(Point3d startPoint, Point3d endPoint)
        {
            double hLength = Length(startPoint, endPoint);
            double zLength = endPoint.Z - startPoint.Z;
            double sLength = zLength * zLength + hLength * hLength;
            return Math.Sqrt(sLength);
        }

        private List<Point3d> GetMaxSlopePoints(Point3d point, RasterReader reader)
        {
            double cellSizeX = reader.CellSizeX;
            double cellSizeY = reader.CellSizeY;

            int row = reader.RowCount / 50;
            int col = reader.ColumnCount / 50;

            double[] adfGeoTransform = new double[6];
            reader.DataSet.GetGeoTransform(adfGeoTransform);

            int _maxR = 0;
            int _maxC = 0;
            List<Point3d> outresult = new List<Point3d>();
            //遍历周围八个点，取最大值
            List<double> dstDic = new List<double>();
            for (int r = 0; r < row; r++)
            {
                for (int c = 0; c < col; c++)
                {
                    double center = ReadBand(reader, c, r);
                    if (center != reader.Nodata)
                    {
                        //东方,列+1，行不变,p(i,j)-p(i,j+1)
                        if (c + 1 < col)
                        {
                            double e = center - ReadBand(reader, c + 1, r);
                            dstDic.Add(e);
                        }
                        else
                        {
                            dstDic.Add(-1);
                        }
                        //东南,列+1，行+1,p(i,j)-p(i+1,j+1)
                        if (c + 1 < col && r + 1 < row)
                        {
                            double es = (center - ReadBand(reader, c + 1, r + 1)) / Math.Sqrt(2);
                            dstDic.Add(es);
                        }
                        else
                        {
                            dstDic.Add(-1);
                        }
                        //南，列+0，行+1
                        if (r + 1 < row)
                        {
                            double s = center - ReadBand(reader, c, r + 1);
                            dstDic.Add(s);
                        }
                        else
                        {
                            dstDic.Add(-1);
                        }
                        //西南
                        if (0 < c - 1 && 0 < r - 1)
                        {
                            double ws = (center - ReadBand(reader, c - 1, r - 1)) / Math.Sqrt(2);
                            dstDic.Add(ws);
                        }
                        else
                        {
                            dstDic.Add(-1);
                        }
                        //西
                        if (0 < c - 1)
                        {
                            double w = center - ReadBand(reader, c - 1, r);
                            dstDic.Add(w);
                        }
                        else
                        {
                            dstDic.Add(-1);
                        }
                        //西北
                        if (0 < c - 1 && 0 < r - 1)
                        {
                            double wn = (center - ReadBand(reader, c - 1, r - 1)) / Math.Sqrt(2);
                            dstDic.Add(wn);
                        }
                        else
                        {
                            dstDic.Add(-1);
                        }
                        //北
                        if (0 < r - 1)
                        {
                            double n = center - ReadBand(reader, c, r - 1);
                            dstDic.Add(n);
                        }
                        else
                        {
                            dstDic.Add(-1);
                        }
                        //东北
                        if (0 < r - 1 && c + 1 < col)
                        {
                            double en = (center - ReadBand(reader, c + 1, r - 1)) / Math.Sqrt(2);
                            dstDic.Add(en);
                        }
                        else
                        {
                            dstDic.Add(-1);
                        }

                        int result = 0;
                        //判断该点水流方向
                        var maxTime = dstDic.Select(x => x).Max();
                        int dir = dstDic.FindIndex(x => x == maxTime) + 1;
                        if (dir == 3)
                        {
                            result = 4;//南
                            _maxR = r + 1;
                            _maxC = c;
                        }
                        else if (dir == 2)
                        {
                            result = 2;//东南
                            _maxR = r + 1;
                            _maxC = c + 1;
                        }
                        else if (dir == 7)
                        {
                            result = 64;//北
                            _maxR = r - 1;
                            _maxC = c;
                        }

                        else if (dir == 1)
                        {
                            result = 1;//东
                            _maxR = r;
                            _maxC = c + 1;
                        }

                        else if (dir == 8)
                        {
                            result = 128;//东北
                            _maxR = r - 1;
                            _maxC = c + 1;
                        }

                        else if (dir == 6)
                        {
                            result = 32;//西北
                            _maxR = r - 1;
                            _maxC = c - 1;
                        }

                        else if (dir == 5)
                        {
                            result = 16;//西
                            _maxR = r;
                            _maxC = c - 1;
                        }
                        else if (dir == 4)
                        {
                            result = 8;//西南
                            _maxR = r + 1;
                            _maxC = c - 1;
                        }
                        else
                        {
                            result = 0;
                            _maxR = r;
                            _maxC = c;
                        }
                        double px = point.X + r * adfGeoTransform[1] + c * adfGeoTransform[2];
                        double py = point.Y + r * adfGeoTransform[4] + c * adfGeoTransform[5];
                        double pz = ReadBand(reader, _maxR, _maxC);//_globeView.GetElevation(px, py);
                        if (pz != reader.Nodata)
                        {
                            Point3d maxPoint = new Point3d(px, py, pz);
                            outresult.Add(maxPoint);
                        }
                        //Application.DoEvents();
                    }
                }/* end for c */
                //Application.DoEvents();
            }/* enf for r */
            return outresult;
        }

        private Dictionary<Point2d, Geometry> CopyDicGeometry(Dictionary<Point2d, Geometry> source)
        {
            Dictionary<Point2d, Geometry> result = new Dictionary<Point2d, Geometry>();
            if (source != null && source.Count > 0)
            {
                foreach (var item in source)
                {
                    result.Add(item.Key, item.Value);
                }
            }
            return result;
        }

        private Boolean GeometryIsContainPoint(Point2d point, Geometry geo)
        {
            for (int a = 0; a < geo.GetPointCount(); a++)
            {
                if (point.X == geo.GetX(a) && point.Y == geo.GetY(a))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 沟道流速系数计算
        /// 坡面流速系数计算
        /// R=(1.02*26 + 1.00*10 + 0.83*7+0.93*10)/(26+10+10+7) = 0.97
        /// </summary>
        public double FlowVelocity(string shppath, string inputDemPath, string outDemPath)
        {
            //按范围裁剪栅格
            ImgCut.CutTiff(shppath, inputDemPath, outDemPath);

            //读取裁剪后的栅格
            RasterReader raster = new RasterReader(outDemPath);

            int row = raster.RowCount;
            int col = raster.ColumnCount;
            int count = raster.RasterCount;

            int xsize = raster.DataSet.RasterXSize;
            int ysize = raster.DataSet.RasterYSize;

            Band band = raster.DataSet.GetRasterBand(1);

            //无效值
            double nodatavalue;
            int hasval;
            band.GetNoDataValue(out nodatavalue, out hasval);

            double[] readData = new double[row * col];
            band.ReadRaster(0, 0, xsize, ysize, readData, row, col, 0, 0);
            var res = readData.GroupBy(t => t).Select(t => new { count = t.Count(), Key = t.Key }).ToArray();
            double total = 0;
            double totalcount = 0;
            foreach (var s in res)
            {
                if (total != nodatavalue)
                {
                    total += s.Key * s.count;
                    totalcount += s.count;
                }
            }
            double R = total / totalcount;
            return R;
        }

        private List<Point2d> GetPointsByPolygon(ShpReader shp)
        {
            OSGeo.OGR.Feature ofea;
            List<Point2d> ptlist = new List<Point2d>();
            while (((ofea = shp.layer.GetNextFeature()) != null))
            {
                int count = ofea.GetGeometryRef().GetPointCount();
                string a = "";
                ofea.GetGeometryRef().ExportToWkt(out a);
                Regex reg = new Regex(@"\(([^)&^(]*)\)");
                Match m = reg.Match(a);
                string pointstr = "";
                if (m.Success)
                {
                    pointstr = m.Result("$1").TrimStart('(').TrimEnd(')');
                }
                for (int i = 0; i < pointstr.Split(',').Length; i++)
                {
                    string[] point = pointstr.Split(',')[i].Split(' ');
                    double x = double.Parse(point[0]);
                    double y = double.Parse(point[1]);
                    Point2d p = new Point2d(x, y);
                    ptlist.Add(p);
                }
            }
            return ptlist;
        }

        /// <summary>
        /// 暴雨损失参数计算
        /// </summary>
        /// <param name="shppath"></param>
        /// <param name="inputDemPath"></param>
        /// <param name="outDemPath"></param>
        public void RainLoss(GlobeView globeView, string shppath, string inputDemPath, string outDemPath, List<Point2d> pointlist)
        {
            //按范围裁剪栅格
            ImgCut.CutTiff(shppath, inputDemPath, outDemPath);

            //读取裁剪后的栅格
            RasterReader raster = new RasterReader(inputDemPath);

            int row = raster.RowCount;
            int col = raster.ColumnCount;
            int count = raster.RasterCount;

            int xsize = raster.DataSet.RasterXSize;
            int ysize = raster.DataSet.RasterYSize;

            Band band = raster.DataSet.GetRasterBand(1);

            //无效值
            double nodatavalue;
            int hasval;
            band.GetNoDataValue(out nodatavalue, out hasval);

            double[] readData = new double[row * col];
            band.ReadRaster(0, 0, xsize, ysize, readData, row, col, 0, 0);

            var res = readData.GroupBy(t => t).Select(t => new { count = t.Count(), Key = t.Key }).ToArray();
            double total = 0;
            double totalcount = 0;
            foreach (var s in res)
            {
                if (s.Key != nodatavalue)
                {
                    total += s.Key * s.count;
                    totalcount += s.count;
                }
            }
            double F = 0;
            double sf = 0;
            WatershedArea(globeView, pointlist, inputDemPath, ref F, ref sf);
            //double R = total / totalcount;//暴雨损失系数
            //double r = 0;//暴雨损失指数 数据无法提供，
            //double N = 1 / (1 + 0.016 * sf * 0.6);//折减系数
            //WriteText("暴雨损失系数：" + R.ToString("f3") + "；\r\n暴雨损失指数：" + r + "；\r\n折减系数：" + N.ToString("f6") + "；\r\n投影面积：" + F.ToString("f3") + "；\r\n表面积：" + sf.ToString("f3"));
        }

        /// <summary>
        /// 流域面积计算
        /// 流域面积F，单位平方千米
        /// </summary>
        private TerrainPoint[,] _terrainTile;
        private void WatershedArea(GlobeView _globeView, List<Point2d> pointlist, string rasterpath, ref double _resultProjectArea, ref double _resultSurfaceArea)
        {
            if (pointlist.Count >= 3)
            {
                // 外接矩阵，边界取经纬度范围的反值
                GeoRect _analysisRect = new GeoRect(-90, 90, 180, -180);
                List<double> xPoint = new List<double>();
                List<double> yPoint = new List<double>();
                for (int i = 0; i < pointlist.Count; i++)
                {
                    Point2d p = pointlist[i];
                    // 外接矩阵计算
                    xPoint.Add(p.X);
                    yPoint.Add(p.Y);
                }
                _analysisRect.North = yPoint.Max();
                _analysisRect.South = yPoint.Min();
                _analysisRect.East = xPoint.Max();
                _analysisRect.West = xPoint.Min();

                // NTS多边形
                iTelluroLib.GeoTools.Geometries.Coordinate[] coords = new iTelluroLib.GeoTools.Geometries.Coordinate[pointlist.Count + 1];

                for (int i = 0; i < pointlist.Count; i++)
                {
                    coords[i] = new iTelluroLib.GeoTools.Geometries.Coordinate(pointlist[i].X, pointlist[i].Y);
                }
                coords[pointlist.Count] = new iTelluroLib.GeoTools.Geometries.Coordinate(pointlist[0].X, pointlist[0].Y);

                //创建多边形
                iTelluroLib.GeoTools.Geometries.LinearRing ring = new iTelluroLib.GeoTools.Geometries.LinearRing(coords);
                iTelluroLib.GeoTools.Geometries.Polygon _analysisPolygon = new iTelluroLib.GeoTools.Geometries.Polygon(ring);

                RasterReader raster = new RasterReader(rasterpath);
                double _widthStep = raster.CellSizeX;
                double _heightStep = raster.CellSizeY;

                // 网格大小：行、列数
                int _widthNum = (int)((_analysisRect.East - _analysisRect.West) / _widthStep);
                int _heightNum = (int)((_analysisRect.North - _analysisRect.South) / _heightStep);

                _terrainTile = new TerrainPoint[_widthNum, _heightNum];

                //构建高程矩阵
                double[] x = new double[_widthNum * _heightNum];
                double[] y = new double[_widthNum * _heightNum];
                double[] z = new double[_widthNum * _heightNum];
                int pos = 0;
                for (int i = 0; i < _widthNum; i++)
                {
                    for (int j = 0; j < _heightNum; j++)
                    {
                        x[pos] = _analysisRect.West + i * _widthStep;
                        y[pos] = _analysisRect.South + j * _heightStep;
                        z[pos] = ReadBand(raster, i, j);
                        pos++;
                    }
                }
                pos = 0;
                for (int i = 0; i < _widthNum; i++)
                {
                    for (int j = 0; j < _heightNum; j++)
                    {
                        TerrainPoint tp = new TerrainPoint();
                        tp.Longitude = x[pos];
                        tp.Latitude = y[pos];
                        tp.Altitude = z[pos];
                        tp.col = i;
                        tp.row = j;
                        pos++;
                        _terrainTile[i, j] = tp;
                    }
                }
                double _cellArea = CaculateCellAera(_globeView, raster, _widthNum, _heightNum);

                //坡度计算
                for (int i = 0; i < _widthNum; i++)
                {
                    for (int j = 0; j < _heightNum; j++)
                    {
                        TerrainPoint tp = _terrainTile[i, j];
                        iTelluroLib.GeoTools.Geometries.Point currentPoint = new iTelluroLib.GeoTools.Geometries.Point(tp.Longitude, tp.Latitude);
                        if (_analysisPolygon.Contains(currentPoint))
                        {
                            _resultProjectArea += _cellArea;
                            //计算坡度
                            double slope = GetSlope(raster, tp);
                            double currentCellArea = _cellArea / Math.Cos(slope);
                            _resultSurfaceArea += currentCellArea;
                        }
                        //Application.DoEvents();
                    }
                    //Application.DoEvents();
                }
            }/* end if point.count>3 */
        }

        /// <summary>
        /// 单个网格的投影面积（m为单位）计算, 取计算范围的中心位置
        /// </summary>
        private double CaculateCellAera(GlobeView _globeView, RasterReader reader, int _widthNum, int _heightNum)
        {
            int ti, tj;
            ti = _widthNum / 2;
            tj = _heightNum / 2;

            iTelluro.GlobeEngine.DataSource.Geometry.Point3d p00, p01, p10;
            if (reader.IsProjected)
            {
                p00 = new Point3d(_terrainTile[ti, tj].Longitude, _terrainTile[ti, tj].Latitude, _terrainTile[ti, tj].Altitude);
                p01 = new Point3d(_terrainTile[ti, tj + 1].Longitude, _terrainTile[ti, tj + 1].Latitude, _terrainTile[ti, tj + 1].Altitude);
                p10 = new Point3d(_terrainTile[ti + 1, tj].Longitude, _terrainTile[ti + 1, tj].Latitude, _terrainTile[ti + 1, tj].Altitude);
            }
            else
            {
                double r = _globeView.GlobeViewSetting.EquatorialRadius + _terrainTile[ti, tj].Altitude;
                p00 = MathEngine.SphericalToCartesianD(Angle.FromDegrees(_terrainTile[ti, tj].Latitude), Angle.FromDegrees(_terrainTile[ti, tj].Longitude), r);
                p01 = MathEngine.SphericalToCartesianD(Angle.FromDegrees(_terrainTile[ti, tj + 1].Latitude), Angle.FromDegrees(_terrainTile[ti, tj + 1].Longitude), r);
                p10 = MathEngine.SphericalToCartesianD(Angle.FromDegrees(_terrainTile[ti + 1, tj].Latitude), Angle.FromDegrees(_terrainTile[ti + 1, tj].Longitude), r);
            }
            double area = (p00 - p01).Length * (p00 - p10).Length;
            return area;
        }

        private double GetSlope(RasterReader reader, TerrainPoint tp)
        {
            double zx = 0, zy = 0, ex = 0;
            if (tp.col - 1 > 0 && tp.row - 1 > 0 && tp.row + 1 < _terrainTile.GetLength(1) && tp.col + 1 < _terrainTile.GetLength(0))
            {
                iTelluro.GlobeEngine.DataSource.Geometry.Point3d d1 = null, d2 = null;
                if (reader.IsProjected)
                {
                    d1 = new Point3d(_terrainTile[tp.col - 1, tp.row].Longitude, tp.Latitude, tp.Altitude);
                    d2 = new Point3d(_terrainTile[tp.col + 1, tp.row].Longitude, tp.Latitude, tp.Altitude);
                }
                else
                {
                    d1 = MathEngine.SphericalToCartesianD(
                                      Angle.FromDegrees(tp.Latitude),
                                      Angle.FromDegrees(_terrainTile[tp.col - 1, tp.row].Longitude),//lon - demSpan
                                      GlobeTools.CurrentWorld.EquatorialRadius + tp.Altitude
                                      );

                    d2 = MathEngine.SphericalToCartesianD(
                                     Angle.FromDegrees(tp.Latitude),
                                     Angle.FromDegrees(_terrainTile[tp.col + 1, tp.row].Longitude),//lon + demSpan),
                                     GlobeTools.CurrentWorld.EquatorialRadius + tp.Altitude
                                     );
                }
                iTelluro.GlobeEngine.DataSource.Geometry.Point3d segment = d2 - d1;
                ex = segment.Length;
                //zx = (elv[1, 2] - elv[1, 0]) / ex;
                //zy = (elv[2, 1] - elv[0, 1]) / ex;
                zx = (_terrainTile[tp.col + 1, tp.row + 0].Altitude - _terrainTile[tp.col - 1, tp.row + 0].Altitude) / ex;
                zy = (_terrainTile[tp.col + 0, tp.row + 1].Altitude - _terrainTile[tp.col + 0, tp.row - 1].Altitude) / ex;
            }
            return Math.Atan(Math.Sqrt(zx * zx + zy * zy));
        }
        #endregion
    }

    /// <summary>
    /// 高程矩阵
    /// </summary>
    public class TerrainPoint
    {
        public double Longitude;
        public double Latitude;
        public double Altitude;
        public int row;
        public int col;
    }

    public class Grid
    {
        public int i;
        public int j;
        public Grid(int i, int j)
        {
            this.i = i;
            this.j = j;
        }
        public Grid()
        {
            i = j = 0;
        }

        public override string ToString()
        {
            return string.Format("i={0},j={1}", i, j);
        }
    }

    public enum Direct
    {
        left = 1,//左
        leftTop = 2,//左上
        top = 4,//上
        rightTop = 8,//右上
        right = 16,//右
        rigthBottom = 32,//右下
        bottom = 64,//下
        leftBottom = 128//左下
    }
}
