namespace FloodPeakTool
{
    partial class MainForm
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.panelTop = new System.Windows.Forms.Panel();
            this.panelTopMain = new System.Windows.Forms.Panel();
            this.panelTitle = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panelMain = new System.Windows.Forms.Panel();
            this.PlnMainPlug = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.ShowPLG = new System.Windows.Forms.Panel();
            this.Pnl_Main_Left = new System.Windows.Forms.Panel();
            this.panelMenu = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.File = new System.Windows.Forms.ToolStripMenuItem();
            this.btnNew = new System.Windows.Forms.ToolStripMenuItem();
            this.btnOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSave = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnImportDom = new System.Windows.Forms.ToolStripMenuItem();
            this.btnImportShp = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnExport = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnExit = new System.Windows.Forms.ToolStripMenuItem();
            this.暴雨参数ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.暴雨频率计算ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.暴雨衰减参数计算ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.暴雨损失参数计算ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.汇流参数ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.河槽汇流参数计算ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.坡面汇流参数计算ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.洪峰流量ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.清水流量计算ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.泥石流洪峰流量计算ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.dotNetBarManager = new DevComponents.DotNetBar.DotNetBarManager(this.components);
            this.dockSite4 = new DevComponents.DotNetBar.DockSite();
            this.dockSite1 = new DevComponents.DotNetBar.DockSite();
            this.dockSite2 = new DevComponents.DotNetBar.DockSite();
            this.dockSite3 = new DevComponents.DotNetBar.DockSite();
            this.panelTop.SuspendLayout();
            this.panelTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panelMain.SuspendLayout();
            this.PlnMainPlug.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panelMenu.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.panelTop.Controls.Add(this.panelTopMain);
            this.panelTop.Controls.Add(this.panelTitle);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Padding = new System.Windows.Forms.Padding(2, 2, 2, 0);
            this.panelTop.Size = new System.Drawing.Size(1184, 60);
            this.panelTop.TabIndex = 7;
            // 
            // panelTopMain
            // 
            this.panelTopMain.BackColor = System.Drawing.Color.Transparent;
            this.panelTopMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTopMain.Location = new System.Drawing.Point(322, 2);
            this.panelTopMain.Name = "panelTopMain";
            this.panelTopMain.Size = new System.Drawing.Size(860, 58);
            this.panelTopMain.TabIndex = 1;
            // 
            // panelTitle
            // 
            this.panelTitle.Controls.Add(this.pictureBox1);
            this.panelTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelTitle.Location = new System.Drawing.Point(2, 2);
            this.panelTitle.Name = "panelTitle";
            this.panelTitle.Size = new System.Drawing.Size(320, 58);
            this.panelTitle.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(320, 58);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // panelMain
            // 
            this.panelMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.panelMain.Controls.Add(this.PlnMainPlug);
            this.panelMain.Controls.Add(this.Pnl_Main_Left);
            this.panelMain.Controls.Add(this.panelMenu);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 60);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(1184, 673);
            this.panelMain.TabIndex = 8;
            // 
            // PlnMainPlug
            // 
            this.PlnMainPlug.Controls.Add(this.tabControl1);
            this.PlnMainPlug.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PlnMainPlug.Location = new System.Drawing.Point(240, 42);
            this.PlnMainPlug.Name = "PlnMainPlug";
            this.PlnMainPlug.Size = new System.Drawing.Size(944, 631);
            this.PlnMainPlug.TabIndex = 2;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(944, 631);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.ShowPLG);
            this.tabPage1.ForeColor = System.Drawing.Color.Black;
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(936, 605);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "三维视图";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // ShowPLG
            // 
            this.ShowPLG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ShowPLG.Location = new System.Drawing.Point(3, 3);
            this.ShowPLG.Margin = new System.Windows.Forms.Padding(0);
            this.ShowPLG.Name = "ShowPLG";
            this.ShowPLG.Size = new System.Drawing.Size(930, 599);
            this.ShowPLG.TabIndex = 0;
            // 
            // Pnl_Main_Left
            // 
            this.Pnl_Main_Left.Dock = System.Windows.Forms.DockStyle.Left;
            this.Pnl_Main_Left.Location = new System.Drawing.Point(0, 42);
            this.Pnl_Main_Left.Name = "Pnl_Main_Left";
            this.Pnl_Main_Left.Size = new System.Drawing.Size(240, 631);
            this.Pnl_Main_Left.TabIndex = 1;
            this.Pnl_Main_Left.Visible = false;
            // 
            // panelMenu
            // 
            this.panelMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(225)))), ((int)(((byte)(252)))));
            this.panelMenu.Controls.Add(this.menuStrip1);
            this.panelMenu.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelMenu.Location = new System.Drawing.Point(0, 0);
            this.panelMenu.Name = "panelMenu";
            this.panelMenu.Size = new System.Drawing.Size(1184, 42);
            this.panelMenu.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(224)))), ((int)(((byte)(255)))));
            this.menuStrip1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("menuStrip1.BackgroundImage")));
            this.menuStrip1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.File,
            this.暴雨参数ToolStripMenuItem,
            this.汇流参数ToolStripMenuItem,
            this.洪峰流量ToolStripMenuItem,
            this.帮助ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1184, 42);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // File
            // 
            this.File.BackColor = System.Drawing.Color.Transparent;
            this.File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnNew,
            this.btnOpen,
            this.btnSave,
            this.toolStripSeparator1,
            this.btnImportDom,
            this.btnImportShp,
            this.toolStripSeparator2,
            this.btnExport,
            this.toolStripSeparator3,
            this.btnExit});
            this.File.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.File.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.File.Margin = new System.Windows.Forms.Padding(3);
            this.File.MergeIndex = 0;
            this.File.Name = "File";
            this.File.Size = new System.Drawing.Size(74, 32);
            this.File.Text = "文  件(&F)";
            this.File.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            // 
            // btnNew
            // 
            this.btnNew.BackColor = System.Drawing.Color.Transparent;
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(166, 24);
            this.btnNew.Text = "新建工程(&N)";
            this.btnNew.Click += new System.EventHandler(this.新建工程ToolStripMenuItem_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.BackColor = System.Drawing.Color.Transparent;
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(166, 24);
            this.btnOpen.Text = "打开工程(&O)";
            this.btnOpen.Click += new System.EventHandler(this.打开工程ToolStripMenuItem_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.Enabled = false;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(166, 24);
            this.btnSave.Text = "保存工程(&S)";
            this.btnSave.Click += new System.EventHandler(this.保存工程ToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(163, 6);
            // 
            // btnImportDom
            // 
            this.btnImportDom.BackColor = System.Drawing.Color.Transparent;
            this.btnImportDom.Enabled = false;
            this.btnImportDom.Name = "btnImportDom";
            this.btnImportDom.Size = new System.Drawing.Size(166, 24);
            this.btnImportDom.Text = "导入DOM(&M)";
            this.btnImportDom.Click += new System.EventHandler(this.导入DOMToolStripMenuItem_Click);
            // 
            // btnImportShp
            // 
            this.btnImportShp.BackColor = System.Drawing.Color.Transparent;
            this.btnImportShp.Enabled = false;
            this.btnImportShp.Name = "btnImportShp";
            this.btnImportShp.Size = new System.Drawing.Size(166, 24);
            this.btnImportShp.Text = "导入SHP(&H)";
            this.btnImportShp.Click += new System.EventHandler(this.导入SHPToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(163, 6);
            // 
            // btnExport
            // 
            this.btnExport.BackColor = System.Drawing.Color.Transparent;
            this.btnExport.Enabled = false;
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(166, 24);
            this.btnExport.Text = "导出(&P)";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(163, 6);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(166, 24);
            this.btnExit.Text = "退出程序(&E)";
            this.btnExit.Click += new System.EventHandler(this.退出程序ToolStripMenuItem_Click);
            // 
            // 暴雨参数ToolStripMenuItem
            // 
            this.暴雨参数ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.暴雨频率计算ToolStripMenuItem,
            this.暴雨衰减参数计算ToolStripMenuItem,
            this.暴雨损失参数计算ToolStripMenuItem});
            this.暴雨参数ToolStripMenuItem.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.暴雨参数ToolStripMenuItem.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.暴雨参数ToolStripMenuItem.Name = "暴雨参数ToolStripMenuItem";
            this.暴雨参数ToolStripMenuItem.Size = new System.Drawing.Size(77, 38);
            this.暴雨参数ToolStripMenuItem.Text = "暴雨参数";
            // 
            // 暴雨频率计算ToolStripMenuItem
            // 
            this.暴雨频率计算ToolStripMenuItem.Name = "暴雨频率计算ToolStripMenuItem";
            this.暴雨频率计算ToolStripMenuItem.Size = new System.Drawing.Size(190, 24);
            this.暴雨频率计算ToolStripMenuItem.Text = "暴雨频率计算";
            this.暴雨频率计算ToolStripMenuItem.Click += new System.EventHandler(this.暴雨频率计算ToolStripMenuItem_Click);
            // 
            // 暴雨衰减参数计算ToolStripMenuItem
            // 
            this.暴雨衰减参数计算ToolStripMenuItem.Name = "暴雨衰减参数计算ToolStripMenuItem";
            this.暴雨衰减参数计算ToolStripMenuItem.Size = new System.Drawing.Size(190, 24);
            this.暴雨衰减参数计算ToolStripMenuItem.Text = "暴雨衰减参数计算";
            this.暴雨衰减参数计算ToolStripMenuItem.Click += new System.EventHandler(this.暴雨频率计算ToolStripMenuItem_Click);
            // 
            // 暴雨损失参数计算ToolStripMenuItem
            // 
            this.暴雨损失参数计算ToolStripMenuItem.Name = "暴雨损失参数计算ToolStripMenuItem";
            this.暴雨损失参数计算ToolStripMenuItem.Size = new System.Drawing.Size(190, 24);
            this.暴雨损失参数计算ToolStripMenuItem.Text = "暴雨损失参数计算";
            this.暴雨损失参数计算ToolStripMenuItem.Click += new System.EventHandler(this.暴雨频率计算ToolStripMenuItem_Click);
            // 
            // 汇流参数ToolStripMenuItem
            // 
            this.汇流参数ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.河槽汇流参数计算ToolStripMenuItem,
            this.坡面汇流参数计算ToolStripMenuItem});
            this.汇流参数ToolStripMenuItem.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.汇流参数ToolStripMenuItem.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.汇流参数ToolStripMenuItem.Name = "汇流参数ToolStripMenuItem";
            this.汇流参数ToolStripMenuItem.Size = new System.Drawing.Size(77, 38);
            this.汇流参数ToolStripMenuItem.Text = "汇流参数";
            // 
            // 河槽汇流参数计算ToolStripMenuItem
            // 
            this.河槽汇流参数计算ToolStripMenuItem.Name = "河槽汇流参数计算ToolStripMenuItem";
            this.河槽汇流参数计算ToolStripMenuItem.Size = new System.Drawing.Size(190, 24);
            this.河槽汇流参数计算ToolStripMenuItem.Text = "河槽汇流参数计算";
            this.河槽汇流参数计算ToolStripMenuItem.Click += new System.EventHandler(this.暴雨频率计算ToolStripMenuItem_Click);
            // 
            // 坡面汇流参数计算ToolStripMenuItem
            // 
            this.坡面汇流参数计算ToolStripMenuItem.Name = "坡面汇流参数计算ToolStripMenuItem";
            this.坡面汇流参数计算ToolStripMenuItem.Size = new System.Drawing.Size(190, 24);
            this.坡面汇流参数计算ToolStripMenuItem.Text = "坡面汇流参数计算";
            this.坡面汇流参数计算ToolStripMenuItem.Click += new System.EventHandler(this.暴雨频率计算ToolStripMenuItem_Click);
            // 
            // 洪峰流量ToolStripMenuItem
            // 
            this.洪峰流量ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.清水流量计算ToolStripMenuItem,
            this.泥石流洪峰流量计算ToolStripMenuItem});
            this.洪峰流量ToolStripMenuItem.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.洪峰流量ToolStripMenuItem.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.洪峰流量ToolStripMenuItem.Name = "洪峰流量ToolStripMenuItem";
            this.洪峰流量ToolStripMenuItem.Size = new System.Drawing.Size(77, 38);
            this.洪峰流量ToolStripMenuItem.Text = "洪峰流量";
            // 
            // 清水流量计算ToolStripMenuItem
            // 
            this.清水流量计算ToolStripMenuItem.Name = "清水流量计算ToolStripMenuItem";
            this.清水流量计算ToolStripMenuItem.Size = new System.Drawing.Size(204, 24);
            this.清水流量计算ToolStripMenuItem.Text = "清水洪峰流量计算";
            this.清水流量计算ToolStripMenuItem.Click += new System.EventHandler(this.暴雨频率计算ToolStripMenuItem_Click);
            // 
            // 泥石流洪峰流量计算ToolStripMenuItem
            // 
            this.泥石流洪峰流量计算ToolStripMenuItem.Name = "泥石流洪峰流量计算ToolStripMenuItem";
            this.泥石流洪峰流量计算ToolStripMenuItem.Size = new System.Drawing.Size(204, 24);
            this.泥石流洪峰流量计算ToolStripMenuItem.Text = "泥石流洪峰流量计算";
            this.泥石流洪峰流量计算ToolStripMenuItem.Click += new System.EventHandler(this.暴雨频率计算ToolStripMenuItem_Click);
            // 
            // 帮助ToolStripMenuItem
            // 
            this.帮助ToolStripMenuItem.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.帮助ToolStripMenuItem.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.帮助ToolStripMenuItem.Name = "帮助ToolStripMenuItem";
            this.帮助ToolStripMenuItem.Size = new System.Drawing.Size(78, 38);
            this.帮助ToolStripMenuItem.Text = "帮  助(&H)";
            // 
            // dotNetBarManager
            // 
            this.dotNetBarManager.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.Ins);
            this.dotNetBarManager.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.Del);
            this.dotNetBarManager.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.F1);
            this.dotNetBarManager.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlA);
            this.dotNetBarManager.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlC);
            this.dotNetBarManager.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlV);
            this.dotNetBarManager.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlX);
            this.dotNetBarManager.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlY);
            this.dotNetBarManager.AutoDispatchShortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlZ);
            this.dotNetBarManager.BottomDockSite = null;
            this.dotNetBarManager.EnableFullSizeDock = false;
            this.dotNetBarManager.LeftDockSite = null;
            this.dotNetBarManager.ParentForm = this;
            this.dotNetBarManager.RightDockSite = null;
            this.dotNetBarManager.ShowCustomizeContextMenu = false;
            this.dotNetBarManager.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.dotNetBarManager.ToolbarBottomDockSite = this.dockSite4;
            this.dotNetBarManager.ToolbarLeftDockSite = this.dockSite1;
            this.dotNetBarManager.ToolbarRightDockSite = this.dockSite2;
            this.dotNetBarManager.ToolbarTopDockSite = this.dockSite3;
            this.dotNetBarManager.TopDockSite = null;
            // 
            // dockSite4
            // 
            this.dockSite4.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.dockSite4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dockSite4.Location = new System.Drawing.Point(0, 733);
            this.dockSite4.Name = "dockSite4";
            this.dockSite4.Size = new System.Drawing.Size(1184, 0);
            this.dockSite4.TabIndex = 12;
            this.dockSite4.TabStop = false;
            // 
            // dockSite1
            // 
            this.dockSite1.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.dockSite1.Dock = System.Windows.Forms.DockStyle.Left;
            this.dockSite1.Location = new System.Drawing.Point(0, 0);
            this.dockSite1.Name = "dockSite1";
            this.dockSite1.Size = new System.Drawing.Size(0, 733);
            this.dockSite1.TabIndex = 9;
            this.dockSite1.TabStop = false;
            // 
            // dockSite2
            // 
            this.dockSite2.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.dockSite2.Dock = System.Windows.Forms.DockStyle.Right;
            this.dockSite2.Location = new System.Drawing.Point(1184, 0);
            this.dockSite2.Name = "dockSite2";
            this.dockSite2.Size = new System.Drawing.Size(0, 733);
            this.dockSite2.TabIndex = 10;
            this.dockSite2.TabStop = false;
            // 
            // dockSite3
            // 
            this.dockSite3.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.dockSite3.Dock = System.Windows.Forms.DockStyle.Top;
            this.dockSite3.Location = new System.Drawing.Point(0, 0);
            this.dockSite3.Name = "dockSite3";
            this.dockSite3.Size = new System.Drawing.Size(1184, 0);
            this.dockSite3.TabIndex = 11;
            this.dockSite3.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1184, 733);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.dockSite1);
            this.Controls.Add(this.dockSite2);
            this.Controls.Add(this.dockSite3);
            this.Controls.Add(this.dockSite4);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds;
            this.Text = "泥石流小流域洪峰流量计算";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseUp);
            this.panelTop.ResumeLayout(false);
            this.panelTitle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panelMain.ResumeLayout(false);
            this.PlnMainPlug.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panelMenu.ResumeLayout(false);
            this.panelMenu.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Panel panelTitle;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panelTopMain;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panelMenu;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem File;
        private System.Windows.Forms.ToolStripMenuItem btnNew;
        private System.Windows.Forms.ToolStripMenuItem btnOpen;
        private System.Windows.Forms.ToolStripMenuItem btnSave;
        private System.Windows.Forms.ToolStripMenuItem btnImportDom;
        private System.Windows.Forms.ToolStripMenuItem btnImportShp;
        private System.Windows.Forms.ToolStripMenuItem btnExport;
        private System.Windows.Forms.ToolStripMenuItem btnExit;
        private System.Windows.Forms.ToolStripMenuItem 暴雨参数ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 汇流参数ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 洪峰流量ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 帮助ToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Panel Pnl_Main_Left;
        private System.Windows.Forms.Panel PlnMainPlug;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Panel ShowPLG;
        private DevComponents.DotNetBar.DotNetBarManager dotNetBarManager;
        private System.Windows.Forms.ToolStripMenuItem 暴雨频率计算ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 暴雨衰减参数计算ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 暴雨损失参数计算ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 河槽汇流参数计算ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 坡面汇流参数计算ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 清水流量计算ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 泥石流洪峰流量计算ToolStripMenuItem;
        private DevComponents.DotNetBar.DockSite dockSite1;
        private DevComponents.DotNetBar.DockSite dockSite2;
        private DevComponents.DotNetBar.DockSite dockSite3;
        private DevComponents.DotNetBar.DockSite dockSite4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    }
}

