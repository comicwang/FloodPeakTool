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
        public RainstormAttenuation()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!backgroundWorker1.IsBusy)
            {
                FormOutput.AppendLog("开始计算...");

                //progressBar1.Visible = true;
                string state = txtState.Text;
                string level = cmbLevel.Text;
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
            FormOutput.AppendLog(string.Format("开始从数据库中获取站号【{0}】频率为【{1}】的单点雨量值", args[0], args[1]));

            string commandText = "select * from rainfall_percent where MONITORNUM='{0}' and [PERCENT]={1}";
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //progressBar1.Visible = false;
            //progressBar1.Value = 0;

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

            _xmlPath = Path.Combine(Path.GetDirectoryName(Parent.ProjectModel.ProjectPath), ConfigNames.RainStormSub + ".xml");
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
