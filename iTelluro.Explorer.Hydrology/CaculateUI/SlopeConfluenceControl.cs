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
using OSGeo.OGR;
using iTelluro.DataTools.Utility.GIS;

namespace FloodPeakToolUI.UI
{
    [Export(typeof(ICaculateMemo))]
    public partial class SlopeConfluenceControl : UserControl, ICaculateMemo
    {
        private GlobeView _globeView = null;
        private PnlLeftControl _parent = null;
        private string _xmlPath = string.Empty;

        public SlopeConfluenceControl()
        {
            InitializeComponent();
        }

        private void CopyAddNodeModel(List<NodeModel> lstModels, NodeModel model)
        {
            NodeModel temp = new NodeModel();
            temp.PNode = Guids.PMHL;
            temp.ShowCheck = false;
            temp.ImageIndex = model.ImageIndex;
            temp.NodeId = Guid.NewGuid().ToString();
            temp.NodeName = model.NodeName;
            temp.CanRemove = true;
            lstModels.Add(temp);
        }

        #region Events

        /// <summary>
        /// 开始计算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

                _parent.OporateCaculateNode(Guids.PMHL, result.ToArray());


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

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            //获取结果值
            PMHLResult result = new PMHLResult()
            {
                L2 = string.IsNullOrEmpty(textBox1.Text) ? 0 : Convert.ToDouble(textBox1.Text),
                l2 = string.IsNullOrEmpty(textBox2.Text) ? 0 : Convert.ToDouble(textBox2.Text),
                A2 = string.IsNullOrEmpty(textBox3.Text) ? 0 : Convert.ToDouble(textBox3.Text)
            };
            //保存结果
            XmlHelper.Serialize<PMHLResult>(result, _xmlPath);
            MsgBox.ShowInfo("保存成功！");
        }

        private void fileChooseControl3_OnSelectIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = File.Exists(fileChooseControl1.FilePath) || File.Exists(fileChooseControl2.FilePath) || File.Exists(fileChooseControl3.FilePath);
        }

        #endregion

        #region 计算流域平均坡长和坡度,以及坡面流速系数


        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            string[] args = e.Argument as string[];

            double avgLength = 0;
            double avgSlope = 0;
            if (File.Exists(args[0]) && File.Exists(args[1]))
            {
                FormOutput.AppendLog("开始获取线的交点...");
                RasterReader reader = new RasterReader(args[1]);
                List<Point3d> pts = CalPoints(args[0],reader);
                FormOutput.AppendLog("开始计算平均坡度和坡长...");
                CalAvgSlopeLength(pts, reader, ref avgLength, ref avgSlope);
                reader.Dispose();
                FormOutput.AppendLog("平均坡长：" + avgLength.ToString("f3"));
                FormOutput.AppendLog("平均坡度：" + avgSlope.ToString("f3"));
            }
            double? r = null;
            if (File.Exists(args[1]) && File.Exists(args[2]))
            {
                FormOutput.AppendLog("开始计算坡面流速系数...");
                r = RasterCoefficientReader.ReadCoeficient(args[2], args[1]);
                if (r.HasValue)
                    FormOutput.AppendLog("结果--坡面流速系数为：" + r.Value.ToString());
            }
            e.Result = new double[] { avgLength, avgSlope, r.GetValueOrDefault(0) };
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
                        singleLength += RasterCoefficientReader.Length(point1, point2);
                    }
                    singleSlope = Math.Abs((startpt.Z - resultPt[resultPt.Count - 1].Z) / (RasterCoefficientReader.Length(startpt, resultPt[resultPt.Count - 1])));
                    totalSlope += singleSlope;
                    totalLength += singleLength;
                    count = points.Count * resultPt.Count;
                    FormOutput.AppendProress(((i + 1) * 100) / points.Count);
                }
                avglength = totalLength /(1000*count);
                avgslope = totalSlope * 1000 / points.Count;
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
                    double center = RasterCoefficientReader.ReadBand(reader, c, r);
                    //东方,列+1，行不变,p(i,j)-p(i,j+1)
                    if (c + 1 < col)
                    {
                        double e = center - RasterCoefficientReader.ReadBand(reader, c + 1, r);
                        dstDic.Add(e);
                    }
                    else
                    {
                        dstDic.Add(-1);
                    }

                    //东南,列+1，行+1,p(i,j)-p(i+1,j+1)
                    if (c + 1 < col && r + 1 < row)
                    {
                        double es = (center - RasterCoefficientReader.ReadBand(reader, c + 1, r + 1)) / Math.Sqrt(2);
                        dstDic.Add(es);
                    }
                    else
                    {
                        dstDic.Add(-1);
                    }

                    //南，列+0，行+1
                    if (r + 1 < row)
                    {
                        double s = center - RasterCoefficientReader.ReadBand(reader, c, r + 1);
                        dstDic.Add(s);
                    }
                    else
                    {
                        dstDic.Add(-1);
                    }

                    //西南
                    if (0 < c - 1 && 0 < r - 1)
                    {
                        double ws = (center - RasterCoefficientReader.ReadBand(reader, c - 1, r - 1)) / Math.Sqrt(2);
                        dstDic.Add(ws);
                    }
                    else
                    {
                        dstDic.Add(-1);
                    }

                    //西
                    if (0 < c - 1)
                    {
                        double w = center - RasterCoefficientReader.ReadBand(reader, c - 1, r);
                        dstDic.Add(w);
                    }
                    else
                    {
                        dstDic.Add(-1);
                    }

                    //西北
                    if (0 < c - 1 && 0 < r - 1)
                    {
                        double wn = (center - RasterCoefficientReader.ReadBand(reader, c - 1, r - 1)) / Math.Sqrt(2);
                        dstDic.Add(wn);
                    }
                    else
                    {
                        dstDic.Add(-1);
                    }

                    //北
                    if (0 < r - 1)
                    {
                        double n = center - RasterCoefficientReader.ReadBand(reader, c, r - 1);
                        dstDic.Add(n);
                    }
                    else
                    {
                        dstDic.Add(-1);
                    }

                    //东北
                    if (0 < r - 1 && c + 1 < col)
                    {
                        double en = (center - RasterCoefficientReader.ReadBand(reader, c + 1, r - 1)) / Math.Sqrt(2);
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
                    double pz = RasterCoefficientReader.ReadBand(reader, _maxR, _maxC);//_globeView.GetElevation(px, py);
                    Point3d maxPoint = new Point3d(px, py, pz);
                    outresult.Add(maxPoint);
                    Application.DoEvents();
                }/* end for c */
                Application.DoEvents();
            }/* enf for r */
            return outresult;
        }

        /// <summary>
        /// 计算线的交点
        /// 先获取每条线段的startpoint与endpoint，然后判断是否有重合
        /// </summary>
        private List<Point3d> CalPoints(string path,RasterReader reader)
        {
            ShpReader shp = new ShpReader(path);
            OSGeo.OGR.Feature ofea;

            List<Point3d> linePoint = new List<Point3d>();
            List<OSGeo.OGR.Geometry> featureList = new List<OSGeo.OGR.Geometry>();
            while (((ofea = shp.layer.GetNextFeature()) != null))
            {
                int count = ofea.GetGeometryRef().GetPointCount();
                Point3d start = new Point3d(ofea.GetGeometryRef().GetX(0), ofea.GetGeometryRef().GetY(0), GetElevationByPointInDEM(new Point2d(ofea.GetGeometryRef().GetX(0), ofea.GetGeometryRef().GetY(0)), reader));
                Point3d end = new Point3d(ofea.GetGeometryRef().GetX(count - 1), ofea.GetGeometryRef().GetY(count - 1), GetElevationByPointInDEM(new Point2d(ofea.GetGeometryRef().GetX(0), ofea.GetGeometryRef().GetY(0)), reader));
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

        private double ReadBand(RasterReader reader, int col, int row)
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
            get { return Guids.PMHL; }
        }

        /// <summary>
        /// 组件名称
        /// </summary>
        public string CaculateName
        {
            get { return "坡面汇流参数计算"; }
        }

        /// <summary>
        /// 组件描述
        /// </summary>
        public string Discription
        {
            get { return "计算坡面汇流参数模块"; }
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
                NodeModel[] nodes = Parent.ProjectModel.Nodes.Where(t => t.PNode == Guids.PMHL).ToArray();
                fileChooseControl1.BindSource(Parent, (nodes != null && nodes.Count() > 0) ? nodes[0].NodeName : string.Empty);
                fileChooseControl2.BindSource(Parent, (nodes != null && nodes.Count() > 0) ? nodes[1].NodeName : string.Empty);
                fileChooseControl3.BindSource(Parent, (nodes != null && nodes.Count() > 0) ? nodes[2].NodeName : string.Empty);

                //显示之前的结果
                _xmlPath = Path.Combine(Path.GetDirectoryName(Parent.ProjectModel.ProjectPath), ConfigNames.SlopeConfluence);
                if (File.Exists(_xmlPath))
                {
                    PMHLResult result = XmlHelper.Deserialize<PMHLResult>(_xmlPath);
                    if (result != null)
                    {
                        textBox1.Text = result.L2 == 0 ? "" : result.L2.ToString();
                        textBox2.Text = result.l2 == 0 ? "" : result.l2.ToString();
                        textBox3.Text = result.A2 == 0 ? "" : result.A2.ToString();
                    }
                }
            }
            catch
            {

            }
            Parent.UIParent.Controls.Add(this);
        }

        #endregion

    }
}
