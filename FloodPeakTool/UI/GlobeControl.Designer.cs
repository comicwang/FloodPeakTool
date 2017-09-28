namespace FloodPeakTool
{
    partial class GlobeControl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GlobeControl));
            this.panelAll = new System.Windows.Forms.Panel();
            this.panelGlobe = new System.Windows.Forms.Panel();
            this.expandableSplitter1 = new DevComponents.DotNetBar.ExpandableSplitter();
            this.itemPanel1 = new DevComponents.DotNetBar.ItemPanel();
            this.tabControlGlobeBottom = new DevComponents.DotNetBar.TabControl();
            this.barGlobeTools = new DevComponents.DotNetBar.Bar();
            this.buttonZoomIn = new DevComponents.DotNetBar.ButtonItem();
            this.buttonZoomOut = new DevComponents.DotNetBar.ButtonItem();
            this.buttonNorth = new DevComponents.DotNetBar.ButtonItem();
            this.buttonZoomAll = new DevComponents.DotNetBar.ButtonItem();
            this.buttonTileUp = new DevComponents.DotNetBar.ButtonItem();
            this.buttonTileDown = new DevComponents.DotNetBar.ButtonItem();
            this.buttonMeasure = new DevComponents.DotNetBar.ButtonItem();
            this.buttonMeasureMulti = new DevComponents.DotNetBar.ButtonItem();
            this.buttonTerrainInfo = new DevComponents.DotNetBar.ButtonItem();
            this.buttonTerrainProfile = new DevComponents.DotNetBar.ButtonItem();
            this.buttonVolumeAnalysis = new DevComponents.DotNetBar.ButtonItem();
            this.btnItemTools = new DevComponents.DotNetBar.ButtonItem();
            this.打印视图ButtonItem = new DevComponents.DotNetBar.ButtonItem();
            this.保存截图ButtonItem = new DevComponents.DotNetBar.ButtonItem();
            this.清除缓存ButtonItem = new DevComponents.DotNetBar.ButtonItem();
            this.btnItemView = new DevComponents.DotNetBar.ButtonItem();
            this.控制面板ButtonItem = new DevComponents.DotNetBar.ButtonItem();
            this.指北针ButtonItem = new DevComponents.DotNetBar.ButtonItem();
            this.比例尺ButtonItem = new DevComponents.DotNetBar.ButtonItem();
            this.鼠标指针ButtonItem = new DevComponents.DotNetBar.ButtonItem();
            this.大气层ButtonItem = new DevComponents.DotNetBar.ButtonItem();
            this.经纬度网格ButtonItem = new DevComponents.DotNetBar.ButtonItem();
            this.位置信息ButtonItem = new DevComponents.DotNetBar.ButtonItem();
            this.位置信息格式ButtonItem = new DevComponents.DotNetBar.ButtonItem();
            this.度分秒ButtonItem = new DevComponents.DotNetBar.ButtonItem();
            this.度ButtonItem = new DevComponents.DotNetBar.ButtonItem();
            this.标注加边ButtonItem = new DevComponents.DotNetBar.ButtonItem();
            this.高程缩放ButtonItem = new DevComponents.DotNetBar.ButtonItem();
            this.高程缩放0ButtonItem = new DevComponents.DotNetBar.ButtonItem();
            this.高程缩放1ButtonItem = new DevComponents.DotNetBar.ButtonItem();
            this.高程缩放2ButtonItem = new DevComponents.DotNetBar.ButtonItem();
            this.高程缩放3ButtonItem = new DevComponents.DotNetBar.ButtonItem();
            this.鼠标惯性ButtonItem = new DevComponents.DotNetBar.ButtonItem();
            this.全屏ButtonItem = new DevComponents.DotNetBar.ButtonItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.panelAll.SuspendLayout();
            this.itemPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabControlGlobeBottom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barGlobeTools)).BeginInit();
            this.SuspendLayout();
            // 
            // panelAll
            // 
            this.panelAll.BackColor = System.Drawing.Color.White;
            this.panelAll.Controls.Add(this.panelGlobe);
            this.panelAll.Controls.Add(this.expandableSplitter1);
            this.panelAll.Controls.Add(this.itemPanel1);
            this.panelAll.Controls.Add(this.barGlobeTools);
            this.panelAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelAll.Location = new System.Drawing.Point(0, 0);
            this.panelAll.Name = "panelAll";
            this.panelAll.Size = new System.Drawing.Size(658, 685);
            this.panelAll.TabIndex = 2;
            // 
            // panelGlobe
            // 
            this.panelGlobe.BackColor = System.Drawing.Color.Black;
            this.panelGlobe.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelGlobe.Location = new System.Drawing.Point(0, 25);
            this.panelGlobe.Name = "panelGlobe";
            this.panelGlobe.Size = new System.Drawing.Size(658, 513);
            this.panelGlobe.TabIndex = 0;
            // 
            // expandableSplitter1
            // 
            this.expandableSplitter1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.expandableSplitter1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter1.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter1.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.expandableSplitter1.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.expandableSplitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.expandableSplitter1.ExpandableControl = this.itemPanel1;
            this.expandableSplitter1.ExpandFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter1.ExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter1.ExpandLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.expandableSplitter1.ExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandableSplitter1.GripDarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.expandableSplitter1.GripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandableSplitter1.GripLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.expandableSplitter1.GripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.expandableSplitter1.HotBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(151)))), ((int)(((byte)(61)))));
            this.expandableSplitter1.HotBackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(184)))), ((int)(((byte)(94)))));
            this.expandableSplitter1.HotBackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground2;
            this.expandableSplitter1.HotBackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground;
            this.expandableSplitter1.HotExpandFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter1.HotExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter1.HotExpandLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.expandableSplitter1.HotExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandableSplitter1.HotGripDarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter1.HotGripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter1.HotGripLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.expandableSplitter1.HotGripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.expandableSplitter1.Location = new System.Drawing.Point(0, 538);
            this.expandableSplitter1.Name = "expandableSplitter1";
            this.expandableSplitter1.Size = new System.Drawing.Size(658, 6);
            this.expandableSplitter1.Style = DevComponents.DotNetBar.eSplitterStyle.Office2007;
            this.expandableSplitter1.TabIndex = 8;
            this.expandableSplitter1.TabStop = false;
            this.expandableSplitter1.ExpandedChanging += new DevComponents.DotNetBar.ExpandChangeEventHandler(this.expandableSplitter1_ExpandedChanging);
            // 
            // itemPanel1
            // 
            // 
            // 
            // 
            this.itemPanel1.BackgroundStyle.BackColor = System.Drawing.Color.White;
            this.itemPanel1.BackgroundStyle.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.itemPanel1.BackgroundStyle.BorderBottomWidth = 1;
            this.itemPanel1.BackgroundStyle.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(157)))), ((int)(((byte)(185)))));
            this.itemPanel1.BackgroundStyle.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.itemPanel1.BackgroundStyle.BorderLeftWidth = 1;
            this.itemPanel1.BackgroundStyle.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.itemPanel1.BackgroundStyle.BorderRightWidth = 1;
            this.itemPanel1.BackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.itemPanel1.BackgroundStyle.BorderTopWidth = 1;
            this.itemPanel1.BackgroundStyle.Class = "";
            this.itemPanel1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.itemPanel1.BackgroundStyle.PaddingBottom = 1;
            this.itemPanel1.BackgroundStyle.PaddingLeft = 1;
            this.itemPanel1.BackgroundStyle.PaddingRight = 1;
            this.itemPanel1.BackgroundStyle.PaddingTop = 1;
            this.itemPanel1.ContainerControlProcessDialogKey = true;
            this.itemPanel1.Controls.Add(this.tabControlGlobeBottom);
            this.itemPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.itemPanel1.LayoutOrientation = DevComponents.DotNetBar.eOrientation.Vertical;
            this.itemPanel1.Location = new System.Drawing.Point(0, 544);
            this.itemPanel1.Name = "itemPanel1";
            this.itemPanel1.Size = new System.Drawing.Size(658, 141);
            this.itemPanel1.TabIndex = 7;
            this.itemPanel1.Text = "itemPanel1";
            // 
            // tabControlGlobeBottom
            // 
            this.tabControlGlobeBottom.AutoCloseTabs = true;
            this.tabControlGlobeBottom.BackColor = System.Drawing.Color.White;
            this.tabControlGlobeBottom.CanReorderTabs = true;
            this.tabControlGlobeBottom.CloseButtonOnTabsAlwaysDisplayed = false;
            this.tabControlGlobeBottom.CloseButtonOnTabsVisible = true;
            this.tabControlGlobeBottom.CloseButtonVisible = true;
            this.tabControlGlobeBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlGlobeBottom.Location = new System.Drawing.Point(0, 0);
            this.tabControlGlobeBottom.Name = "tabControlGlobeBottom";
            this.tabControlGlobeBottom.SelectedTabFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.tabControlGlobeBottom.SelectedTabIndex = 0;
            this.tabControlGlobeBottom.Size = new System.Drawing.Size(658, 141);
            this.tabControlGlobeBottom.Style = DevComponents.DotNetBar.eTabStripStyle.Office2007Document;
            this.tabControlGlobeBottom.TabIndex = 0;
            this.tabControlGlobeBottom.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox;
            this.tabControlGlobeBottom.Text = "tabControl1";
            this.tabControlGlobeBottom.TabItemClose += new DevComponents.DotNetBar.TabStrip.UserActionEventHandler(this.tabControlGlobeBottom_TabItemClose);
            // 
            // barGlobeTools
            // 
            this.barGlobeTools.AccessibleDescription = "三维工具条 (barGlobe)";
            this.barGlobeTools.AccessibleName = "三维工具条";
            this.barGlobeTools.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            this.barGlobeTools.Dock = System.Windows.Forms.DockStyle.Top;
            this.barGlobeTools.DockSide = DevComponents.DotNetBar.eDockSide.Document;
            this.barGlobeTools.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.barGlobeTools.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.buttonZoomIn,
            this.buttonZoomOut,
            this.buttonNorth,
            this.buttonZoomAll,
            this.buttonTileUp,
            this.buttonTileDown,
            this.buttonMeasure,
            this.buttonMeasureMulti,
            this.buttonTerrainInfo,
            this.buttonTerrainProfile,
            this.buttonVolumeAnalysis,
            this.btnItemTools,
            this.btnItemView});
            this.barGlobeTools.Location = new System.Drawing.Point(0, 0);
            this.barGlobeTools.Name = "barGlobeTools";
            this.barGlobeTools.RoundCorners = false;
            this.barGlobeTools.Size = new System.Drawing.Size(658, 25);
            this.barGlobeTools.Stretch = true;
            this.barGlobeTools.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.barGlobeTools.TabIndex = 0;
            this.barGlobeTools.TabStop = false;
            this.barGlobeTools.Text = "三维工具条";
            this.barGlobeTools.Visible = false;
            this.barGlobeTools.ItemClick += new System.EventHandler(this.barGlobeTools_ItemClick);
            // 
            // buttonZoomIn
            // 
            this.buttonZoomIn.Image = ((System.Drawing.Image)(resources.GetObject("buttonZoomIn.Image")));
            this.buttonZoomIn.Name = "buttonZoomIn";
            this.buttonZoomIn.Text = "放大";
            this.buttonZoomIn.Tooltip = "放大";
            // 
            // buttonZoomOut
            // 
            this.buttonZoomOut.Image = ((System.Drawing.Image)(resources.GetObject("buttonZoomOut.Image")));
            this.buttonZoomOut.Name = "buttonZoomOut";
            this.buttonZoomOut.Text = "缩小";
            this.buttonZoomOut.Tooltip = "缩小";
            // 
            // buttonNorth
            // 
            this.buttonNorth.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.buttonNorth.Image = ((System.Drawing.Image)(resources.GetObject("buttonNorth.Image")));
            this.buttonNorth.Name = "buttonNorth";
            this.buttonNorth.Text = "正北";
            this.buttonNorth.Tooltip = "正北";
            // 
            // buttonZoomAll
            // 
            this.buttonZoomAll.Image = ((System.Drawing.Image)(resources.GetObject("buttonZoomAll.Image")));
            this.buttonZoomAll.Name = "buttonZoomAll";
            this.buttonZoomAll.Text = "全球|复原";
            this.buttonZoomAll.Tooltip = "全球, 复原";
            // 
            // buttonTileUp
            // 
            this.buttonTileUp.Image = ((System.Drawing.Image)(resources.GetObject("buttonTileUp.Image")));
            this.buttonTileUp.Name = "buttonTileUp";
            this.buttonTileUp.Text = "摄像机向上";
            this.buttonTileUp.Tooltip = "摄像机向上[垂直]";
            // 
            // buttonTileDown
            // 
            this.buttonTileDown.Image = ((System.Drawing.Image)(resources.GetObject("buttonTileDown.Image")));
            this.buttonTileDown.Name = "buttonTileDown";
            this.buttonTileDown.Text = "摄像机放平";
            this.buttonTileDown.Tooltip = "摄像机放平";
            // 
            // buttonMeasure
            // 
            this.buttonMeasure.BeginGroup = true;
            this.buttonMeasure.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.buttonMeasure.Image = ((System.Drawing.Image)(resources.GetObject("buttonMeasure.Image")));
            this.buttonMeasure.Name = "buttonMeasure";
            this.buttonMeasure.Text = "测距";
            this.buttonMeasure.Tooltip = "直线距离";
            // 
            // buttonMeasureMulti
            // 
            this.buttonMeasureMulti.Image = ((System.Drawing.Image)(resources.GetObject("buttonMeasureMulti.Image")));
            this.buttonMeasureMulti.Name = "buttonMeasureMulti";
            this.buttonMeasureMulti.Text = "折线距离";
            this.buttonMeasureMulti.Tooltip = "折线距离";
            // 
            // buttonTerrainInfo
            // 
            this.buttonTerrainInfo.Image = ((System.Drawing.Image)(resources.GetObject("buttonTerrainInfo.Image")));
            this.buttonTerrainInfo.Name = "buttonTerrainInfo";
            this.buttonTerrainInfo.Text = "地形信息";
            this.buttonTerrainInfo.Tooltip = "地形信息";
            // 
            // buttonTerrainProfile
            // 
            this.buttonTerrainProfile.Image = ((System.Drawing.Image)(resources.GetObject("buttonTerrainProfile.Image")));
            this.buttonTerrainProfile.Name = "buttonTerrainProfile";
            this.buttonTerrainProfile.Text = "地形剖面";
            this.buttonTerrainProfile.Tooltip = "地形剖面分析";
            // 
            // buttonVolumeAnalysis
            // 
            this.buttonVolumeAnalysis.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.buttonVolumeAnalysis.Image = ((System.Drawing.Image)(resources.GetObject("buttonVolumeAnalysis.Image")));
            this.buttonVolumeAnalysis.Name = "buttonVolumeAnalysis";
            this.buttonVolumeAnalysis.Text = "体积";
            this.buttonVolumeAnalysis.Tooltip = "体积量算";
            // 
            // btnItemTools
            // 
            this.btnItemTools.Image = ((System.Drawing.Image)(resources.GetObject("btnItemTools.Image")));
            this.btnItemTools.ItemAlignment = DevComponents.DotNetBar.eItemAlignment.Far;
            this.btnItemTools.Name = "btnItemTools";
            this.btnItemTools.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.打印视图ButtonItem,
            this.保存截图ButtonItem,
            this.清除缓存ButtonItem});
            this.btnItemTools.Text = "工具";
            // 
            // 打印视图ButtonItem
            // 
            this.打印视图ButtonItem.Image = ((System.Drawing.Image)(resources.GetObject("打印视图ButtonItem.Image")));
            this.打印视图ButtonItem.Name = "打印视图ButtonItem";
            this.打印视图ButtonItem.Text = "打印视图";
            this.打印视图ButtonItem.Click += new System.EventHandler(this.打印视图ButtonItem_Click);
            // 
            // 保存截图ButtonItem
            // 
            this.保存截图ButtonItem.Image = ((System.Drawing.Image)(resources.GetObject("保存截图ButtonItem.Image")));
            this.保存截图ButtonItem.Name = "保存截图ButtonItem";
            this.保存截图ButtonItem.Text = "保存截图";
            this.保存截图ButtonItem.Click += new System.EventHandler(this.保存截图ButtonItem_Click);
            // 
            // 清除缓存ButtonItem
            // 
            this.清除缓存ButtonItem.BeginGroup = true;
            this.清除缓存ButtonItem.Name = "清除缓存ButtonItem";
            this.清除缓存ButtonItem.Text = "清除缓存";
            this.清除缓存ButtonItem.Click += new System.EventHandler(this.清除缓存ButtonItem_Click);
            // 
            // btnItemView
            // 
            this.btnItemView.Image = ((System.Drawing.Image)(resources.GetObject("btnItemView.Image")));
            this.btnItemView.ItemAlignment = DevComponents.DotNetBar.eItemAlignment.Far;
            this.btnItemView.Name = "btnItemView";
            this.btnItemView.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.控制面板ButtonItem,
            this.指北针ButtonItem,
            this.比例尺ButtonItem,
            this.鼠标指针ButtonItem,
            this.大气层ButtonItem,
            this.经纬度网格ButtonItem,
            this.位置信息ButtonItem,
            this.位置信息格式ButtonItem,
            this.标注加边ButtonItem,
            this.高程缩放ButtonItem,
            this.鼠标惯性ButtonItem,
            this.全屏ButtonItem});
            this.btnItemView.Text = "视图";
            // 
            // 控制面板ButtonItem
            // 
            this.控制面板ButtonItem.Checked = true;
            this.控制面板ButtonItem.Name = "控制面板ButtonItem";
            this.控制面板ButtonItem.Text = "控制面板";
            this.控制面板ButtonItem.Click += new System.EventHandler(this.控制面板ButtonItem_Click);
            // 
            // 指北针ButtonItem
            // 
            this.指北针ButtonItem.Name = "指北针ButtonItem";
            this.指北针ButtonItem.Text = "指北针";
            this.指北针ButtonItem.Click += new System.EventHandler(this.指北针ButtonItem_Click);
            // 
            // 比例尺ButtonItem
            // 
            this.比例尺ButtonItem.Checked = true;
            this.比例尺ButtonItem.Name = "比例尺ButtonItem";
            this.比例尺ButtonItem.Text = "比例尺";
            this.比例尺ButtonItem.Click += new System.EventHandler(this.比例尺ButtonItem_Click);
            // 
            // 鼠标指针ButtonItem
            // 
            this.鼠标指针ButtonItem.Checked = true;
            this.鼠标指针ButtonItem.Name = "鼠标指针ButtonItem";
            this.鼠标指针ButtonItem.Text = "鼠标指针";
            this.鼠标指针ButtonItem.Click += new System.EventHandler(this.鼠标指针ButtonItem_Click);
            // 
            // 大气层ButtonItem
            // 
            this.大气层ButtonItem.Checked = true;
            this.大气层ButtonItem.Name = "大气层ButtonItem";
            this.大气层ButtonItem.Text = "大气层";
            this.大气层ButtonItem.Click += new System.EventHandler(this.大气层ButtonItem_Click);
            // 
            // 经纬度网格ButtonItem
            // 
            this.经纬度网格ButtonItem.Name = "经纬度网格ButtonItem";
            this.经纬度网格ButtonItem.Text = "经纬度网格";
            this.经纬度网格ButtonItem.Click += new System.EventHandler(this.经纬度网格ButtonItem_Click);
            // 
            // 位置信息ButtonItem
            // 
            this.位置信息ButtonItem.BeginGroup = true;
            this.位置信息ButtonItem.Checked = true;
            this.位置信息ButtonItem.Name = "位置信息ButtonItem";
            this.位置信息ButtonItem.Text = "位置信息";
            this.位置信息ButtonItem.Click += new System.EventHandler(this.位置信息ButtonItem_Click);
            // 
            // 位置信息格式ButtonItem
            // 
            this.位置信息格式ButtonItem.Name = "位置信息格式ButtonItem";
            this.位置信息格式ButtonItem.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.度分秒ButtonItem,
            this.度ButtonItem});
            this.位置信息格式ButtonItem.Text = "位置信息格式";
            // 
            // 度分秒ButtonItem
            // 
            this.度分秒ButtonItem.Name = "度分秒ButtonItem";
            this.度分秒ButtonItem.Text = "度分秒";
            this.度分秒ButtonItem.Click += new System.EventHandler(this.度分秒ButtonItem_Click);
            // 
            // 度ButtonItem
            // 
            this.度ButtonItem.Name = "度ButtonItem";
            this.度ButtonItem.Text = "度";
            this.度ButtonItem.Click += new System.EventHandler(this.度ButtonItem_Click);
            // 
            // 标注加边ButtonItem
            // 
            this.标注加边ButtonItem.Name = "标注加边ButtonItem";
            this.标注加边ButtonItem.Text = "标注加边";
            this.标注加边ButtonItem.Click += new System.EventHandler(this.标注加边ButtonItem_Click);
            // 
            // 高程缩放ButtonItem
            // 
            this.高程缩放ButtonItem.BeginGroup = true;
            this.高程缩放ButtonItem.Name = "高程缩放ButtonItem";
            this.高程缩放ButtonItem.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.高程缩放0ButtonItem,
            this.高程缩放1ButtonItem,
            this.高程缩放2ButtonItem,
            this.高程缩放3ButtonItem});
            this.高程缩放ButtonItem.Text = "高程缩放";
            // 
            // 高程缩放0ButtonItem
            // 
            this.高程缩放0ButtonItem.Name = "高程缩放0ButtonItem";
            this.高程缩放0ButtonItem.Text = "0";
            this.高程缩放0ButtonItem.Click += new System.EventHandler(this.高程缩放0ButtonItem_Click);
            // 
            // 高程缩放1ButtonItem
            // 
            this.高程缩放1ButtonItem.Name = "高程缩放1ButtonItem";
            this.高程缩放1ButtonItem.Text = "1";
            this.高程缩放1ButtonItem.Click += new System.EventHandler(this.高程缩放1ButtonItem_Click);
            // 
            // 高程缩放2ButtonItem
            // 
            this.高程缩放2ButtonItem.Name = "高程缩放2ButtonItem";
            this.高程缩放2ButtonItem.Text = "2";
            this.高程缩放2ButtonItem.Click += new System.EventHandler(this.高程缩放2ButtonItem_Click);
            // 
            // 高程缩放3ButtonItem
            // 
            this.高程缩放3ButtonItem.Name = "高程缩放3ButtonItem";
            this.高程缩放3ButtonItem.Text = "3";
            this.高程缩放3ButtonItem.Click += new System.EventHandler(this.高程缩放3ButtonItem_Click);
            // 
            // 鼠标惯性ButtonItem
            // 
            this.鼠标惯性ButtonItem.Name = "鼠标惯性ButtonItem";
            this.鼠标惯性ButtonItem.Text = "鼠标惯性";
            this.鼠标惯性ButtonItem.Click += new System.EventHandler(this.鼠标惯性ButtonItem_Click);
            // 
            // 全屏ButtonItem
            // 
            this.全屏ButtonItem.BeginGroup = true;
            this.全屏ButtonItem.Image = ((System.Drawing.Image)(resources.GetObject("全屏ButtonItem.Image")));
            this.全屏ButtonItem.Name = "全屏ButtonItem";
            this.全屏ButtonItem.Text = "全屏";
            this.全屏ButtonItem.Click += new System.EventHandler(this.全屏ButtonItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "layers.png");
            this.imageList1.Images.SetKeyName(1, "arrow_branch.png");
            this.imageList1.Images.SetKeyName(2, "arrow_merge.png");
            this.imageList1.Images.SetKeyName(3, "map_edit.png");
            this.imageList1.Images.SetKeyName(4, "toolTerrainProfile.Image.png");
            this.imageList1.Images.SetKeyName(5, "toolVolumeAnalysis.Image.png");
            this.imageList1.Images.SetKeyName(6, "toolTerrainInfo.Image.png");
            // 
            // printDialog1
            // 
            this.printDialog1.UseEXDialog = true;
            // 
            // GlobeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelAll);
            this.Name = "GlobeControl";
            this.Size = new System.Drawing.Size(658, 685);
            this.panelAll.ResumeLayout(false);
            this.itemPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabControlGlobeBottom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barGlobeTools)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelAll;
        private System.Windows.Forms.Panel panelGlobe;
        private DevComponents.DotNetBar.ExpandableSplitter expandableSplitter1;
        private DevComponents.DotNetBar.ItemPanel itemPanel1;
        private DevComponents.DotNetBar.TabControl tabControlGlobeBottom;
        private DevComponents.DotNetBar.Bar barGlobeTools;
        private DevComponents.DotNetBar.ButtonItem buttonZoomIn;
        private DevComponents.DotNetBar.ButtonItem buttonZoomOut;
        private DevComponents.DotNetBar.ButtonItem buttonNorth;
        private DevComponents.DotNetBar.ButtonItem buttonZoomAll;
        private DevComponents.DotNetBar.ButtonItem buttonTileUp;
        private DevComponents.DotNetBar.ButtonItem buttonTileDown;
        private DevComponents.DotNetBar.ButtonItem buttonMeasure;
        private DevComponents.DotNetBar.ButtonItem buttonMeasureMulti;
        private DevComponents.DotNetBar.ButtonItem buttonTerrainInfo;
        private DevComponents.DotNetBar.ButtonItem buttonTerrainProfile;
        private DevComponents.DotNetBar.ButtonItem buttonVolumeAnalysis;
        private System.Windows.Forms.ImageList imageList1;
        private DevComponents.DotNetBar.ButtonItem btnItemView;
        private DevComponents.DotNetBar.ButtonItem 控制面板ButtonItem;
        private DevComponents.DotNetBar.ButtonItem 指北针ButtonItem;
        private DevComponents.DotNetBar.ButtonItem 比例尺ButtonItem;
        private DevComponents.DotNetBar.ButtonItem 鼠标指针ButtonItem;
        private DevComponents.DotNetBar.ButtonItem 大气层ButtonItem;
        private DevComponents.DotNetBar.ButtonItem 经纬度网格ButtonItem;
        private DevComponents.DotNetBar.ButtonItem 位置信息ButtonItem;
        private DevComponents.DotNetBar.ButtonItem 位置信息格式ButtonItem;
        private DevComponents.DotNetBar.ButtonItem 度分秒ButtonItem;
        private DevComponents.DotNetBar.ButtonItem 度ButtonItem;
        private DevComponents.DotNetBar.ButtonItem 标注加边ButtonItem;
        private DevComponents.DotNetBar.ButtonItem 高程缩放ButtonItem;
        private DevComponents.DotNetBar.ButtonItem 高程缩放0ButtonItem;
        private DevComponents.DotNetBar.ButtonItem 高程缩放1ButtonItem;
        private DevComponents.DotNetBar.ButtonItem 高程缩放2ButtonItem;
        private DevComponents.DotNetBar.ButtonItem 高程缩放3ButtonItem;
        private DevComponents.DotNetBar.ButtonItem 鼠标惯性ButtonItem;
        private DevComponents.DotNetBar.ButtonItem 全屏ButtonItem;
        private DevComponents.DotNetBar.ButtonItem btnItemTools;
        private DevComponents.DotNetBar.ButtonItem 打印视图ButtonItem;
        private DevComponents.DotNetBar.ButtonItem 保存截图ButtonItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.PrintDialog printDialog1;
        private DevComponents.DotNetBar.ButtonItem 清除缓存ButtonItem;
    }
}
