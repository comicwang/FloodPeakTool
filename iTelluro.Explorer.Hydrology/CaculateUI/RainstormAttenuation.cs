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
                string state = txtState.Text;
                string level = cmbLevel.SelectedValue.ToString();
                backgroundWorker1.RunWorkerAsync(new string[] { state, level });
            }
            else
            {
                FormOutput.AppendLog("当前后台正在计算...");
            }
        }


        private void txtState_TextChanged(object sender, EventArgs e)
        {
            btnCaculate.Enabled = !string.IsNullOrEmpty(txtState.Text) && !string.IsNullOrWhiteSpace(txtState.Text);
        }

        #region 计算暴雨衰减


        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            string[] args = e.Argument as string[];
            //FormOutput.AppendLog(string.Format("开始从数据库中获取站号【{0}】频率为【{1}%】的单点雨量值", args[0], args[1]));

            string commandText = string.Format("select During,VALUE from rainfall_percent where MONITORNUM='{0}' and [PERCENT]={1} order by During", args[0], args[1]);
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetConnSting(), CommandType.Text, commandText);
            if (ds.Tables[0].Rows.Count == 0)
            {
                FormOutput.AppendLog("未查询到数据..");
            }
            e.Result = ds;
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                DataSet ds = e.Result as DataSet;
                if (_resutUI == null)
                {
                    _resutUI = new CaculateFirgureUI();
                    _resutUI.Dock = DockStyle.Fill;
                }
                _parent.ShowDock("暴雨衰减曲线", _resutUI);
                string _xmlPath = Path.Combine(Path.GetDirectoryName(_parent.ProjectModel.ProjectPath), ConfigNames.RainStormSub);
                _resutUI.BindResult(ds, _xmlPath);
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
            Parent.UIParent.Controls.Add(this);
        }

        #endregion

    }
}
