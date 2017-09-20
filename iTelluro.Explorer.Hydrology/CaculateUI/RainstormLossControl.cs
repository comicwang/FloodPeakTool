using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iTelluro.Explorer.Raster;
using iTelluro.GlobeEngine.DataSource.Geometry;
using iTelluro.DataTools.Utility.SHP;
using FloodPeakUtility;
using iTelluro.GlobeEngine.MapControl3D;
using FloodPeakUtility.UI;
using iTelluro.GlobeEngine.Analyst;
using System.ComponentModel.Composition;
using System.IO;
using FloodPeakUtility.Model;
using iTelluro.DataTools.Utility.Img;
using OSGeo.GDAL;
using iTelluro.GlobeEngine.Mathematics;
using iTelluro.GlobeEngine.Graphics3D;

namespace FloodPeakToolUI.UI
{
    [Export(typeof(ICaculateMemo))]
    public partial class RainstormLossControl : UserControl, ICaculateMemo
    {
        private GlobeView _globeView = null;
        private PnlLeftControl _parent = null;
        private string _xmlPath = string.Empty;
        public RainstormLossControl()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!backgroundWorker1.IsBusy)
            {
                FormOutput.AppendLog("开始计算...");
                //保存计算参数
                List<NodeModel> result = new List<NodeModel>();
                NodeModel model = fileChooseControl1.SelectedValue;
                if (model != null)
                {
                    CopyAddNodeModel(result, model);
                }

                NodeModel model1 = fileChooseControl2.SelectedValue;
                if (model1 != null)
                {
                    CopyAddNodeModel(result, model1);
                }
                _parent.OporateCaculateNode(Guids.BYSS, result.ToArray());


                //获取计算参数
                string tifPath = fileChooseControl1.FilePath;//影像路径
                string argShp = fileChooseControl2.FilePath;//流速系数
                //progressBar1.Visible = true;
                backgroundWorker1.RunWorkerAsync(new string[] {tifPath, argShp });
            }
            else
            {
                FormOutput.AppendLog("当前后台正在计算...");
            }
        }

        private void CopyAddNodeModel(List<NodeModel> lstModels, NodeModel model)
        {
            NodeModel temp = new NodeModel();
            temp.PNode = Guids.BYSS;
            temp.ShowCheck = false;
            temp.ImageIndex = model.ImageIndex;
            temp.NodeId = Guid.NewGuid().ToString();
            temp.NodeName = model.NodeName;
            temp.CanRemove = true;
            lstModels.Add(temp);
        }

        private void fileChooseControl3_OnSelectIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = File.Exists(fileChooseControl1.FilePath) && File.Exists(fileChooseControl2.FilePath);
        }

        #region 计算暴雨损失系数


        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            
            string[] args = e.Argument as string[];
            string inputDemPath = args[0];
            string outDemPath = Path.Combine(Path.GetDirectoryName(_parent.ProjectModel.ProjectPath), "WDEM1.tif");
            string shppath = args[1];
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
            RainLoss(shppath, inputDemPath, outDemPath, ptlist,ref e);

        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            double[] result = e.Result as double[];
            if (result != null && result.Length > 3)
            {
                textBox1.Text = result[0].ToString();
                textBox2.Text = result[1].ToString();
                textBox3.Text = result[2].ToString();
            }
           // progressBar1.Visible = false;
           // progressBar1.Value = 0;

        }

        /// <summary>
        /// 暴雨损失参数计算
        /// </summary>
        /// <param name="shppath"></param>
        /// <param name="inputDemPath"></param>
        /// <param name="outDemPath"></param>
        public void RainLoss(string shppath, string inputDemPath, string outDemPath, List<Point2d> pointlist, ref DoWorkEventArgs e)
        {
            FormOutput.AppendLog("按范围裁剪栅格..");
            ImgCut.CutTiff(shppath, inputDemPath, outDemPath);
            if (File.Exists(outDemPath) == false)
            {
                FormOutput.AppendLog("裁剪失败！");
                return;
            }
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
            FormOutput.AppendLog("开始计算流域面积..");
            WatershedArea(pointlist, inputDemPath, ref F);
            FormOutput.AppendLog("开始计算损失指数,折减系数..");
            double R = total / totalcount;//损失指数
            double N = 1 / (1 + 0.016 * F * 0.6);//折减系数
            FormOutput.AppendLog("流域面积:" + F.ToString("f3") + "损失指数：" + R.ToString("f3") + "；折减系数：" + N.ToString("f3"));
            e.Result = new double[] { F, R, N };
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
        /// 流域面积计算
        /// 流域面积F，单位平方千米
        /// </summary>
        private TerrainPoint[,] _terrainTile;
        private void WatershedArea(List<Point2d> pointlist, string rasterpath, ref double _resultSurfaceArea)
        {
            if (pointlist.Count >= 3)
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
            p00 = MathEngine.SphericalToCartesianD(Angle.FromDegrees(_terrainTile[ti, tj].Latitude), Angle.FromDegrees(_terrainTile[ti, tj].Longitude), r);
            p01 = MathEngine.SphericalToCartesianD(Angle.FromDegrees(_terrainTile[ti, tj + 1].Latitude), Angle.FromDegrees(_terrainTile[ti, tj + 1].Longitude), r);
            p10 = MathEngine.SphericalToCartesianD(Angle.FromDegrees(_terrainTile[ti + 1, tj].Latitude), Angle.FromDegrees(_terrainTile[ti + 1, tj].Longitude), r);
            double area = (p00 - p01).Length * (p00 - p10).Length;
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

        #endregion

        #region 实现组件接口

        /// <summary>
        /// 组件ID
        /// </summary>
        public string CaculateId
        {
            get { return Guids.BYSS; }
        }

        /// <summary>
        /// 组件名称
        /// </summary>
        public string CaculateName
        {
            get { return "暴雨损失参数计算"; }
        }

        /// <summary>
        /// 组件描述
        /// </summary>
        public string Discription
        {
            get { return "计算暴雨损失参数模块"; }
        }

        /// <summary>
        /// 加载组件
        /// </summary>
        /// <param name="Globe"></param>
        /// <param name="Parent"></param>
        public void LoadPlugin(iTelluro.GlobeEngine.MapControl3D.GlobeView Globe, PnlLeftControl Parent)
        {
            _globeView = Globe;
            _parent = Parent;
            this.Dock = DockStyle.Fill;
            //绑定控制台输出
            //textBox4.BindConsole();
            //绑定数据源，显示查询条件
            NodeModel[] nodes= Parent.ProjectModel.Nodes.Where(t => t.PNode == Guids.BYSS).ToArray();
            fileChooseControl1.BindSource(Parent, (nodes != null && nodes.Count() > 0)?nodes[0].NodeName:string.Empty);
            fileChooseControl2.BindSource(Parent, (nodes != null && nodes.Count() > 1) ? nodes[1].NodeName : string.Empty);

            //显示之前的结果
            _xmlPath = Path.Combine(Path.GetDirectoryName(Parent.ProjectModel.ProjectPath), ConfigNames.RainStormLoss + ".xml");
            if (File.Exists(_xmlPath))
            {
                BYSSResult result = XmlHelper.Deserialize<BYSSResult>(_xmlPath);
                if (result != null)
                {
                    textBox1.Text = result.AreaR == 0 ? "" : result.AreaR.ToString();
                    textBox2.Text = result.LossR == 0 ? "" : result.LossR.ToString();
                    textBox3.Text = result.SubN == 0 ? "" : result.SubN.ToString();
                }
            }
            
            Parent.UIParent.Controls.Add(this);
        }

        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            //获取结果值
            BYSSResult result = new BYSSResult()
            {
                AreaR = string.IsNullOrEmpty(textBox1.Text) ? 0 : Convert.ToDouble(textBox1.Text),
                LossR = string.IsNullOrEmpty(textBox2.Text) ? 0 : Convert.ToDouble(textBox2.Text),
                SubN = string.IsNullOrEmpty(textBox3.Text) ? 0 : Convert.ToDouble(textBox3.Text)
            };
            XmlHelper.Serialize<BYSSResult>(result, _xmlPath);
            MsgBox.ShowInfo("保存成功！");
        }

    }
}
