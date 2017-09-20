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
    public partial class MainCaculateControl : UserControl, ICaculateMemo
    {
        private GlobeView _globeView = null;
        private PnlLeftControl _parent = null;
        private string _xmlPath = string.Empty;
        public MainCaculateControl()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!backgroundWorker1.IsBusy)
            {
                FormOutput.AppendLog("开始计算...");
      
                //progressBar1.Visible = true;
                backgroundWorker1.RunWorkerAsync();
            }
            else
            {
                FormOutput.AppendLog("当前后台正在计算...");
            }
        }

        #region 计算暴雨衰减


        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //progressBar1.Visible = false;
            //progressBar1.Value = 0;
            CaculateResultUI resutUI = new CaculateResultUI();
            resutUI.Dock = DockStyle.Fill;
            _parent.ShowDock("洪峰流量统计结果", resutUI);
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
            Parent.UIParent.Controls.Add(this);
        }

        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            FormCaculateArgView form = new FormCaculateArgView();
            form.ShowDialog();
        }

    }
}
