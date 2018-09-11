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
using iTelluro.DataTools.Utility.DEM;
using OSGeo.OGR;
using iTelluro.DataTools.Utility.GIS;
using iTelluro.GlobeEngine.DataSource.Layer;
using iTelluro.GeometrySymbol.SymbolModel;
using iTelluro.Explorer.Vector;
using iTelluro.Explorer.VectorLoader.ShpSymbolModel;

namespace FloodPeakToolUI.UI
{
    /// <summary>
    /// 主沟道参数计算UI
    /// </summary>
    [Export(typeof(ICaculateMemo))]
    public partial class RiverConfluenceControl : UserControl, ICaculateMemo
    {
        #region 字段

        private GlobeView _globeView = null;
        private PnlLeftControl _parent = null;
        private string _xmlPath;
        private Dictionary<Point2d, Geometry> _mainRiver = null;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public RiverConfluenceControl()
        {
            InitializeComponent();
        }

        #endregion

        private void CopyAddNodeModel(List<NodeModel> lstModels, NodeModel model)
        {
            NodeModel temp = new NodeModel();
            temp.PNode = Guids.HCHL;
            temp.ShowCheck = false;
            temp.ImageIndex = model.ImageIndex;
            temp.NodeId = Guid.NewGuid().ToString();
            temp.NodeName = model.NodeName;
            temp.CanRemove = true;
            lstModels.Add(temp);
        }

        #region 事件

        /// <summary>
        /// 保存结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            //获取结果值
            HCHLResult result = new HCHLResult()
            {
                L1 = string.IsNullOrEmpty(textBox1.Text) ? 0 : Convert.ToDouble(textBox1.Text),
                l1 = string.IsNullOrEmpty(textBox2.Text) ? 0 : Convert.ToDouble(textBox2.Text),
                A1 = string.IsNullOrEmpty(textBox3.Text) ? 0 : Convert.ToDouble(textBox3.Text)
            };
            XmlHelper.Serialize<HCHLResult>(result, _xmlPath);
            MsgBox.ShowInfo("保存成功！");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!backgroundWorker1.IsBusy)
            {
                FormOutput.AppendLog("开始计算...");
                FormOutput.AppendProress(true);
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
                _parent.OporateCaculateNode(Guids.HCHL, result.ToArray());

                //获取计算参数
                string tifPath = fileChooseControl1.FilePath;//影像路径
                string mainShp = fileChooseControl3.FilePath; //主沟道shp
                string argShp = fileChooseControl2.FilePath;//流速系数
                //progressBar1.Visible = true;
                backgroundWorker1.RunWorkerAsync(new string[] { mainShp, tifPath, argShp });
            }
            else
            {
                FormOutput.AppendLog("当前后台正在计算...");
            }
        }

        private void fileChooseControl3_OnSelectIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = File.Exists(fileChooseControl1.FilePath) || File.Exists(fileChooseControl2.FilePath) || File.Exists(fileChooseControl3.FilePath);
        }

        /// <summary>
        /// 导出主沟道
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            if (_mainRiver == null)
            {
                MsgBox.ShowInfo("暂时未计算主沟道");
                return;
            }
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "矢量文件|*.shp";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                ShpReader shp = new ShpReader(fileChooseControl3.FilePath);
                CreateShp(dialog.FileName, _mainRiver, shp.SpatialRef);
                System.Diagnostics.Process.Start("Explorer.exe", Path.GetDirectoryName(dialog.FileName));
            }
        }

        private void RiverConfluenceControl_Load(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(picInfo, "河道流速系数A1取值表");
        }

        private void picInfo_Click(object sender, EventArgs e)
        {
            FormA1Table _a1Table = new FormA1Table();
            if (DialogResult.OK == _a1Table.ShowDialog())
            {
                textBox3.Text = _a1Table.SelectedValue;
            }
        }

        #endregion

        #region 计算沟道长度和纵比降,以及沟道流速系数


        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            string[] args = e.Argument as string[];
            double riverlength = 0;
            double j = 0;
            if (File.Exists(args[0]) && File.Exists(args[1]))
            {
                FormOutput.AppendLog("开始递归所有沟道分支...");
                List<Dictionary<Point2d, Geometry>> results = GetAllRivers(args[0]);

                FormOutput.AppendLog(string.Format("共得到{0}条支流...", results.Count));

                FormOutput.AppendLog("开始获取主沟道，并计算其长度...");
              
                riverlength = CaculateRiver(results, args[1], ref _mainRiver);

                riverlength = riverlength / 1000;

                FormOutput.AppendLog("主河道长度：" + riverlength.ToString("f3"));

                FormOutput.AppendLog("开始计算主沟道纵降比...");
                j = GetLonGradient(_mainRiver, args[1]);
                FormOutput.AppendLog("主河道纵降比：" + j.ToString("f3"));
            }
           
            double? r = null;
            if (File.Exists(args[2]) && File.Exists(args[1]))
            {
                FormOutput.AppendLog("开始计算沟道流域系数...");
                r = RasterCoefficientReader.ReadCoeficient(args[2], args[1]);
                if (r.HasValue)
                    FormOutput.AppendLog("沟道流速系数：" + r.Value.ToString("f3"));
            }

            e.Result = new double[] { riverlength, j, r.GetValueOrDefault(0) };
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            double[] result = e.Result as double[];
            if (Convert.ToDouble(result[0]) != 0)
                textBox1.Text = result[0].ToString();
            if (Convert.ToDouble(result[1]) != 0)
                textBox2.Text = result[1].ToString();
            if (Convert.ToDouble(result[2]) != 0)
                textBox3.Text = result[2].ToString();
            FormOutput.AppendProress(false);
            //输出主沟道到临时目录并且加载到三维球和节点
            string tempPath = Path.Combine(Path.GetTempPath(), "主沟道" + DateTime.Now.ToString("HH_mm") + ".shp");
            ShpReader shp = new ShpReader(fileChooseControl3.FilePath);
            CreateShp(tempPath, _mainRiver, shp.SpatialRef);
            _parent.AddShpLineLayerByPath(tempPath);
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
        /// 递归所有支流
        /// </summary>
        /// <param name="sourcePoint"></param>
        /// <param name="geoList"></param>
        /// <param name="dic"></param>
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

        private double GetLength(Point3d startPoint, Point3d endPoint)
        {
            double hLength = Length(startPoint, endPoint);
            double zLength = endPoint.Z - startPoint.Z;
            double sLength = zLength * zLength + hLength * hLength;
            return Math.Sqrt(sLength);
        }

        private double GetElevationByPointInDEM(Point2d sourcePoint, RasterReader raster)
        {
            try
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
            catch (Exception ex)
            {
                return 0;
            }

        }

        private double GetLonGradient(Dictionary<Point2d, Geometry> mainRiver, string tifPath)
        {

            RasterReader reader = new RasterReader(tifPath);
            List<Point2d> pointList = new List<Point2d>();
            List<Point3d> gradientPoints = new List<Point3d>();
            int percent=0;
            foreach (var item in mainRiver)
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
                percent++;
                FormOutput.AppendProress(50 + (percent * 50 / mainRiver.Count));
            }
            double length = 0;
            double z = 0;
            double minElevation = gradientPoints[0].Z;
            for (int i = 1; i < gradientPoints.Count; i++)
            {
                if (minElevation > gradientPoints[i].Z) minElevation = gradientPoints[i].Z;
                Point3d point1 = gradientPoints[i - 1];
                Point3d point2 = gradientPoints[i];
                length += Length(point1, point2);
                z += Length(point1, point2) * (point1.Z + point2.Z);
            }
            return z * 1000 / (length * length);
        }

        /// <summary>
        /// 获取河流的主点
        /// </summary>
        /// <param name="mainShp"></param>
        /// <param name="featureList"></param>
        /// <returns></returns>
        private List<Point2d> GetRiverPoints(string mainShp,out List<OSGeo.OGR.Geometry> featureList)
        {
            ShpReader shp = new ShpReader(mainShp);
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

        private List<Dictionary<Point2d, Geometry>> GetAllRivers(string mainShp)
        {
            //获取主沟道          
            ShpReader shp = new ShpReader(mainShp);
            List<OSGeo.OGR.Feature> maxFeature = new List<OSGeo.OGR.Feature>();
            List<Geometry> geoList = null;
            List<Point2d> pts1 = GetRiverPoints(mainShp, out geoList);
            List<Dictionary<Point2d, Geometry>> dic = new List<Dictionary<Point2d, Geometry>>();
            GetLineByPoint(pts1[0], geoList, ref dic);
            return dic;
        }

        private double CaculateRiver(List<Dictionary<Point2d, Geometry>> dic,string tifPath,ref Dictionary<Point2d, Geometry> mainRiver)
        {
            RasterReader reader = new RasterReader(tifPath);
            //Dictionary<Point2d, Geometry> maxGeoLength = null;
            double maxLength = 0;  //主沟道长度
            List<Point3d> gradientPoints = new List<Point3d>();
           // List<Point3d> gradientPoints1 = new List<Point3d>();
            int percent = 0;
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
                    mainRiver = geoLength;
                    //gradientPoints1 = gradientPoints;
                }
                percent++;
                FormOutput.AppendProress(percent * 50 / dic.Count);
            }
            reader.Dispose();
            return maxLength;         
        }

        private void CreateShp(string outpath,Dictionary<Point2d, Geometry> geoList,OSGeo.OSR.SpatialReference srt)
        {
            //注册ogr库
            string pszDriverName = "ESRI Shapefile";
            OSGeo.OGR.Ogr.RegisterAll();

            //调用对shape文件读写的Driver接口
            OSGeo.OGR.Driver poDriver = OSGeo.OGR.Ogr.GetDriverByName(pszDriverName);
            if (poDriver == null)
            {
                MessageBox.Show("驱动错误！");
            }
            //创建河网shp
            string shpPath = outpath;

            OSGeo.OGR.DataSource poDs;
            poDs = ShpHelp.GetShpDriver().CreateDataSource(shpPath, null);

            //创建图层
            OSGeo.OGR.Layer poLayer;
            //OSGeo.OSR.SpatialReference srt = new OSGeo.OSR.SpatialReference(Const.WGS84);
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

        public double ReadBand(RasterReader reader, int col, int row)
        {
            double[] d = null;
            reader.ReadBand(col, row, 1, 1, out d);
            return d[0];
        }

        /// <summary>
        /// 求两点距离
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        private double Length(Point3d from, Point3d to)
        {
            double l = Math.Sqrt((to.X - from.X) * (to.X - from.X) + (to.Y - from.Y) * (to.Y - from.Y));
            return l;
        }

        #endregion

        #region 实现组件接口

        /// <summary>
        /// 组件ID
        /// </summary>
        public string CaculateId
        {
            get { return Guids.HCHL; }
        }

        /// <summary>
        /// 组件名称
        /// </summary>
        public string CaculateName
        {
            get { return "沟道汇流参数计算"; }
        }

        /// <summary>
        /// 组件描述
        /// </summary>
        public string Discription
        {
            get { return "计算沟道汇流参数模块"; }
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
                //绑定数据源
                NodeModel[] nodes = Parent.ProjectModel.Nodes.Where(t => t.PNode == Guids.HCHL).ToArray();
                fileChooseControl1.BindSource(Parent, (nodes != null && nodes.Count() > 0) ? nodes[0].NodeName : string.Empty);
                fileChooseControl2.BindSource(Parent, (nodes != null && nodes.Count() > 1) ? nodes[1].NodeName : string.Empty);
                fileChooseControl3.BindSource(Parent, (nodes != null && nodes.Count() > 2) ? nodes[2].NodeName : string.Empty);

                //显示之前的结果
                _xmlPath = Path.Combine(Path.GetDirectoryName(Parent.ProjectModel.ProjectPath), ConfigNames.RiverConfluence);
                if (File.Exists(_xmlPath))
                {
                    HCHLResult result = XmlHelper.Deserialize<HCHLResult>(_xmlPath);
                    if (result != null)
                    {
                        textBox1.Text = result.L1 == 0 ? "" : result.L1.ToString();
                        textBox2.Text = result.l1 == 0 ? "" : result.l1.ToString();
                        textBox3.Text = result.A1 == 0 ? "" : result.A1.ToString();
                    }
                }
            }
            catch
            { }
            Parent.UIParent.Controls.Add(this);
        }

        #endregion
    }
}
