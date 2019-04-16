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
using System.Text.RegularExpressions;
using System.Configuration;
using FloodPeakUtility.Algorithm;

namespace FloodPeakToolUI.UI
{
    [Export(typeof(ICaculateMemo))]
    public partial class RainstormLossControl : UserControl, ICaculateMemo
    {
        private GlobeView _globeView = null;
        private PnlLeftControl _parent = null;
        private string _xmlPath = string.Empty;
        private Hydrology _hydrology = null;
        private FormRiverArg formRiver = null;
        private DateTime _currentTime = DateTime.Now;
        //当前计算的Dem路径
        private string currentDem = string.Empty;
        //填洼缓存
        private double[,] fillGrid = null;
        //流向数据缓存
        private int[,] directionDev = null;
        //流量数据缓存
        private int[,] Accumulation = null;
        //河网缓存数据
        private int[,] RiverGrid = null;

        #region 河网提取参数

        /// <summary>
        /// 获取河网阀值
        /// </summary>
        private int RiverThreshold = 0;

        /// <summary>
        /// 获取填洼Z值
        /// </summary>
        private int FillThreshold = 0;

        /// <summary>
        /// 获取河网输出路径
        /// </summary>
        private string RiverPath = string.Empty;

        /// <summary>
        /// 获取填洼输出路径
        /// </summary>
        private string FillPath = string.Empty;

        /// <summary>
        /// 获取流向输出路径
        /// </summary>
        private string DirectionPath = string.Empty;

        /// <summary>
        /// 获取流量输出路径
        /// </summary>
        private string AccumulationPath = string.Empty;

        #endregion
        public RainstormLossControl()
        {
            InitializeComponent();
            _hydrology = new Hydrology();
        }

        /// <summary>
        /// 保存计算的GIS文件参数
        /// </summary>
        private void SaveCaculateArg()
        {
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

            NodeModel model2 = fileChooseControl3.SelectedValue;
            if (model2 != null)
            {
                CopyAddNodeModel(result, model2);
            }

            NodeModel model3 = fileChooseControl4.SelectedValue;
            if (model3 != null)
            {
                CopyAddNodeModel(result, model3);
            }
            _parent.OporateCaculateNode(Guids.BYSS, result.ToArray());
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

        #region Events

        private void fileChooseControl3_OnSelectIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = File.Exists(fileChooseControl1.FilePath);
            btnGetHeWang.Enabled = File.Exists(fileChooseControl1.FilePath);
        }

        /// <summary>
        /// 计算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (!backgroundWorker1.IsBusy)
            {
                FormOutput.AppendLog("开始计算...");
                FormOutput.AppendProress(true);
                _currentTime = DateTime.Now;
                SaveCaculateArg();
                //获取计算参数
                string tifPath = fileChooseControl1.FilePath;//影像路径
                string areaShp = fileChooseControl2.FilePath;//流域面积shp
                string RShp = fileChooseControl3.FilePath;//系数R
                string rShp = fileChooseControl4.FilePath;//指数r
                //progressBar1.Visible = true;
                backgroundWorker1.RunWorkerAsync(new string[] { tifPath, areaShp, RShp, rShp });
            }
            else
            {
                FormOutput.AppendLog("当前后台正在计算...");
            }
        }


        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            //获取结果值
            BYSSResult result = new BYSSResult()
            {
                F = string.IsNullOrEmpty(textBox1.Text) ? 0 : Convert.ToDouble(textBox1.Text),
                N = string.IsNullOrEmpty(textBox2.Text) ? 0 : Convert.ToDouble(textBox2.Text),
                R = string.IsNullOrEmpty(textBox3.Text) ? 0 : Convert.ToDouble(textBox3.Text),
                r1 = string.IsNullOrEmpty(txtr1.Text) ? 0 : Convert.ToDouble(txtr1.Text)
            };
            XmlHelper.Serialize<BYSSResult>(result, _xmlPath);
            MsgBox.ShowInfo("保存成功！");
        }

        /// <summary>
        /// 河网提取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetHeWang_Click(object sender, EventArgs e)
        {
            if (!backgroundWorker1.IsBusy)
            {
                //获取河网参数
                if (formRiver == null)
                    formRiver = new FormRiverArg();
                if (formRiver.ShowDialog() == DialogResult.OK)
                {
                    RiverPath = formRiver.RiverPath;
                    RiverThreshold = formRiver.RiverThreshold;
                    FillPath = formRiver.FillPath;
                    DirectionPath = formRiver.DirectionPath;
                    AccumulationPath = formRiver.AccumulationPath;

                    FormOutput.AppendLog("开始计算...");
                    _currentTime = DateTime.Now;
                    SaveCaculateArg();
                    string srcPath = fileChooseControl1.FilePath;
                    //已经计算过，直接从缓存读取
                    if (currentDem == srcPath)
                    {
                        RasterReader read = new RasterReader(srcPath);
                        if (!string.IsNullOrEmpty(FillPath))
                        {
                            DEMReader.SaveDem(read, fillGrid, null, FillPath);
                        }
                        if (!string.IsNullOrEmpty(DirectionPath))
                        {
                            DEMReader.SaveDem(read, directionDev, null, DirectionPath);
                        }
                        if (!string.IsNullOrEmpty(AccumulationPath))
                        {
                            DEMReader.SaveDem(read, Accumulation, null, AccumulationPath);
                        }

                        FormOutput.AppendLog("开始根据回流阀值提取河网..");

                        FormOutput.AppendLog($"当前阀值为{RiverThreshold}..");

                        RiverGrid = new int[Accumulation.GetLength(0), Accumulation.GetLength(1)];
                        for (int i = 0; i < Accumulation.GetLength(0); i++)
                        {
                            for (int j = 0; j < Accumulation.GetLength(1); j++)
                            {
                                if (Accumulation[i, j] < RiverThreshold)
                                {
                                    RiverGrid[i, j] = -1;
                                }

                                else
                                {
                                    RiverGrid[i, j] = 1;
                                }
                            }
                        }
                        DEMReader.SaveDem(read, RiverGrid, null, RiverPath);
                        FormOutput.AppendLog("河网提取完成..");

                        FormOutput.AppendLog(string.Format("提取结束,共耗时{0}秒..", (DateTime.Now - _currentTime).TotalSeconds));
                        System.Diagnostics.Process.Start("Explorer.exe", Path.GetDirectoryName(RiverPath));
                    }
                    else
                        backgroundWorker2.RunWorkerAsync(srcPath);
                }
            }
            else
            {
                FormOutput.AppendLog("当前后台正在计算...");
            }
        }

        #endregion

        #region 计算暴雨损失系数


        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            string[] args = e.Argument as string[];
            string inputDemPath = args[0];
            string shppath = args[1];
            string Rshppath = args[2];
            string rshppath = args[3];
        
            RainLoss(Rshppath,rshppath, inputDemPath, shppath, ref e);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //F,N,R,r
            double?[] result = e.Result as double?[];
            if (result != null && result.Length >= 4)
            {
                if (result[0].HasValue)
                {
                    textBox1.Text = result[0].GetValueOrDefault(0).ToString("f3");
                    textBox2.Text = result[1].GetValueOrDefault(0).ToString("f3");
                }
                if (result[2].HasValue)
                    textBox3.Text = result[2].GetValueOrDefault(0).ToString("f3");
                if (result[3].HasValue)
                    txtr1.Text = result[3].GetValueOrDefault(0).ToString("f3");
            }
            FormOutput.AppendLog(string.Format("计算结束,共耗时{0}秒..", (DateTime.Now - _currentTime).TotalSeconds));
            FormOutput.AppendProress(false);
        }

        /// <summary>
        /// 暴雨损失参数计算
        /// </summary>
        /// <param name="shppath"></param>
        /// <param name="inputDemPath"></param>
        /// <param name="outDemPath"></param>
        public void RainLoss(string Rshppath, string rshppath, string inputDemPath, string areaShp, ref DoWorkEventArgs e)
        {
            //计算流域面积
            double F = 0;
            double? sf = null;
            double? N = null;
            if (File.Exists(areaShp))
            {
                FormOutput.AppendLog("开始计算流域面积和折减系数");
                WatershedArea(areaShp, inputDemPath, ref F, ref sf);
                sf = sf / 1000000;
                FormOutput.AppendLog(string.Format("流域表面积SF = {0}km²", sf.Value.ToString("f3")));
                F = F / 1000000;
                FormOutput.AppendLog(string.Format("流域投影面积F = {0}km²", F.ToString("f3")));
                N = 1 / (1 + 0.016 * F * 0.6);//折减系数
                FormOutput.AppendLog("折减系数N = " + N.Value.ToString("f3"));
            }

            //计算折减系数
            double? R = null;
            if (File.Exists(Rshppath))
            {
                FormOutput.AppendLog("开始计算损失系数");
                R = RasterCoefficientReader.ReadCoeficient(Rshppath, inputDemPath);
                if (R.HasValue)
                    FormOutput.AppendLog("损失系数R = " + R.Value.ToString("f3"));
            }
            double? r = null;
            if (File.Exists(rshppath))
            {
                FormOutput.AppendLog("开始计算损失指数");
                r = RasterCoefficientReader.ReadCoeficient(rshppath, inputDemPath);
                if (r.HasValue)
                    FormOutput.AppendLog("损失指数r = " + r.Value.ToString("f3"));
            }
            e.Result = new double?[] { F, N, R, r };
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
        private void WatershedArea(string shppath, string rasterpath, ref double _resultProjectArea, ref double? _resultSurfaceArea)
        {
            //获取流域面积形状以及它的外接矩形
            GeoRect _analysisRect = new GeoRect(-90, 90, 180, -180); //外接矩形
            iTelluroLib.GeoTools.Geometries.Polygon _analysisPolygon = _hydrology.CalLimitAreaPolygon(shppath, ref _analysisRect);  //流域面形状

            //在存在流域面积界定的情况下要进行裁剪筛选，不存在默认全部计算  
            RasterReader raster = new RasterReader(rasterpath);
            double _widthStep = raster.CellSizeX;
            double _heightStep = raster.CellSizeY;

            // 网格大小：行、列数
            int _widthNum = (int)((_analysisRect.East - _analysisRect.West) / _widthStep);
            int _heightNum = (int)((_analysisRect.North - _analysisRect.South) / _heightStep);

            _terrainTile = new TerrainPoint[_widthNum, _heightNum];

            //构建高程矩阵
            for (int i = 0; i < _widthNum; i++)
            {
                for (int j = 0; j < _heightNum; j++)
                {
                    TerrainPoint tp = new TerrainPoint();
                    tp.Longitude = _analysisRect.West + i * _widthStep;
                    tp.Latitude = _analysisRect.South + j * _heightStep;
                    tp.Altitude = ReadBand(raster, i, j);
                    tp.col = i;
                    tp.row = j;
                    _terrainTile[i, j] = tp;
                }
            }

            double _cellArea = CaculateCellAera(raster, _widthNum, _heightNum);
            //坡度计算
            if (_resultSurfaceArea == null)
                _resultSurfaceArea = 0;
            for (int i = 0; i < _widthNum; i++)
            {
                for (int j = 0; j < _heightNum; j++)
                {
                    TerrainPoint tp = _terrainTile[i, j];
                    iTelluroLib.GeoTools.Geometries.Point currentPoint = new iTelluroLib.GeoTools.Geometries.Point(tp.Longitude, tp.Latitude);
                    if ((_analysisPolygon != null && _analysisPolygon.Contains(currentPoint)) || _analysisPolygon == null)
                    {
                        _resultProjectArea += _cellArea;
                        //计算坡度
                        double slope = GetSlope(raster, tp);
                        double currentCellArea = _cellArea / Math.Cos(slope);
                        _resultSurfaceArea += currentCellArea;
                    }
                }
                FormOutput.AppendProress(((i + 1) * 100) / _widthNum);
            }
        }

        /// <summary>
        /// 计算影像数据单元格的投影面积
        /// （区分了文件为投影坐标还是地理坐标）
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="_widthNum"></param>
        /// <param name="_heightNum"></param>
        /// <returns></returns>
        private double CaculateCellAera(RasterReader reader, int _widthNum, int _heightNum)
        {
            try
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
            catch(Exception ex)
            {
                FormOutput.AppendLog(ex.Message);
                return 0;
            }
         
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

        public double ReadBand(RasterReader reader, int col, int row)
        {
            double[] d = null;
            reader.ReadBand(col, row, 1, 1, out d);
            return d[0];
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
            try
            {
                //绑定数据源，显示查询条件
                NodeModel[] nodes = Parent.ProjectModel.Nodes.Where(t => t.PNode == Guids.BYSS).ToArray();
                fileChooseControl1.BindSource(Parent, (nodes != null && nodes.Count() > 0) ? nodes[0].NodeName : string.Empty);
                fileChooseControl2.BindSource(Parent, (nodes != null && nodes.Count() > 1) ? nodes[1].NodeName : string.Empty);
                fileChooseControl3.BindSource(Parent, (nodes != null && nodes.Count() > 2) ? nodes[2].NodeName : string.Empty);
                fileChooseControl4.BindSource(Parent, (nodes != null && nodes.Count() > 3) ? nodes[3].NodeName : string.Empty);

                //显示之前的结果
                _xmlPath = Path.Combine(Path.GetDirectoryName(Parent.ProjectModel.ProjectPath), ConfigNames.RainStormLoss);
                if (File.Exists(_xmlPath))
                {
                    BYSSResult result = XmlHelper.Deserialize<BYSSResult>(_xmlPath);
                    if (result != null)
                    {
                        textBox1.Text = result.F == 0 ? "" : result.F.ToString();
                        textBox2.Text = result.N == 0 ? "" : result.N.ToString();
                        textBox3.Text = result.R == 0 ? "" : result.R.ToString();
                        txtr1.Text = result.r1 == 0 ? "" : result.r1.ToString();
                    }
                }
            }
            catch
            {

            }

            Parent.UIParent.Controls.Add(this);
        }

        #endregion

        #region 河网提取

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            //读取高程矩阵
            RasterReader read = new RasterReader(e.Argument as string);

            double[,] src = DEMReader.GetElevation(read);

            DEMReader.SaveDem(read, src, null, RiverPath);

            FormOutput.AppendLog("1.计算洼地");

            List<MyGrid> SourceGrid = new List<MyGrid>();

            MyGrid.Src = src;
            SourceGrid = MyGrid.GetSourceGrids();
            FormOutput.AppendLog($"栅格点有{SourceGrid.Count},其中洼地点有{SourceGrid.Where(t => t.IsFill == false).Count()}");

            FormOutput.AppendLog("2.开始填充洼地");

            #region 指定填洼
            int times = 1;

            while (SourceGrid.Where(t => t.IsFill == false).Count() > 0)
            {
                double[,] tempSrc = new double[src.GetLongLength(0), src.GetLongLength(1)];
                foreach (var item in SourceGrid)
                {
                    if (item.IsFill)
                    {
                        tempSrc[item.Row, item.Col] = item.ALT;
                    }
                    else
                    {
                        tempSrc[item.Row, item.Col] = item.FilledALT;
                    }
                }

                MyGrid.Src = tempSrc;
                SourceGrid = MyGrid.GetSourceGrids();
                FormOutput.AppendLog($"第{times}次填充洼地,栅格点有{SourceGrid.Count},其中洼地点有{SourceGrid.Where(t => t.IsFill == false).Count()}");
                src = tempSrc;
                times++;
            }
            #endregion

            fillGrid = src;

            var ss = SourceGrid.OrderByDescending(t => t.ALT).ToList();
            if (!string.IsNullOrEmpty(FillPath))
            {
                DEMReader.SaveDem(read, fillGrid, null, FillPath);
            }

            FormOutput.AppendLog("填充洼地完成");
            //计算流向
            FormOutput.AppendLog("2.开始计算流向..");

            FormOutput.AppendLog($"未确定流向的点有{SourceGrid.Where(t => t.IsFlat).Count()}");

            directionDev = new int[src.GetLength(0), src.GetLength(1)];

            int index = 0;
            FormOutput.AppendProress(true);
            foreach (var item in SourceGrid)
            {
                directionDev[item.Row, item.Col] = item.Direction;
                index++;
                FormOutput.AppendProress(index * 100 / SourceGrid.Count);
            }

            if (!string.IsNullOrEmpty(DirectionPath))
            {
                DEMReader.SaveDem(read, directionDev, null, DirectionPath);
            }

            FormOutput.AppendLog("流向计算完成..");


            FormOutput.AppendLog("3.开始计算汇流量..");

            Accumulation = new int[src.GetLength(0), src.GetLength(1)];

            for (int i1 = 0; i1 < src.GetLength(0); i1++)
            {
                FormOutput.AppendProress(i1 * 100 / src.GetLength(0));
                for (int j1 = 0; j1 < src.GetLength(1); j1++)
                {
                    int i = i1;
                    int j = j1;
                    bool flag = true;
                    List<Point> caledPoint = new List<Point>();
                    while (flag)
                    {
                        //计算之后不计算，防止死循环
                        if (caledPoint.Where(t => t.X == i && t.Y == j).Any())
                        {
                            break;
                        }
                        int direction = directionDev[i, j];
                        caledPoint.Add(new Point(i, j));
                        switch (direction)
                        {
                            case 1:
                                Accumulation[i, j + 1]++;
                                j = j + 1;
                                break;
                            case 2:
                                Accumulation[i = 1, j + 1]++;
                                i = i + 1;
                                j = j + 1;
                                break;
                            case 4:
                                Accumulation[i + 1, j]++;
                                i = i + 1;
                                break;
                            case 8:
                                Accumulation[i + 1, j - 1]++;
                                i = i + 1;
                                j = j - 1;
                                break;
                            case 16:
                                Accumulation[i, j - 1]++;
                                j = j - 1;
                                break;
                            case 32:
                                Accumulation[i - 1, j - 1]++;
                                i = i - 1;
                                j = j - 1;
                                break;
                            case 64:
                                Accumulation[i - 1, j]++;
                                i = i - 1;
                                break;
                            case 128:
                                Accumulation[i - 1, j + 1]++;
                                i = i - 1;
                                j = j + 1;
                                break;
                            default:
                                break;
                        }
                        flag = direction != 0 && i < src.GetLength(0) && j < src.GetLength(1);
                    }
                }
            }

            if (!string.IsNullOrEmpty(AccumulationPath))
            {
                DEMReader.SaveDem(read, Accumulation, null, AccumulationPath);
            }

            FormOutput.AppendLog("计算汇流量完成..");

            FormOutput.AppendLog("4.开始根据回流阀值提取河网..");

            FormOutput.AppendLog($"当前阀值为{RiverThreshold}..");

            RiverGrid = new int[src.GetLength(0), src.GetLength(1)];
            for (int i = 0; i < Accumulation.GetLength(0); i++)
            {
                for (int j = 0; j < Accumulation.GetLength(1); j++)
                {
                    if (Accumulation[i, j] < RiverThreshold)
                    {
                        RiverGrid[i, j] = -1;
                    }

                    else
                    {
                        RiverGrid[i, j] = 1;
                    }
                }
            }

            DEMReader.SaveDem(read, RiverGrid, null, RiverPath);

            FormOutput.AppendLog("河网提取完成..");

            e.Result = RiverPath;
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                FormOutput.AppendLog(string.Format("提取结束,共耗时{0}秒..", (DateTime.Now - _currentTime).TotalSeconds));
                System.Diagnostics.Process.Start("Explorer.exe", Path.GetDirectoryName(e.Result.ToString()));
            }
            FormOutput.AppendProress(false);
            currentDem = fileChooseControl1.FilePath;
        }

        #endregion
    }
}
