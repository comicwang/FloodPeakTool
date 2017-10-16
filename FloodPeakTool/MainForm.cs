using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using iTelluro.GlobeDB;
using iTelluro.GlobeEngine.Graphics3D.Engine;
using iTelluro.GlobeEngine.Utility;
using iTelluro.GlobeEngine.Geometry;
using iTelluro.GlobeEngine;
using iTelluro.GlobeEngine.MapControl3D;
using iTelluro.GlobeEngine.MapControl3D.UI;
using iTelluro.Explorer.BarLib;
using iTelluro.Explorer.Splash;
using iTelluro.Explorer.DOMImport;
using iTelluro.Explorer.Raster;
using iTelluro.Explorer.VectorLoader;
using System.Xml;
using System.ComponentModel.Composition;
using FloodPeakUtility;
using System.ComponentModel.Composition.Hosting;
using FloodPeakUtility.Model;
using FloodPeakUtility.UI;

namespace FloodPeakTool
{
    public partial class MainForm : Form
    {
        public GlobeControl _globeControl= null;
        public GlobeView _globeView;

        public GlobeView GlobeView
        {
            get { return _globeView; }
            set { _globeView = value; }
        }
      
        private GlobeLayerControl _globeLayerUI;
        private LabelEditControl _labelEditUI;
        private GlobeFlyRouteControl _globeRouteUI;
        private GlobeFlyRoutePlusControl _globeRoutePlusUI;
        string _FullPath= string.Empty;
        private PnlLeftControl _pnlLeft = null;
        private ICaculateMemo _IMemo;

        /// <summary>
        /// 当前选中的模块
        /// </summary>
        public ICaculateMemo IMemo
        {
            get { return _IMemo; }
            private set
            {
                if (_IMemo != value && value != null)
                {
                    _IMemo = value;
                    if (_globeView != null && _pnlLeft != null)
                    {
                        _pnlLeft.UIParent.Controls.Clear();
                        _IMemo.LoadPlugin(_globeView, _pnlLeft);
                        _pnlLeft.UIParent.Text = _IMemo.CaculateName;
                    }
                }
                else if (value == null)
                {
                    MsgBox.ShowInfo("功能正在研发中...");
                }
            }
        }

        /// <summary>
        /// 获取所有的模块组合
        /// </summary>
        [ImportMany(typeof(ICaculateMemo))]
        public List<ICaculateMemo> Memos { get; set; }

        public MainForm(string[] args)
        {
            Application.DoEvents();
            InitializeComponent();         
            CreatUIControls();
            CreateGlobe();
        }

        private void CreatUIControls()
        {
            // 常规UI
            _globeLayerUI = new GlobeLayerControl();
            _globeLayerUI.Dock = DockStyle.Fill;

            _labelEditUI = new LabelEditControl();
            _labelEditUI.Dock = DockStyle.Fill;

            _globeRouteUI = new GlobeFlyRouteControl();
            _globeRouteUI.Dock = DockStyle.Fill;

            _globeRoutePlusUI = new GlobeFlyRoutePlusControl();
            _globeRoutePlusUI.Dock = DockStyle.Fill;
        }

        public void CreateGlobe()
        {
            try
            {
                _globeControl = new GlobeControl();
                _globeControl.Dock = DockStyle.Fill;
                this.ShowPLG.Controls.Clear();
                this.ShowPLG.Controls.Add(_globeControl);
                _globeView = _globeControl.Globe;
                _globeView.GlobeViewSetting.ShowLogo = false;//临时添加
            }
            catch (System.Exception e)
            {
                MessageBox.Show(
                                "三维视图初始化失败.\n" + e.Message,
                                "iTelluro GlobeEngine",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                                );

                Close();
                Environment.Exit(0);
            }
            _globeLayerUI.Connect(_globeView);
            _labelEditUI.Connect(_globeView);
            _globeRouteUI.Connect(_globeView);
            _globeRoutePlusUI.Connect(_globeView);     
        }

        #region  鼠标移动窗体跟随移动代码
        //鼠标移动窗体跟随移动代码
        Point mouseOff;//鼠标移动位子变量
        bool leftFlag;//标签是否为左键

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) 
            {
                mouseOff = new Point(-e.X, -e.Y);//得到变量的值
                leftFlag = true;
            }
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                Point mouseSet = Control.MousePosition;
                mouseSet.Offset(mouseOff.X, mouseOff.Y);
                Location = mouseSet;
            }
        }

        private void MainForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (leftFlag) 
            {
                leftFlag = false;//释放鼠标后为false
            }
        }


        #endregion

        #region 操作文件菜单

        /// <summary>
        /// 询问是否保存
        /// </summary>
        /// <returns></returns>
        private bool AskSave()
        {
            if (_pnlLeft != null && _pnlLeft.IsChanged)
            {
                DialogResult result = MsgBox.ShowThreeAsk("当前项目内容未保存,是否保存？");
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    _pnlLeft.SaveProjectChanges(true);
                    //return true;
                }
                else if (result == System.Windows.Forms.DialogResult.No)
                {
                    _pnlLeft.SaveProjectChanges(false);
                    //return true;
                }
                else
                    return false;
            }
            return true;
        }

        private void 新建工程ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AskSave() == true)
            {
                NewProject NJ = new NewProject();
                if (DialogResult.OK == NJ.ShowDialog())
                {
                    _FullPath = NJ.ProjectPath;
                    if (_pnlLeft == null)
                    {
                        _pnlLeft = new PnlLeftControl(_globeView,this.tabControl1);
                        _pnlLeft.Dock = DockStyle.Fill;
                        _pnlLeft.OnChanged += _pnlLeft_OnChanged;
                        _pnlLeft.OnCaculate += _pnlLeft_OnCaculate;
                        Pnl_Main_Left.Controls.Add(_pnlLeft);
                    }
                    Pnl_Main_Left.Show();
                    _pnlLeft.InitialzeProject(_FullPath);
                    //设置标题
                    this.Text = string.Format("{0}-泥石流小流域洪峰流量计算", _pnlLeft.ProjectModel.ProjectName);
                    btnImportDom.Enabled = true;
                    btnImportShp.Enabled = true;
                }
            }
           
        }

        /// <summary>
        /// 显示计算界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="arg"></param>
        private void _pnlLeft_OnCaculate(object sender, CaculateEventArgs arg)
        {
            IMemo = Memos.Where(t => t.CaculateId == arg.CaculateId).FirstOrDefault();
        }

        /// <summary>
        /// 控制保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="arg"></param>
        private void _pnlLeft_OnChanged(object sender, ProjectChangedEventArgs arg)
        {
            btnSave.Enabled = arg.Chnaged;
        }

        private void 退出程序ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AskSave() == true)
            {
                this.Close();
                Application.Exit();
            }
        }

        private void 导入DOMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _pnlLeft.ctxImportDom_Click(null, null);
        }

        private void 导入SHPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _pnlLeft.ctxImportShp_Click(null, null);
        }
       
        private void 打开工程ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AskSave() == true)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "计算文件|*.hfll";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    _FullPath = dialog.FileName;
                    //初始化化树节点
                    if (_pnlLeft == null)
                    {
                        _pnlLeft = new PnlLeftControl(_globeView,this.tabControl1);
                        _pnlLeft.Dock = DockStyle.Fill;
                        _pnlLeft.OnChanged += _pnlLeft_OnChanged;
                        _pnlLeft.OnCaculate += _pnlLeft_OnCaculate;
                        Pnl_Main_Left.Controls.Add(_pnlLeft);
                    }
                    Pnl_Main_Left.Show();
                    _pnlLeft.InitialzeProject(_FullPath);
                    //设置标题
                    this.Text = string.Format("{0}-泥石流小流域洪峰流量计算", _pnlLeft.ProjectModel.ProjectName);
                    btnImportDom.Enabled = true;
                    btnImportShp.Enabled = true;
                }
            }
        }

        private void 保存工程ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_pnlLeft != null)
                _pnlLeft.SaveProjectChanges(true);

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (AskSave() == true)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// 捕获窗口退出事件
        /// 用于杀死所有后台计算进程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            RunExeHelper.KillAll();
        }

        #endregion

        private void MainForm_Load(object sender, EventArgs e)
        {
            AggregateCatalog catelog = new AggregateCatalog();
            catelog.Catalogs.Add(new DirectoryCatalog(Path.Combine(Application.StartupPath, "Plugins")));
            CompositionContainer container = new CompositionContainer(catelog);
            container.ComposeParts(this);

            if (Memos == null || Memos.Count == 0)
            {
                MsgBox.ShowError("系统缺少计算组件,请联系管理员或者尝试重新启动！");
                Application.Exit();
            }
        }

        private void 暴雨频率计算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            IMemo = Memos.Where(t => t.CaculateName == item.Text).FirstOrDefault();
        }

        private void iTelluro工具ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _globeControl.BarGlobeTools.Visible = !_globeControl.BarGlobeTools.Visible;
            btnTools.Image = null;
            if (_globeControl.BarGlobeTools.Visible)
                btnTools.Image = global::FloodPeakTool.Properties.Resources.duigou;
        }
    }
}
