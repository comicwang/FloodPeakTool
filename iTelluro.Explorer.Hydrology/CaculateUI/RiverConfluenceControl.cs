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

namespace FloodPeakToolUI.UI
{
    /// <summary>
    /// 主河槽参数计算UI
    /// </summary>
    [Export(typeof(ICaculateMemo))]
    public partial class RiverConfluenceControl : UserControl, ICaculateMemo
    {
        #region 字段

        private GlobeView _globeView = null;
        private PnlLeftControl _parent = null;
        private string _xmlPath;

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
                string mainShp = fileChooseControl3.FilePath; //主河槽shp
                string argShp = fileChooseControl2.FilePath;//流速系数
                //progressBar1.Visible = true;
                backgroundWorker1.RunWorkerAsync(new string[] { mainShp, tifPath, argShp });
            }
            else
            {
                FormOutput.AppendLog("当前后台正在计算...");
            }
        }

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

        private void fileChooseControl3_OnSelectIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = File.Exists(fileChooseControl1.FilePath) && File.Exists(fileChooseControl2.FilePath) && File.Exists(fileChooseControl3.FilePath);
        }

        #endregion

        #region 计算河槽长度和纵比降,以及河槽流速系数


        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            string[] args = e.Argument as string[];
            FormOutput.AppendLog("开始计算主河道长度...");
            double riverlength = FlowLength(args[0]);
            FormOutput.AppendLog("主河道长度：" + riverlength.ToString("f3"));

            FormOutput.AppendLog("开始计算主河纵降比...");
            double j = CalRiverLonGradient(args[0], args[1]);
            FormOutput.AppendLog("主河道纵降比：" + j.ToString("f3"));

            FormOutput.AppendLog("开始计算河槽流域系数...");
            double? r = RasterCoefficientReader.ReadCoeficient(args[2], args[1]);
            if (r.HasValue)
                FormOutput.AppendLog("河槽流速系数：" + r.Value.ToString("f3"));

            e.Result = new double[] { riverlength, j, r.GetValueOrDefault(0) };
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            double[] result = e.Result as double[];
            textBox1.Text = result[0].ToString();
            textBox2.Text = result[1].ToString();
            textBox3.Text = result[2].ToString();
           // progressBar1.Visible = false;
           // progressBar1.Value = 0;

        }

        /// <summary>
        /// 计算河道纵比降；
        /// 落差：河道两段的河底高程差
        /// 总落差：河源与河口凉茶的河底高程差
        /// 河床坡降：单位河长的落差叫河道纵降比
        /// j = h1-h0/l
        /// 或者 J = (h0+h1)l1 + (h1+h2)l2 +...+ (hn-1 + hn)ln/L*L 
        /// </summary>
        private double CalRiverLonGradient(string shppath, string tifPath)
        {
            ShpReader shp = new ShpReader(shppath);
            RasterReader reader = new RasterReader(tifPath);
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
                    int c = DemHelp.GetColFromLongitude(lon, lat);
                    int r = DemHelp.GetRowFromLatitude(lon, lat);
                    double z = ReadBand(reader, c, r);
                    //float elevation = _globeView.GetElevation(lon, lat);
                    points.Add(new Point3d(lon, lat, z));
                }
            }
            shp.Dispose();
            //计算河道纵比降
            J = GetLonGradient(points);
            return J;
        }

        public double ReadBand(RasterReader reader, int col, int row)
        {
            double[] d = null;
            reader.ReadBand(col, row, 1, 1, out d);
            return d[0];
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
            shp.Dispose();
            return sumLength;
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
            get { return "河槽汇流参数计算"; }
        }

        /// <summary>
        /// 组件描述
        /// </summary>
        public string Discription
        {
            get { return "计算河槽汇流参数模块"; }
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

            //绑定数据源
            NodeModel[] nodes = Parent.ProjectModel.Nodes.Where(t => t.PNode == Guids.HCHL).ToArray();
            fileChooseControl1.BindSource(Parent, (nodes != null && nodes.Count() > 0) ? nodes[0].NodeName : string.Empty);
            fileChooseControl2.BindSource(Parent, (nodes != null && nodes.Count() > 0) ? nodes[1].NodeName : string.Empty);
            fileChooseControl3.BindSource(Parent, (nodes != null && nodes.Count() > 0) ? nodes[2].NodeName : string.Empty);

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
            Parent.UIParent.Controls.Add(this);
        }

        #endregion

    }
}
