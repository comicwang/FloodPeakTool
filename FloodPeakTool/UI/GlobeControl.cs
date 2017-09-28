using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iTelluro.GlobeEngine.MapControl3D;
using iTelluro.GlobeEngine.MapControl3D.UI;
using DevComponents.DotNetBar;
using iTelluro.Explorer.PluginEngine;
using System.IO;
using System.Drawing.Printing;

namespace FloodPeakTool
{
    public partial class GlobeControl : UserControl
    {
        #region 私有属性区
        private SlopeAnalysisControl _slopeAnalysisUI;
        private TerrainAnalysisControl _terrainAnalysisUI;
        private AreaVolumeAnalysisControl _areaVolumeAnalysisUI;
        private TabPanel _slopeAnalysisTabPanel;
        private TabPanel _terrainAnalysisTabPanel;
        private TabPanel _areaVolumeAnalysisTabPanel;

        public delegate void FullScreenEventHandler(bool flag);
        public event FullScreenEventHandler FullScreen;

        protected virtual void OnFullScreen(bool flag)
        {
            FullScreenEventHandler handler = FullScreen;
            if (handler != null) handler(flag);
        }


        private GlobeView _globeView;
        #endregion

        #region 公共属性区
        public GlobeView Globe
        {
            get { return _globeView; }
        }

        public DevComponents.DotNetBar.TabControl GlobeBottomPanel
        {
            get { return tabControlGlobeBottom; }
        }

        public ExpandableSplitter GlobeBottomSplitter
        {
            get { return expandableSplitter1; }
        }

        public Bar BarGlobeTools
        {
            get { return barGlobeTools; }
        }

        public string FullScreenText
        {
            set
            {
                全屏ButtonItem.Text = value;
            }
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public GlobeControl()
        {
            InitializeComponent();
            CreateGlobe();
            CreateLayer();
            CreateUI();
        }
        #endregion

        /// <summary>
        /// 初始化Globe
        /// </summary>
        private void CreateGlobe()
        {
            try
            {
                _globeView = new GlobeView();
                _globeView.Dock = DockStyle.Fill;
                _globeView.GlobeViewSetting.ShowLogo = false;
                // 三维窗口事件
                _globeView.GlobeKeyPress += new EventHandler<KeyEventArgs>(OnGlobeKeyPress);
            }
            catch (System.Exception e)
            {
                MessageBox.Show("三维视图初始化失败.\n" + e.Message, "iTelluro GlobeEngine", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
            this.panelGlobe.Controls.Add(_globeView);
        }
        // 设置全屏相关
        void OnGlobeKeyPress(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                OnFullScreen(false);
            }
        }

        /// <summary>
        /// 初始化UI
        /// </summary>
        private void CreateUI()
        {
            _slopeAnalysisUI = new SlopeAnalysisControl();
            _slopeAnalysisUI.Dock = DockStyle.Fill;
            _slopeAnalysisUI.Connect(_globeView);

            _terrainAnalysisUI = new TerrainAnalysisControl();
            _terrainAnalysisUI.Dock = DockStyle.Fill;
            _terrainAnalysisUI.Connect(_globeView);

            _areaVolumeAnalysisUI = new AreaVolumeAnalysisControl();
            _areaVolumeAnalysisUI.Dock = DockStyle.Fill;
            _areaVolumeAnalysisUI.Connect(_globeView);

            _slopeAnalysisTabPanel = new TabPanel("地形信息", _slopeAnalysisUI, imageList1.Images[6]);
            _slopeAnalysisTabPanel.AddToTabControl(tabControlGlobeBottom, expandableSplitter1);
            _slopeAnalysisTabPanel.Hide();

            _terrainAnalysisTabPanel = new TabPanel("地形剖面", _terrainAnalysisUI, imageList1.Images[4]);
            _terrainAnalysisTabPanel.AddToTabControl(tabControlGlobeBottom, expandableSplitter1);
            _terrainAnalysisTabPanel.Hide();

            _areaVolumeAnalysisTabPanel = new TabPanel("面积体积分析", _areaVolumeAnalysisUI, imageList1.Images[5]);
            _areaVolumeAnalysisTabPanel.AddToTabControl(tabControlGlobeBottom, expandableSplitter1);
            _areaVolumeAnalysisTabPanel.Hide();
        }
        /// <summary>
        /// 初始化图层
        /// </summary>
        private void CreateLayer()
        {
        }

        private void barGlobeTools_ItemClick(object sender, EventArgs e)
        {
            ButtonItem item = sender as ButtonItem;
            if (item == null) return;

            switch (item.Name)
            {
                case "buttonZoomIn":
                    double alt = this._globeView.GlobeCamera.CameraAltitude;
                    double lon = this._globeView.GlobeCamera.CameraLongitude;
                    double lat = this._globeView.GlobeCamera.CameraLatitude;
                    this._globeView.GlobeCamera.GotoLatLonAltitude(lat, lon, alt * 0.8);
                    break;
                case "buttonZoomOut":
                    alt = this._globeView.GlobeCamera.CameraAltitude;
                    lon = this._globeView.GlobeCamera.CameraLongitude;
                    lat = this._globeView.GlobeCamera.CameraLatitude;
                    this._globeView.GlobeCamera.GotoLatLonAltitude(lat, lon, alt * 1.2);
                    break;
                case "buttonNorth":
                    this._globeView.GlobeCamera.Reset();
                    break;
                case "buttonZoomAll":
                    this._globeView.GlobeCamera.Reset();
                    this._globeView.GlobeCamera.Reset();
                    break;
                case "buttonTileDown":
                    this._globeView.GlobeCamera.CameraTilt += 5;
                    break;
                case "buttonTileUp":
                    this._globeView.GlobeCamera.CameraTilt -= 5;
                    break;
                case "buttonMeasure":
                    this._globeView.DoMeasure();
                    break;
                case "buttonMeasureMulti":
                    this._globeView.DoMeasureMuliline();
                    break;
                case "buttonTerrainInfo":
                    _slopeAnalysisTabPanel.Show();
                    break;
                case "buttonTerrainProfile":
                    _terrainAnalysisTabPanel.Show();
                    break;
                case "buttonVolumeAnalysis":
                    _areaVolumeAnalysisTabPanel.Show();
                    break;
                default:
                    break;
            }
        }

        private void tabControlGlobeBottom_TabItemClose(object sender, TabStripActionEventArgs e)
        {
            try
            {
                e.Cancel = true;

                DevComponents.DotNetBar.TabControl tabcontrol = sender as DevComponents.DotNetBar.TabControl;
                if (tabcontrol != null)
                {
                    tabcontrol.SelectedTab.Visible = false;

                    bool hasvisible = false;

                    for (int i = 0; i < tabcontrol.Tabs.Count; i++)
                    {
                        if (tabcontrol.Tabs[i].Visible)
                        {
                            hasvisible = true;
                            break;
                        }
                    }
                    expandableSplitter1.Expanded = hasvisible;
                }
            }
            catch (Exception ex)
            { 
            
            }
        }

        private void 控制面板ButtonItem_Click(object sender, EventArgs e)
        {
            _globeView.GlobeViewSetting.ShowNavigation = !_globeView.GlobeViewSetting.ShowNavigation;
            控制面板ButtonItem.Checked = _globeView.GlobeViewSetting.ShowNavigation;
        }

        private void 指北针ButtonItem_Click(object sender, EventArgs e)
        {
            _globeView.GlobeViewSetting.ShowCompass = !_globeView.GlobeViewSetting.ShowCompass;
            指北针ButtonItem.Checked = _globeView.GlobeViewSetting.ShowCompass;
        }

        private void 比例尺ButtonItem_Click(object sender, EventArgs e)
        {
            _globeView.GlobeViewSetting.ShowScalebar = !_globeView.GlobeViewSetting.ShowScalebar;
            比例尺ButtonItem.Checked = _globeView.GlobeViewSetting.ShowScalebar;
        }

        private void 鼠标指针ButtonItem_Click(object sender, EventArgs e)
        {
            _globeView.GlobeViewSetting.ShowCrossHair = !_globeView.GlobeViewSetting.ShowCrossHair;
            鼠标指针ButtonItem.Checked = _globeView.GlobeViewSetting.ShowCrossHair;
        }

        private void 大气层ButtonItem_Click(object sender, EventArgs e)
        {
            _globeView.GlobeViewSetting.ShowSky = !_globeView.GlobeViewSetting.ShowSky;
            大气层ButtonItem.Checked = _globeView.GlobeViewSetting.ShowSky;
        }

        private void 经纬度网格ButtonItem_Click(object sender, EventArgs e)
        {
            _globeView.GlobeViewSetting.ShowLatLonGrid = !_globeView.GlobeViewSetting.ShowLatLonGrid;
            经纬度网格ButtonItem.Checked = _globeView.GlobeViewSetting.ShowLatLonGrid;
        }

        private void 位置信息ButtonItem_Click(object sender, EventArgs e)
        {
            _globeView.GlobeViewSetting.ShowPositionInfo = !_globeView.GlobeViewSetting.ShowPositionInfo;
            位置信息ButtonItem.Checked = _globeView.GlobeViewSetting.ShowPositionInfo;
        }

        private void 度分秒ButtonItem_Click(object sender, EventArgs e)
        {
            this._globeView.GlobeViewSetting.MousePositionInDMS = true;
        }

        private void 度ButtonItem_Click(object sender, EventArgs e)
        {
            this._globeView.GlobeViewSetting.MousePositionInDMS = false;
        }

        private void 标注加边ButtonItem_Click(object sender, EventArgs e)
        {
            _globeView.GlobeViewSetting.OutlineLabelText = !_globeView.GlobeViewSetting.OutlineLabelText;
            标注加边ButtonItem.Checked = _globeView.GlobeViewSetting.OutlineLabelText;
        }

        private void 高程缩放0ButtonItem_Click(object sender, EventArgs e)
        {
            _globeView.GlobeViewSetting.VerticalExaggeration = 0;
        }

        private void 高程缩放1ButtonItem_Click(object sender, EventArgs e)
        {
            _globeView.GlobeViewSetting.VerticalExaggeration = 1;
        }

        private void 高程缩放2ButtonItem_Click(object sender, EventArgs e)
        {
            _globeView.GlobeViewSetting.VerticalExaggeration = 2;
        }

        private void 高程缩放3ButtonItem_Click(object sender, EventArgs e)
        {
            _globeView.GlobeViewSetting.VerticalExaggeration = 3;
        }

        private void 鼠标惯性ButtonItem_Click(object sender, EventArgs e)
        {
            _globeView.GlobeCamera.CameraHasMomentum = !_globeView.GlobeCamera.CameraHasMomentum;
            鼠标惯性ButtonItem.Checked = _globeView.GlobeCamera.CameraHasMomentum;
        }

        private void 全屏ButtonItem_Click(object sender, EventArgs e)
        {
            if (全屏ButtonItem.Text == @"全屏")
            {
                OnFullScreen(true);
                全屏ButtonItem.Text = @"退出全屏";
            }
            else
            {
                OnFullScreen(false);
                全屏ButtonItem.Text = @"全屏";
            }
        }

        private void 打印视图ButtonItem_Click(object sender, EventArgs e)
        {
            this._globeView.SaveScreenshot(Path.Combine(Path.GetTempPath(), "iTelluro.Bmp"));
            PrintDocument document = new PrintDocument();
            document.PrintPage += new PrintPageEventHandler(this.OnPrintPage);
            this.printDialog1.AllowSomePages = true;
            this.printDialog1.ShowHelp = true;
            this.printDialog1.Document = document;
            if (this.printDialog1.ShowDialog() == DialogResult.OK)
            {
                document.Print();
            }
        }

        private void 保存截图ButtonItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = null;
            if (this.saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _globeView.SaveScreenshot(saveFileDialog1.FileName);
                }
                catch
                {
                    _globeView.SaveScreenshot(saveFileDialog1.FileName + ".png");
                }

            }
        }

        private void OnPrintPage(object sender, PrintPageEventArgs e)
        {
            Bitmap image = new Bitmap(Path.Combine(Path.GetTempPath(), "iTelluro.Bmp"));
            if (image != null)
            {
                int x = e.MarginBounds.X;
                int y = e.MarginBounds.Y;
                int width = image.Width;
                int height = image.Height;
                if ((width / e.MarginBounds.Width) > (height / e.MarginBounds.Height))
                {
                    width = e.MarginBounds.Width;
                    height = image.Height * e.MarginBounds.Width / image.Width;
                }
                else
                {
                    height = e.MarginBounds.Height;
                    width = image.Width * e.MarginBounds.Height / image.Height;
                }
                Rectangle destrect = new Rectangle(x, y, width, height);
                e.Graphics.DrawImage(image, destrect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);
            }
        }

        private void 清除缓存ButtonItem_Click(object sender, EventArgs e)
        {
            _globeView.ClearCache();
        }

        private void expandableSplitter1_ExpandedChanging(object sender, ExpandedChangeEventArgs e)
        {
            if (!this.expandableSplitter1.Expanded)
            {
                bool toshow = false;
                for (int i = 0; i < tabControlGlobeBottom.Tabs.Count; i++)
                {
                    if (tabControlGlobeBottom.Tabs[i].Visible)
                    {
                        toshow = true;
                        break;
                    }
                }
                if (!toshow)
                    e.Cancel = true;
            }
        }
    }
}
