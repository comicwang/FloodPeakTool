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
using System.Collections;
using FuncforHFLL;

namespace FloodPeakToolUI.UI
{
    [Export(typeof(ICaculateMemo))]
    public partial class RainstormAttenuationControl : UserControl, ICaculateMemo
    {
        private GlobeView _globeView = null;
        private PnlLeftControl _parent = null;
        private string _xmlPath = string.Empty;
        private CaculatePercentUI _resutUI = null;
        public RainstormAttenuationControl()
        {
            InitializeComponent();
            InitializeCombox();
        }

        private void InitializeCombox()
        {
            List<SimpleModel> dataSource = new List<SimpleModel>()
            {
                new SimpleModel(){ Display="10分钟频率统计",Value="MAX_10_MIN"},
                new SimpleModel(){Display="30分钟频率统计",Value="MAX_30_MIN"},
                new SimpleModel(){Display="1小时频率统计" ,Value="[MAX_1_HOUR]"},
                new SimpleModel(){Display="3小时频率统计" ,Value="[MAX_3_HOUR]"},
                new SimpleModel(){Display="6小时频率统计" ,Value="[MAX_6_HOUR]"},
                new SimpleModel(){Display="12小时频率统计",Value="[MAX_12_HOUR]"},
                new SimpleModel(){Display="48小时频率统计",Value="[MAX_48_HOUR]"},
                new SimpleModel(){Display="72小时频率统计",Value="[MAX_72_HOUR]"},
                new SimpleModel(){Display="1天频率统计"   ,Value="MAX_1_DAY"},
                new SimpleModel(){Display="3天频率统计"   ,Value="MAX_3_DAY"},
                new SimpleModel(){Display="5天频率统计"   ,Value="MAX_5_DAY"},
                new SimpleModel(){Display="7天频率统计"   ,Value="MAX_7_DAY"},
                new SimpleModel(){Display="15天频率统计"  ,Value="MAX_15_DAY"},
                new SimpleModel(){Display="30天频率统计"  ,Value="MAX_30_DAY"}
            };

            cmbPercent.DataSource = dataSource;
            cmbPercent.DisplayMember = "Display";
            cmbPercent.ValueMember = "Value";
            cmbPercent.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!backgroundWorker1.IsBusy)
            {
                FormOutput.AppendLog("开始计算...");

                //progressBar1.Visible = true;
                string state = txtState.Text;
                string percent = cmbPercent.SelectedValue.ToString();
                backgroundWorker1.RunWorkerAsync(new string[] { state, percent });
            }
            else
            {
                FormOutput.AppendLog("当前后台正在计算...");
            }
        }

        #region 计算暴雨频率


        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            string[] args = e.Argument as string[];  //state-percent
            FormOutput.AppendLog(string.Format("读取数据库站点【{0}】时间段为【{1}】的年统计最大值...", args[0], args[1]));
            //读取数据库某个站点某个统计频率的年统计最大值
            string commandText = string.Format("select {0} from RAINFALL_YEAR_MAX where MONITORNUM='{1}' order by {0} desc", args[1], args[0]);
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetConnSting(), CommandType.Text, commandText);
            //整理数据
            List<PercentStaticsModel> lstStatics = new List<PercentStaticsModel>();
            string points = string.Empty;
            if (ds.Tables[0].Rows.Count==0)
            {
                FormOutput.AppendLog("统计数据不足，请重新选择统计条件！");
                return;
            }
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (ds.Tables[0].Rows[i][0] == DBNull.Value)
                    continue;
                PercentStaticsModel temp = new PercentStaticsModel()
                {
                    RowIndex = i + 1,
                    MaxValue = Convert.ToDecimal(ds.Tables[0].Rows[i][0]) * 10,
                    CArg = ((decimal)(i + 1)) / ds.Tables[0].Rows.Count
                };
                lstStatics.Add(temp);
                points += ds.Tables[0].Rows[i][0];
                if (i < ds.Tables[0].Rows.Count - 1)
                    points += ",";
            }
            FormOutput.AppendLog(string.Format("统计值的数量为{0}个..", ds.Tables[0].Rows.Count));
            FormOutput.AppendLog(string.Format("统计值为[{0}]", points));
            string filePath = Path.Combine(Application.StartupPath, "SStatic.xls");
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            //保存数据到xls
            XmlHelper.SaveDataToExcelFile<PercentStaticsModel>(lstStatics, filePath);

            FormOutput.AppendLog("开始计算水文频率曲线..");
            RunExeHelper.RunMethod(MethodName.SWCure);
            e.Result = "1";
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result!=null)
                RunExeHelper.FindFigureAndTodo(ShowResult);
        }

        private void ShowResult(IntPtr windowPtr)
        {
            CvCure cv = XmlHelper.Deserialize<CvCure>(Path.Combine(Application.StartupPath, ConfigNames.SvCure));
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("计算结果：");
            builder.AppendLine("统计样本平均值X = " + cv.X);
            builder.AppendLine("变差系数Cv = " + cv.Cv);
            builder.AppendLine("偏态系数Cs = " + cv.Cs);
            builder.AppendLine("拟合度N = " + cv.Nihe);
            FormOutput.AppendLog(builder.ToString());
            if (cv != null)
            {
                if (_parent.InvokeRequired)
                {
                    _parent.Invoke(new Action(() =>
                    {
                        if (_resutUI == null)
                        {
                            _resutUI = new CaculatePercentUI();
                            _resutUI.Dock = DockStyle.Fill;
                            _parent.ShowDock("水文频率计算与曲线配线", _resutUI);
                        }
                        cv.State = txtState.Text;
                        cv.Time = cmbPercent.SelectedValue.ToString();
                        _resutUI.BindResult(cv, windowPtr);
                    }));
                }

            }
        }

        #endregion

        #region 实现组件接口

        /// <summary>
        /// 组件ID
        /// </summary>
        public string CaculateId
        {
            get { return Guids.BYPL; }
        }

        /// <summary>
        /// 组件名称
        /// </summary>
        public string CaculateName
        {
            get { return "暴雨频率计算"; }
        }

        /// <summary>
        /// 组件描述
        /// </summary>
        public string Discription
        {
            get { return "计算暴雨频率模块"; }
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
            Parent.UIParent.Controls.Add(this);
        }

        #endregion

        private void txtState_TextChanged(object sender, EventArgs e)
        {
            btnCaculate.Enabled = !string.IsNullOrEmpty(txtState.Text) && !string.IsNullOrWhiteSpace(txtState.Text);
        }

    }
}
