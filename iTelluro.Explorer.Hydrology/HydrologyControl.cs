using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iTelluro.GlobeEngine.MapControl3D;
using iTelluro.DataTools.Utility.SHP;
using OSGeo.OGR;
using iTelluro.GlobeEngine.DataSource.Geometry;
using iTelluro.GlobeEngine.Mathematics;
using iTelluro.GlobeEngine.Analyst;
using iTelluro.Explorer.VectorLoader.Utility;
using iTelluro.GlobeEngine.DataSource.Layer;
using System.IO;
using iTelluro.Explorer.Raster;
using FloodPeakToolUI.UI;
using iTelluro.GlobeEngine.Graphics3D;
using iTelluro.GlobeEngine.Graphics3D.Terrain;
using FloodPeakToolUI;
using OSGeo.GDAL;
using iTelluro.GlobeEngine.DataSource.Data;
using iTelluro.DataTools.Utility.DEM;
using iTelluro.DataTools.Utility.Img;
using iTelluro.DataTools.Utility.GIS;

namespace iTelluro.Explorer.Hydrology
{
    /// <summary>
    /// 水文分析控件
    /// </summary>
    public partial class HydrologyControl : UserControl
    {
        private GlobeView _globeView;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="globeView"></param>
        public HydrologyControl(GlobeView globeView)
        {
            InitializeComponent();
            _globeView = globeView;
            //加载河流数据
            LoadLayers();
        }

        private void LoadLayers()
        {
            //_shpLoader = new ShpLayerLoader(_globeView);
            DataLayerList data = _globeView.GlobeLayers.DataLayers;
            for (int i = 0; i < data.Count; i++)
            {
                DataLayer lyr = data[i];
            }
        }

        private void linkLblLocation_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _globeView.GlobeCamera.FlyTo(102.1, 29.6);
        }
        
        /// <summary>
        /// 计算河流长度
        /// 主河道长度L，单位千米
        /// </summary>
        private double FlowLength(string shppath)
        {
            ShpReader shp = new ShpReader(shppath);
            OSGeo.OGR.Feature ofea;
            double sumLength = 0;
            while (((ofea = shp.layer.GetNextFeature()) != null))
            {
                int id = ofea.GetFID();
                double length = 0;
                //点集个数
                int count = ofea.GetGeometryRef().GetPointCount();
                for (int i = 1; i < count; i++)
                {
                    double startLon = ofea.GetGeometryRef().GetX(i - 1);
                    double startLat = ofea.GetGeometryRef().GetY(i - 1);
                    double endLon = ofea.GetGeometryRef().GetX(i);
                    double endLat = ofea.GetGeometryRef().GetY(i);
                    //float elevation = _globeView.GetElevation(lon, lat);
                    length += Length(new Point3d(startLon, startLat, 0), new Point3d(endLon, endLat, 0));
                }
                sumLength += length;
                //WriteText(string.Format("河道id为:{0}的长度是{1,7:f3} m", id, length.ToString("f3")));
            }
            return sumLength;
        }
        
        private void GetMainRiver(Point3d startPoint,ref double maxLenth,ref List<Point3d> maxLengthPoint)
        {
            ShpReader shp = new ShpReader(@"E:\Project\测试数据\河网矢量.shp");
            OSGeo.OGR.Feature ofea;
            while (((ofea = shp.layer.GetNextFeature()) != null))
            {
                int count = ofea.GetGeometryRef().GetPointCount();
                List<Point3d> p3dList = new List<Point3d>();
                for (int i = 0; i < count; i++)
                {
                    Point3d p=new Point3d(ofea.GetGeometryRef().GetX(i), ofea.GetGeometryRef().GetY(i), ofea.GetGeometryRef().GetZ(i));
                    p3dList.Add(p);
                }
                if (p3dList.Contains(startPoint))
                {
                    double currLength = Length(startPoint,p3dList[count-1]);
                    if (currLength>maxLenth)
                    {
                        maxLenth = currLength;
                        maxLengthPoint.Add(p3dList[count - 1]);
                        GetMainRiver(p3dList[count - 1], ref maxLenth, ref maxLengthPoint);
                    }
                }
            }
        }
        
        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Point3d p = new Point3d(479176.40060202824, 3706620.1535430113, 0);
            double max = 0;
            List<Point3d> maxPoint = new List<Point3d>();
            GetMainRiver(p,ref max,ref maxPoint);
            WriteText("主河槽长度："+max.ToString("f3"));
        }

        /// <summary>
        /// 流域平均坡度
        /// </summary>
        /// 流域平均高程 Z = (f1Z1 + f2Z2 +...+ fnZn) / F (f:两等高线之间面积；Z:两等高线之间平均高程) 
        /// 流域平均坡度 S = (Z/F) * 0.5 * (L1 + L2 +...+ Ln) (Z:流域平均高程，F:流域总面积，L:两等高线之间的流域长度)
        private void Slope(double lon, double lat)
        {
            //Math.PI
            WriteText("坡度计算：");
            double slope = 0;
            double aspect = 0;
            double alt = 0;
            SpatialAnalysis.CaculateSlope(lon, lat, out slope, out aspect, out alt);
            string aspectDes = SpatialAnalysis.CaculateAspectDes(aspect);
            WriteText("点" + lon.ToString("f3") + "," + lat.ToString("f3") + "\r\n坡度：" + slope.ToString("f3") + "\r\n坡向：" + aspect.ToString("f3") + "\r\n坡向描述：" + aspectDes);
        }

        /// <summary>
        /// 流域平均坡长
        /// 分水线到主河槽的平均距离
        /// 坡长计算公式
        /// L =*
        /// </summary>
        private void SlopeLength()
        {
            double flowacc = 0;
            double cellsize = 0;
            double slope = 0;
            double angle = slope * Math.PI / 180.0;
            double n = (Math.Sin(angle) / 0.0896) / (3.0 * Math.Pow(Math.Sin(angle), 0.8) + 0.56);
            double m = n * (n - 1);
            double sl = Math.Pow(((flowacc * cellsize) / 22.13), m);
        }

        /// <summary>
        /// 计算河道纵比降；
        /// 落差：河道两段的河底高程差
        /// 总落差：河源与河口凉茶的河底高程差
        /// 河床坡降：单位河长的落差叫河道纵降比
        /// j = h1-h0/l
        /// 或者 J = (h0+h1)l1 + (h1+h2)l2 +...+ (hn-1 + hn)ln/L*L 
        /// </summary>
        private double CalRiverLonGradient(string shppath)
        {
            ShpReader shp = new ShpReader(shppath);
            OSGeo.OGR.Feature ofea;
            List<Point3d> points = new List<Point3d>();
            double J = 0; 
            while (((ofea = shp.layer.GetNextFeature()) != null))
            {
                //点集个数
                int count = ofea.GetGeometryRef().GetPointCount();
                for (int i = 0; i < count; i++)
                {
                    double lon = ofea.GetGeometryRef().GetX(i);
                    double lat = ofea.GetGeometryRef().GetY(i);
                    float elevation = _globeView.GetElevation(lon, lat);
                    points.Add(new Point3d(lon, lat, elevation));
                }
            }
            //计算河道纵比降
            J = GetLonGradient(points);
            return J;
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

        private void linkLblLonGradient_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            double j=CalRiverLonGradient(@"E:\Project\河网\主河槽.shp");
            WriteText("主河道纵降比：" + j.ToString("f3"));
        }

        private void linkLblFlowLength_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            double riverlength = FlowLength(@"E:\Project\河网\主河槽.shp");
            WriteText("主河道长度：" + riverlength.ToString("f3"));
        }

        private void linkLblWatershed_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RasterReader reader = new RasterReader(@"E:\Project\河网\DEM.tif");
            double avgLength = 0;
            double avgSlope = 0;
            List<Point3d> pts = GetPoints();
            CalAvgSlopeLength(pts, reader, ref avgLength, ref avgSlope);
            WriteText("平均坡长：" + avgLength.ToString("f3") + "；平均坡度：" + avgSlope.ToString("f3"));
        }

        //取主河槽等分点
        private List<Point3d> GetPoints()
        {
            ShpReader shp = new ShpReader(@"E:\Project\河网\主河槽.shp");
            
            OSGeo.OGR.Feature ofea;
            List<Point3d> arr = new List<Point3d>();
            while (((ofea = shp.layer.GetNextFeature()) != null))
            {
                //点集个数
                int count = ofea.GetGeometryRef().GetPointCount();
                for (int i = 0; i < count; i++)
                {
                    double lon = ofea.GetGeometryRef().GetX(i);
                    double lat = ofea.GetGeometryRef().GetY(i);
                    float elevation = _globeView.GetElevation(lon, lat);
                    arr.Add(new Point3d(lon, lat, elevation));
                }
            }

            List<Point3d> pickPoint = new List<Point3d>();
            for (int i = 2; i <=5; i++)
            {
                int index=arr.Count/i;
                int index1 = 0;
                if (index < (arr.Count / 2)) 
                {
                    index1 = index + arr.Count /2;
                }
                pickPoint.Add(arr[index1]);
                pickPoint.Add(arr[index]);
            }
            return pickPoint;
        }

        /// <summary>
        /// 坡度分析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLblSlope_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            double lat = Convert.ToDouble(this.txtLat.Text);
            double lon = Convert.ToDouble(this.txtLon.Text);
            Slope(lon, lat);
            double s = 0;
            double a = 0;
            double b = 0;
            //CaculateSlope(lon, lat, out s, out a, out b);
        }

        /// <summary>
        /// 坡长分析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLblSlopeLength_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //FormCalSlope frm = new FormCalSlope(this._globeView);
            //frm.ShowDialog();

            RasterReader reader = new RasterReader(@"E:\Project\河网\DEM.tif");
            int col = reader.ColumnCount;
            int row = reader.RowCount;
            double[,] dem = new double[row, col];
            double[,] dir = new double[row, col];
           
            D8Calculate d8 = new D8Calculate(col, row, -1);
            d8.cal(dem, ref dir);
            
            double[] director = new double[col * row];
            int c=0;
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    director[c] = dir[i, j];
                    c++;
                    //WriteText("第" + i + "行，第" + j + "列的流向：" + dir[i, j]);
                    Application.DoEvents();
                }
                Application.DoEvents();
            }

            var res = director.GroupBy(t => t).Select(t => new { count = t.Count(), Key = t.Key }).ToArray();
            foreach (var s in res)
            {
                WriteText(s.Key+"，" + s.count);
            }
        }

        private void linkLblClear_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.textBox1.Text = "";
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string inputDemPath = @"E:\Project\河网\DEM.tif";
            string outDemPath = @"E:\Project\demTest\WDEM.tif";
            string shppath = @"E:\Project\demTest\流域面3.shp";
            ShpReader shp = new ShpReader(shppath);
            OSGeo.OGR.Feature ofea;
            List<Point2d> ptlist = new List<Point2d>();
            while (((ofea = shp.layer.GetNextFeature()) != null))
            {
                int count = ofea.GetGeometryRef().GetPointCount();
                for (int i = 0; i < count; i++)
                {
                    Point2d p = new Point2d(ofea.GetGeometryRef().GetX(i), ofea.GetGeometryRef().GetY(i));
                    ptlist.Add(p);
                }
            }
            RainLoss(shppath, inputDemPath, outDemPath, ptlist);
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string inputDemPath = @"E:\Project\河网\DEM.tif";
            string outDemPath = @"E:\Project\demTest\WDEM.tif";
            string shppath = @"E:\Project\demTest\流域面3.shp";
            double r = FlowVelocity(shppath, inputDemPath, outDemPath);
            WriteText("河槽流速系数：" + r.ToString("f3"));
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string inputDemPath = @"E:\Project\河网\DEM.tif";
            string outDemPath = @"E:\Project\demTest\WDEM.tif";
            string shppath = @"E:\Project\demTest\流域面3.shp";
            double r= FlowVelocity(shppath, inputDemPath, outDemPath);
            WriteText("坡面流速系数：" + r.ToString("f3"));
        }


        /// <summary>
        /// 计算线的交点
        /// 先获取每条线段的startpoint与endpoint，然后判断是否有重合
        /// </summary>
        private List<Point3d> CalPoints()
        {
            List<Point3d> srcPoints = new List<Point3d>();
            List<Point3d> interPoints = new List<Point3d>();
            //遍历线段取首尾两个点坐标
            ShpReader shp = new ShpReader(@"E:\Project\河网\主河槽.shp");
            OSGeo.OGR.Feature ofea;
            while (((ofea = shp.layer.GetNextFeature()) != null))
            {
                //点集个数
                int count = ofea.GetGeometryRef().GetPointCount();
                double startlon = ofea.GetGeometryRef().GetX(0);
                double startlat = ofea.GetGeometryRef().GetY(0);
                double startElevation = _globeView.GetElevation(startlon, startlat);
                Point3d startPoint = new Point3d(startlon, startlat, startElevation);

                if (!interPoints.Contains(startPoint))
                {
                    //元数据中包含
                    if (srcPoints.Contains(startPoint))
                    {
                        srcPoints.Remove(startPoint);
                        interPoints.Add(startPoint);
                    }
                    else
                    {
                        srcPoints.Add(startPoint);
                    }
                }

                double endlon = ofea.GetGeometryRef().GetX(count - 1);
                double endlat = ofea.GetGeometryRef().GetY(count - 1);
                double endElevation = _globeView.GetElevation(endlon, endlat);
                Point3d endPoint = new Point3d(endlon, endlat, endElevation);
                if (!interPoints.Contains(endPoint))
                {
                    //元数据中包含
                    if (srcPoints.Contains(endPoint))
                    {
                        srcPoints.Remove(endPoint);
                        interPoints.Add(endPoint);
                    }
                    else
                    {
                        srcPoints.Add(endPoint);
                    }
                }
            }
            List<Point3d> result = interPoints.Distinct<Point3d>().ToList();
            return result;
        }

        /// <summary>
        /// 计算平均坡度和坡长
        /// </summary>
        /// <param name="point"></param>
        /// <param name="reader"></param>
        /// <param name="avglength"></param>
        /// <param name="avgslope"></param>
        public void CalAvgSlopeLength(List<Point3d> points,RasterReader reader,ref double avglength, ref double avgslope)
        {
            double totalLength = 0;
            double totalSlope = 0;
            if (points != null && points.Count > 0)
            {
                for (int i = 0; i < points.Count; i++)
                {
                    Point3d startpt = points[i];
                    List<Point3d> resultPt = GetMaxSlopePoints(points[i], reader);
                    double singleLength = 0;
                    double singleSlope = 0;
                    for (int j = 1; j < resultPt.Count; j++)
                    {
                        Point3d point1 = resultPt[i - 1];
                        Point3d point2 = resultPt[i];
                        singleLength += Length(point1, point2);
                    }
                    singleSlope = Math.Asin(Math.Abs(startpt.Z - resultPt[resultPt.Count - 1].Z));
                    totalSlope += singleSlope;
                    totalLength += singleLength;
                    avglength = totalLength / points.Count;
                    avgslope = totalSlope / points.Count;
                    Application.DoEvents();
                }
            }
        }

        /// <summary>
        /// 计算当前点的最陡方向
        /// 返回最陡方向的点集
        /// D8算法
        /// </summary>
        /// <param name="point"></param>
        /// <param name="reader"></param>
        /// <param name="outresult"></param>
        private List<Point3d> GetMaxSlopePoints(Point3d point,RasterReader reader)
        {
            double cellSizeX = reader.CellSizeX;
            double cellSizeY = reader.CellSizeY;

            int row = reader.RowCount;
            int col = reader.ColumnCount;

            double[] adfGeoTransform = new double[6];
            reader.DataSet.GetGeoTransform(adfGeoTransform);

            int xoffset = ((int)((point.X - reader.MapRectWest) / cellSizeX)) + 1;
            int yoffset = ((int)((reader.MapRectNorth - point.Y) / cellSizeY)) + 1;

            int _maxR = 0;
            int _maxC = 0;
            List<Point3d> outresult=new List<Point3d>();
            //遍历周围八个点，取最大值
            List<double> dstDic = new List<double>();
            for (int r = 0; r < row; r++)
            {
                for (int c = 0; c < col; c++)
                {
                    double center = ReadBand(reader, c, r);
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
                        _maxC = c+1;
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
                        _maxR = r ;
                        _maxC = c+1;
                    }

                    else if (dir == 8)
                    {
                        result = 128;//东北
                        _maxR = r - 1;
                        _maxC = c+1;
                    }

                    else if (dir == 6)
                    {
                        result = 32;//西北
                        _maxR = r -1;
                        _maxC = c-1;
                    }

                    else if (dir == 5)
                    {
                        result = 16;//西
                        _maxR = r ;
                        _maxC = c-1;
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
                        _maxR = r ;
                        _maxC = c;
                    }
                    double px = point.X + _maxR * adfGeoTransform[1] + _maxC * adfGeoTransform[2];
                    double py = point.Y + _maxR * adfGeoTransform[4] + _maxC * adfGeoTransform[5];
                    double pz = _globeView.GetElevation(px, py);
                    Point3d maxPoint = new Point3d(px, py, pz);
                    outresult.Add(maxPoint);
                    Application.DoEvents();
                }/* end for c */
                Application.DoEvents();
            }/* enf for r */
            return outresult;
        }

        public double ReadBand(RasterReader reader, int col, int row)
        {
            double[] d = null;
            reader.ReadBand(col, row, 1, 1, out d);
            return d[0];
        }

        /// <summary>
        /// 河槽流速系数计算
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

            double[] readData = new double[row * col];
            band.ReadRaster(0, 0, xsize, ysize, readData, row, col, 0, 0);

            var res = readData.GroupBy(t => t).Select(t => new { count = t.Count(), Key = t.Key }).ToArray();
            double total = 0;
            double totalcount = 0;
            foreach (var s in res)
            {
                total += s.Key * s.count;
                totalcount += s.count;
            }
            double R = total / totalcount;
            return R;
        }

        /// <summary>
        /// 暴雨损失参数计算
        /// </summary>
        /// <param name="shppath"></param>
        /// <param name="inputDemPath"></param>
        /// <param name="outDemPath"></param>
        public void RainLoss(string shppath, string inputDemPath, string outDemPath, List<Point2d> pointlist)
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

            double[] readData = new double[row * col];
            band.ReadRaster(0, 0, xsize, ysize, readData, row, col, 0, 0);

            var res = readData.GroupBy(t => t).Select(t => new { count = t.Count(), Key = t.Key }).ToArray();
            double total = 0;
            double totalcount = 0;
            foreach (var s in res)
            {
                total += s.Key * s.count;
                totalcount += s.count;
            }
            double F = 0;
            WatershedArea(pointlist,inputDemPath,ref F);
            double R = total / totalcount;//损失指数
            double N = 1 / (1 + 0.016 * F * 0.6);//折减系数
            WriteText("损失指数：" + R.ToString("f3")+"；折减系数："+N.ToString("f3"));
        }

        /// <summary>
        /// 流域面积计算
        /// 流域面积F，单位平方千米
        /// </summary>
        private TerrainPoint[,] _terrainTile;
        private void WatershedArea(List<Point2d> pointlist, string rasterpath, ref double _resultSurfaceArea)
        {
            if (pointlist.Count>=3)
            {
                // 外接矩阵，边界取经纬度范围的反值
                GeoRect _analysisRect = new GeoRect(-90, 90, 180, -180);
                for (int i = 0; i < pointlist.Count; i++)
                {
                    Point2d p = pointlist[i];
                    // 外接矩阵计算
                    if (p.X > _analysisRect.East) _analysisRect.East = p.X;
                    if (p.X < _analysisRect.West) _analysisRect.West = p.X;
                    if (p.Y > _analysisRect.North) _analysisRect.North = p.Y;
                    if (p.Y < _analysisRect.South) _analysisRect.South = p.Y;
                }

                // NTS多边形
                iTelluroLib.GeoTools.Geometries.Coordinate[] coords =
                        new iTelluroLib.GeoTools.Geometries.Coordinate[pointlist.Count + 1];

                for (int i = 0; i < pointlist.Count; i++)
                {
                    coords[i] = new iTelluroLib.GeoTools.Geometries.Coordinate(pointlist[i].X, pointlist[i].Y);
                }
                coords[pointlist.Count] =
                    new iTelluroLib.GeoTools.Geometries.Coordinate(pointlist[0].X, pointlist[0].Y);

                // 创建多边形
                iTelluroLib.GeoTools.Geometries.Polygon _analysisPolygon =
                        new iTelluroLib.GeoTools.Geometries.Polygon(
                                                new iTelluroLib.GeoTools.Geometries.LinearRing(coords)
                                                );

                RasterReader raster = new RasterReader(rasterpath);
                // 网格大小：行、列数
                int _widthNum = (int)((_analysisRect.East - _analysisRect.West) / raster.CellSizeX);
                int _heightNum = (int)((_analysisRect.North - _analysisRect.South) / raster.CellSizeX);
                _terrainTile = new TerrainPoint[_widthNum, _heightNum];

                double _cellArea = CaculateCellAera(raster);
                _resultSurfaceArea = 0;
                double _resultProjectArea = 0;
                //坡度计算
                for (int i = 0; i < _widthNum; i++)
                {
                    for (int j = 0; j < _heightNum; j++)
                    {
                        TerrainPoint tp = _terrainTile[i, j];

                        iTelluroLib.GeoTools.Geometries.Point currentPoint =
                                    new iTelluroLib.GeoTools.Geometries.Point(
                                                                tp.Longitude, tp.Latitude);

                        if (_analysisPolygon.Contains(currentPoint))
                        {
                            _resultProjectArea += _cellArea;

                            // 计算坡度
                            double slope = GetSlope(tp);
                            double currentCellArea = _cellArea / Math.Cos(slope);
                            _resultSurfaceArea += currentCellArea;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 单个网格的投影面积（m为单位）计算, 取计算范围的中心位置
        /// </summary>
        private double CaculateCellAera(RasterReader raster)
        {
            int _widthNum = (int)((raster.MapRectEast - raster.MapRectWest) / raster.CellSizeX);
            int _heightNum = (int)((raster.MapRectNorth - raster.MapRectSouth) / raster.CellSizeY);

            int row = raster.RowCount;
            int col = raster.ColumnCount;

            int xsize = raster.DataSet.RasterXSize;
            int ysize = raster.DataSet.RasterYSize;

            Band band = raster.DataSet.GetRasterBand(1);
            double[] gt = new double[6];
            raster.DataSet.GetGeoTransform(gt);

            double[] readData = new double[row * col];
            band.ReadRaster(0, 0, xsize, ysize, readData, row, col, 0, 0);
            //构建高程矩阵
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    double x = gt[0] + i * gt[1] + j * gt[2];
                    double y = gt[3] + i * gt[4] + j * gt[5];
                    double z = _globeView.GetElevation(x, y);
                    TerrainPoint tp = new TerrainPoint();
                    tp.Longitude = x;
                    tp.Latitude = y;
                    tp.Altitude = z;
                    tp.col = i;
                    tp.row = j;
                    _terrainTile[i, j] = tp;
                }
            }

            int ti, tj;
            ti = _widthNum / 2;
            tj = _heightNum / 2;

            iTelluro.GlobeEngine.DataSource.Geometry.Point3d p00, p01, p10;
            double r = _globeView.GlobeViewSetting.EquatorialRadius + _terrainTile[ti, tj].Altitude;
            p00 = MathEngine.SphericalToCartesianD(Angle.FromDegrees(_terrainTile[ti, tj].Latitude),Angle.FromDegrees(_terrainTile[ti, tj].Longitude),r );
            p01 = MathEngine.SphericalToCartesianD(Angle.FromDegrees(_terrainTile[ti, tj + 1].Latitude),Angle.FromDegrees(_terrainTile[ti, tj + 1].Longitude),r);
            p10 = MathEngine.SphericalToCartesianD(Angle.FromDegrees(_terrainTile[ti + 1, tj].Latitude),Angle.FromDegrees(_terrainTile[ti + 1, tj].Longitude),r);
            double area=(p00 - p01).Length* (p00 - p10).Length;
            return area;
        }

        private double GetSlope(TerrainPoint tp)
        {
            double zx, zy, ex;

            iTelluro.GlobeEngine.DataSource.Geometry.Point3d d1 = MathEngine.SphericalToCartesianD(
                                        Angle.FromDegrees(tp.Latitude),
                                        Angle.FromDegrees(_terrainTile[tp.col - 1, tp.row].Longitude),//lon - demSpan
                                        GlobeTools.CurrentWorld.EquatorialRadius + tp.Altitude
                                        );

            iTelluro.GlobeEngine.DataSource.Geometry.Point3d d2 = MathEngine.SphericalToCartesianD(
                                        Angle.FromDegrees(tp.Latitude),
                                        Angle.FromDegrees(_terrainTile[tp.col + 1, tp.row].Longitude),//lon + demSpan),
                                        GlobeTools.CurrentWorld.EquatorialRadius + tp.Altitude
                                        );

            iTelluro.GlobeEngine.DataSource.Geometry.Point3d segment = d2 - d1;
            ex = segment.Length;

            //zx = (elv[1, 2] - elv[1, 0]) / ex;
            //zy = (elv[2, 1] - elv[0, 1]) / ex;
            zx = (_terrainTile[tp.col + 1, tp.row + 0].Altitude - _terrainTile[tp.col - 1, tp.row + 0].Altitude) / ex;
            zy = (_terrainTile[tp.col + 0, tp.row + 1].Altitude - _terrainTile[tp.col + 0, tp.row - 1].Altitude) / ex;

            return Math.Atan(Math.Sqrt(zx * zx + zy * zy));
        }

        private class TerrainPoint
        {
            public double Longitude;
            public double Latitude;
            public double Altitude;
            public int row;
            public int col;
        }

        /// <summary>
        /// 求两点距离
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        private double Length(Point3d from, Point3d to)
        {
            double l = SpatialAnalysis.CaculateGCDistance(from.Y, from.X, to.Y, to.X);
            return l;
        }

        //private void AddShpFile()
        //{
        //    OpenFileDialog ofd = new OpenFileDialog();
        //    ofd.Filter = "shp文件|*.shp";
        //    if (ofd.ShowDialog() == DialogResult.OK)
        //    {
        //        this.textBox3.Text = ofd.FileName;
        //        Geometry geo = ShpReader.GetFirstGeo(this.textBox3.Text);
        //        _globeView.GlobeCamera.FlyTo(geo.GetX(0), geo.GetY(0));
        //    }
        //}

        private void WriteText(string info)
        {
            if (string.IsNullOrEmpty(this.textBox1.Text))
            {
                this.textBox1.Text += info;
            }
            else
            {
                this.textBox1.Text += "\r\n" + info;
            }
        }

       
    }
}
