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
    public partial class RainstormAttenuation : UserControl, ICaculateMemo
    {
        private GlobeView _globeView = null;
        private PnlLeftControl _parent = null;
        private string _xmlPath = string.Empty;
        private CaculateFirgureUI _resutUI=null;
        public RainstormAttenuation()
        {
            InitializeComponent();
            InitializeCombox();
        }

        private void InitializeCombox()
        {
            List<SimpleModel> dataSource = new List<SimpleModel>()
            {
                new SimpleModel(){ Display="100年一遇",Value="1"},
                new SimpleModel(){Display="50年一遇",Value="2"},
                new SimpleModel(){Display="20年一遇" ,Value="5"},
                new SimpleModel(){Display="10年一遇" ,Value="10"},
                new SimpleModel(){Display="5年一遇" ,Value="20"},
            };

            cmbLevel.DataSource = dataSource;
            cmbLevel.DisplayMember = "Display";
            cmbLevel.ValueMember = "Value";
            cmbLevel.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!backgroundWorker1.IsBusy)
            {
                FormOutput.AppendLog("开始计算...");
                string state = txtState.Text;
                string level = cmbLevel.SelectedValue.ToString();
                backgroundWorker1.RunWorkerAsync(new string[] { state, level });
            }
            else
            {
                FormOutput.AppendLog("当前后台正在计算...");
            }
        }

        #region 计算暴雨衰减


        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            string[] args = e.Argument as string[];
            FormOutput.AppendLog(string.Format("开始从数据库中获取站号【{0}】频率为【{1}%】的单点雨量值", args[0], args[1]));

            string commandText = string.Format("select During,VALUE from rainfall_percent where MONITORNUM='{0}' and [PERCENT]={1} order by During", args[0], args[1]);
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetConnSting(), CommandType.Text, commandText);
            if (ds.Tables[0].Rows.Count == 0)
            {
                FormOutput.AppendLog("统计数据不足，请重新选择统计条件！");
                return;
            }
            //将数据按照大于一小时和小于一小时来分类
            string minHour = string.Empty;
            string minHourValue = string.Empty;
            string maxHour = string.Empty;
            string maxHourValue = string.Empty;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                double during = Convert.ToDouble(ds.Tables[0].Rows[i][0]);
                double value = Convert.ToDouble(ds.Tables[0].Rows[i][1]);
                if (during <= 1)
                {
                    minHour += during;
                    minHourValue += value;
                    if (i < ds.Tables[0].Rows.Count - 1)
                    {
                        minHour += ",";
                        minHourValue += ",";
                    }
                }
                else
                {
                    maxHour += during;
                    maxHourValue += value;
                    if (i < ds.Tables[0].Rows.Count - 1)
                    {
                        maxHourValue += ",";
                        maxHour += ",";
                    }
                }
            }
            FormOutput.AppendLog(string.Format("计算参数：小于1小时时间段-【{0}】,值【{1}】,大于1小时时间段【{2}】,值【{3}】", minHour, minHourValue, maxHour, maxHourValue));
            StringBuilder builder = new StringBuilder();
            builder.Append(MethodName.RainStormSub);
            builder.Append(" ");
            builder.Append(minHour.Substring(0, minHour.Length - 1));
            builder.Append(" ");
            builder.Append(" ");
            builder.Append(minHourValue.Substring(0, minHourValue.Length - 1));
            builder.Append(" ");
            builder.Append(" ");
            builder.Append(maxHour.Substring(0, maxHour.Length - 1));
            builder.Append(" ");
            builder.Append(" ");
            builder.Append(maxHourValue.Substring(0, maxHourValue.Length - 1));
            builder.Append(" ");
            FormOutput.AppendLog("开始计算暴雨衰减参数...");
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
            SubCure cv = XmlHelper.Deserialize<SubCure>(Path.Combine(Application.StartupPath, ConfigNames.SubCure));
            FormOutput.AppendLog(string.Format("计算结果：暴雨雨力Sd【{0}】,衰减指数nd【{1}】,衰减时间参数d【{2}】,斜率n1【{3}】,斜率n2【{4}】", cv.Sd, cv.nd, cv.d, cv.n1,cv.n2));
            if (cv.n2 / cv.n1 > 1.5)
            {
                FormOutput.AppendLog(string.Format("n2与n1的比为{0},结果无效", cv.n2 / cv.n1));
                //return;
            }
            if (cv != null)
            {
                if (_parent.InvokeRequired)
                {
                    _parent.Invoke(new Action(() =>
                    {
                        if (_resutUI == null)
                        {
                            _resutUI = new CaculateFirgureUI();
                            _resutUI.Dock = DockStyle.Fill;
                            _parent.ShowDock("暴雨衰减曲线", _resutUI);
                        }
                        _resutUI.BindResult(windowPtr);
                        txtSd.Text = cv.Sd.ToString("f3");
                        txtnd.Text = cv.nd.ToString("f3");
                        txtd.Text = cv.d.ToString("f3");
                        txtn1.Text = cv.n1.ToString("f3");
                        txtn2.Text = cv.n2.ToString("f3");
                       
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
            get { return Guids.BYSJ; }
        }

        /// <summary>
        /// 组件名称
        /// </summary>
        public string CaculateName
        {
            get { return "暴雨衰减参数计算"; }
        }

        /// <summary>
        /// 组件描述
        /// </summary>
        public string Discription
        {
            get { return "计算暴雨衰减参数模块"; }
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
            //默认选中剧烈程度第一行
            cmbLevel.SelectedIndex = 0;
            //显示之前的结果

            _xmlPath = Path.Combine(Path.GetDirectoryName(Parent.ProjectModel.ProjectPath), ConfigNames.RainStormSub);
            if (File.Exists(_xmlPath))
            {
                BYSJResult result = XmlHelper.Deserialize<BYSJResult>(_xmlPath);
                if (result != null)
                {
                    txtSd.Text = result.Sd == 0 ? "" : result.Sd.ToString();
                    txtnd.Text = result.nd == 0 ? "" : result.nd.ToString();
                    txtd.Text = result.d == 0 ? "" : result.d.ToString();
                }
            }
            

            Parent.UIParent.Controls.Add(this);
        }

        #endregion

        private void txtState_TextChanged(object sender, EventArgs e)
        {
            btnCaculate.Enabled = !string.IsNullOrEmpty(txtState.Text) && !string.IsNullOrWhiteSpace(txtState.Text);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //保存获取结果值
            BYSJResult result = new BYSJResult()
            {
                Sd = string.IsNullOrEmpty(txtSd.Text) ? 0 : Convert.ToDouble(txtSd.Text),
                nd = string.IsNullOrEmpty(txtnd.Text) ? 0 : Convert.ToDouble(txtnd.Text),
                d = string.IsNullOrEmpty(txtd.Text) ? 0 : Convert.ToDouble(txtd.Text)
            };
            XmlHelper.Serialize<BYSJResult>(result, _xmlPath);
            MsgBox.ShowInfo("保存成功！");
        }

    }
}
