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
using System.Threading;

namespace FloodPeakToolUI.UI
{
    /// <summary>
    /// 洪峰流量计算界面
    /// </summary>
    [Export(typeof(ICaculateMemo))]
    public partial class MainCaculateControl : UserControl, ICaculateMemo
    {

        private GlobeView _globeView = null;
        private PnlLeftControl _parent = null;
        //项目文件夹路径
        private string _projectForlder = string.Empty;
        private CaculateResultUI _resutUI = null;
        public MainCaculateControl()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtQm.Text) || string.IsNullOrWhiteSpace(txtp1.Text) || string.IsNullOrWhiteSpace(txteps1.Text) || string.IsNullOrWhiteSpace(txteps2.Text) || string.IsNullOrWhiteSpace(txttc.Text))
            {
                MsgBox.ShowInfo("请确定参数完整！");
                return;
            }
            _projectForlder = Path.GetDirectoryName(_parent.ProjectModel.ProjectPath);
            //保存基础参数
            XmlHelper.Serialize<DefaultArgModel>(new DefaultArgModel()
                {
                    Qm = txtQm.Text,
                    p1 = txtp1.Text,
                    esp1 = txteps1.Text,
                    esp2 = txteps2.Text,
                    tc = txttc.Text

                }, Path.Combine(_projectForlder, ConfigNames.DefaultArgModel));


            //根据文件夹来获取里面的参数文件
            string xmlPath = Path.Combine(_projectForlder, ConfigNames.RainStormSub);
            //暴雨衰减赋值
            BYSJResult bysj;
            if (File.Exists(xmlPath))
            {
                bysj = XmlHelper.Deserialize<BYSJResult>(xmlPath);
            }
            else
            {
                MsgBox.ShowInfo("请确定参数完整！");
                return;
            }
            //暴雨损失赋值
            xmlPath = Path.Combine(_projectForlder, ConfigNames.RainStormLoss);
            BYSSResult byss;
            if (File.Exists(xmlPath))
            {
                byss = XmlHelper.Deserialize<BYSSResult>(xmlPath);
            }
            else
            {
                MsgBox.ShowInfo("请确定参数完整！");
                return;
            }
            //河槽汇流赋值
            xmlPath = Path.Combine(_projectForlder, ConfigNames.RiverConfluence);
            HCHLResult hchl;
            if (File.Exists(xmlPath))
            {
                hchl = XmlHelper.Deserialize<HCHLResult>(xmlPath);
            }
            else
            {
                MsgBox.ShowInfo("请确定参数完整！");
                return;
            }
            //坡面汇流赋值
            xmlPath = Path.Combine(_projectForlder, ConfigNames.SlopeConfluence);
            PMHLResult pmhl;
            if (File.Exists(xmlPath))
            {
                pmhl = XmlHelper.Deserialize<PMHLResult>(xmlPath);
            }
            else
            {
                MsgBox.ShowInfo("请确定参数完整！");
                return;
            }
            if (!backgroundWorker1.IsBusy)
            {
                FormOutput.AppendLog("开始计算...");
                object[] args = new object[] {
                         bysj,
                         byss,
                         hchl,
                         pmhl,
                          txtQm.Text,
                          txtp1.Text,
                          txteps1.Text,
                          txteps2.Text,
                          txttc.Text
                };
                backgroundWorker1.RunWorkerAsync(args);
            }
            else
            {
                FormOutput.AppendLog("当前后台正在计算...");
            }
        }

        #region 计算洪峰流量


        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //bysj,byss,hchl,pmhl,Qm,p1,eps1,eps2,tc
            object[] args = e.Argument as object[];
            BYSJResult bysj=args[0] as BYSJResult;
            BYSSResult byss=args[1] as BYSSResult;
            HCHLResult hchl=args[2] as HCHLResult;
            PMHLResult pmhl=args[3] as PMHLResult;
            //获取计算需要的参数值
            //p1,Qm,eps1,sd,R,d,nd,r1,F,L1,L2,I1,I2,A1,A2,tc,eps2
            StringBuilder builder = new StringBuilder();
            StringBuilder logBuilder=new StringBuilder();
            logBuilder.AppendLine("*********洪峰流量计算参数**********");
            builder.Append(MethodName.FloodPeak);
            builder.Append(" ");
            builder.Append(args[5]);
            logBuilder.AppendLine("P1："+args[5]);
            builder.Append(" ");
            builder.Append(args[4]);
             logBuilder.AppendLine("Qm："+args[4]);
            builder.Append(" ");
            builder.Append(args[6]);
             logBuilder.AppendLine("eps1："+args[6]);
            builder.Append(" ");
            builder.Append(bysj.Sd);
             logBuilder.AppendLine("Sd："+bysj.Sd);
            builder.Append(" ");
            builder.Append(byss.R);
             logBuilder.AppendLine("R："+byss.R);
            builder.Append(" ");
            builder.Append(bysj.d);
             logBuilder.AppendLine("d："+bysj.d);
            builder.Append(" ");
            builder.Append(bysj.nd);
             logBuilder.AppendLine("nd："+bysj.nd);
            builder.Append(" ");
            builder.Append(byss.r1);
             logBuilder.AppendLine("r1："+byss.r1);
            builder.Append(" ");
            builder.Append(byss.F);
             logBuilder.AppendLine("F："+byss.F);
            builder.Append(" ");
            builder.Append(hchl.L1);
             logBuilder.AppendLine("L1："+hchl.L1);
            builder.Append(" ");
            builder.Append(pmhl.L2);
             logBuilder.AppendLine("L2："+pmhl.L2);
            builder.Append(" ");
            builder.Append(hchl.l1);
             logBuilder.AppendLine("I1："+hchl.l1);
            builder.Append(" ");
            builder.Append(pmhl.l2);
             logBuilder.AppendLine("I2："+pmhl.l2);
            builder.Append(" ");
            builder.Append(hchl.A1);
             logBuilder.AppendLine("A1："+hchl.A1);
            builder.Append(" ");
            builder.Append(pmhl.A2);
             logBuilder.AppendLine("A2："+pmhl.A2);
            builder.Append(" ");
            builder.Append(args[8]);
             logBuilder.AppendLine("tc："+args[8]);
            builder.Append(" ");
            builder.Append(args[7]);
             logBuilder.AppendLine("eps2："+args[7]);
            builder.Append(" ");
            builder.Append(Path.Combine(_projectForlder, ConfigNames.FloodPeak));
            FormOutput.AppendLog(logBuilder.ToString());
            FormOutput.AppendLog("***********************************");
            RunExeHelper.RunMethod(builder.ToString());
            e.Result = "1";
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result != null)
                RunExeHelper.FindFigureAndTodo(ShowResult);
        }

        private void ShowResult(IntPtr windowPtr)
        {
            if (_parent.InvokeRequired)
            {
                _parent.Invoke(new Action(() =>
                {
                    if (_resutUI == null)
                    {
                        _resutUI = new CaculateResultUI();
                        _resutUI.Dock = DockStyle.Fill;
                        _parent.ShowDock("洪峰流量结果与曲线图", _resutUI);
                    }
                    _resutUI.BindResult(Path.Combine(Path.GetDirectoryName(_parent.ProjectModel.ProjectPath), ConfigNames.FloodPeak), windowPtr);
                }));
            }
        }

        #endregion

        #region 实现组件接口

        /// <summary>
        /// 组件ID
        /// </summary>
        public string CaculateId
        {
            get { return Guids.QSHF; }
        }

        /// <summary>
        /// 组件名称
        /// </summary>
        public string CaculateName
        {
            get { return "清水洪峰流量计算"; }
        }

        /// <summary>
        /// 组件描述
        /// </summary>
        public string Discription
        {
            get { return "计算清水洪峰流量模块"; }
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
            //初始化基础参数
            DefaultArgModel model = XmlHelper.Deserialize<DefaultArgModel>(Path.Combine(Path.GetDirectoryName(Parent.ProjectModel.ProjectPath),
                ConfigNames.DefaultArgModel));
            if (model != null)
            {
                txteps1.Text = model.esp1;
                txteps2.Text = model.esp2;
                txtp1.Text = model.p1;
                txtQm.Text = model.Qm;
                txttc.Text = model.tc;
            }
            Parent.UIParent.Controls.Add(this);
        }

        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            string projectForld = Path.GetDirectoryName(_parent.ProjectModel.ProjectPath);
            FormCaculateArgView form = new FormCaculateArgView(projectForld);
            if (DialogResult.OK == form.ShowDialog())
            {
                MsgBox.ShowInfo("保存成功！");
            }
        }

    }
}
