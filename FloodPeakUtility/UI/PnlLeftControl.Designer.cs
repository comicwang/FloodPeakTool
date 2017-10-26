using DevComponents.AdvTree;
using FloodPeakUtility.Model;
using iTelluro.Explorer.Vector;
using iTelluro.Explorer.VectorLoader.ShpSymbolConfig;
using iTelluro.Explorer.VectorLoader.ShpSymbolModel;
namespace FloodPeakUtility.UI
{
    partial class PnlLeftControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PnlLeftControl));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.advTreeMain = new DevComponents.AdvTree.AdvTree();
            this.treeImages = new System.Windows.Forms.ImageList(this.components);
            this.nodeConnector1 = new DevComponents.AdvTree.NodeConnector();
            this.elementStyle1 = new DevComponents.DotNetBar.ElementStyle();
            this.grpCaculate = new System.Windows.Forms.GroupBox();
            this.ctxMain = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ctxImportDom = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxImportShp = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxDelete = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ctxDeleteNode = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxCaculate = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btnBeginCaculate = new System.Windows.Forms.ToolStripMenuItem();
            this.bgwTile = new System.ComponentModel.BackgroundWorker();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.advTreeMain)).BeginInit();
            this.ctxMain.SuspendLayout();
            this.ctxDelete.SuspendLayout();
            this.ctxCaculate.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.advTreeMain);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(222, 359);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "工程树";
            // 
            // advTreeMain
            // 
            this.advTreeMain.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            this.advTreeMain.AllowDrop = true;
            this.advTreeMain.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.advTreeMain.BackgroundStyle.Class = "TreeBorderKey";
            this.advTreeMain.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.advTreeMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.advTreeMain.ImageList = this.treeImages;
            this.advTreeMain.Location = new System.Drawing.Point(3, 17);
            this.advTreeMain.Name = "advTreeMain";
            this.advTreeMain.NodesConnector = this.nodeConnector1;
            this.advTreeMain.NodeStyle = this.elementStyle1;
            this.advTreeMain.PathSeparator = ";";
            this.advTreeMain.Size = new System.Drawing.Size(216, 339);
            this.advTreeMain.Styles.Add(this.elementStyle1);
            this.advTreeMain.TabIndex = 0;
            this.advTreeMain.Text = "advTree1";
            this.advTreeMain.AfterCheck += new DevComponents.AdvTree.AdvTreeCellEventHandler(this.advTreeMain_AfterCheck);
            this.advTreeMain.AfterNodeSelect += new DevComponents.AdvTree.AdvTreeNodeEventHandler(this.advTreeMain_AfterNodeSelect);
            this.advTreeMain.AfterNodeRemove += new DevComponents.AdvTree.TreeNodeCollectionEventHandler(this.advTreeMain_AfterNodeRemove);
            this.advTreeMain.AfterNodeInsert += new DevComponents.AdvTree.TreeNodeCollectionEventHandler(this.advTreeMain_AfterNodeInsert);
            this.advTreeMain.NodeDoubleClick += new DevComponents.AdvTree.TreeNodeMouseEventHandler(this.advTreeMain_NodeDoubleClick);
            // 
            // treeImages
            // 
            this.treeImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("treeImages.ImageStream")));
            this.treeImages.TransparentColor = System.Drawing.Color.Transparent;
            this.treeImages.Images.SetKeyName(0, "layer.png");
            this.treeImages.Images.SetKeyName(1, "layer_wms.png");
            this.treeImages.Images.SetKeyName(2, "layer_grid.png");
            this.treeImages.Images.SetKeyName(3, "document_layout.png");
            this.treeImages.Images.SetKeyName(4, "chart_curve.png");
            this.treeImages.Images.SetKeyName(5, "loading.gif");
            // 
            // nodeConnector1
            // 
            this.nodeConnector1.LineColor = System.Drawing.SystemColors.ControlText;
            // 
            // elementStyle1
            // 
            this.elementStyle1.Class = "";
            this.elementStyle1.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.elementStyle1.Name = "elementStyle1";
            this.elementStyle1.TextColor = System.Drawing.SystemColors.ControlText;
            // 
            // grpCaculate
            // 
            this.grpCaculate.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpCaculate.Location = new System.Drawing.Point(0, 359);
            this.grpCaculate.Name = "grpCaculate";
            this.grpCaculate.Size = new System.Drawing.Size(222, 250);
            this.grpCaculate.TabIndex = 1;
            this.grpCaculate.TabStop = false;
            this.grpCaculate.Text = "模块名称";
            // 
            // ctxMain
            // 
            this.ctxMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctxImportDom,
            this.ctxImportShp});
            this.ctxMain.Name = "ctxMain";
            this.ctxMain.Size = new System.Drawing.Size(147, 48);
            // 
            // ctxImportDom
            // 
            this.ctxImportDom.Name = "ctxImportDom";
            this.ctxImportDom.Size = new System.Drawing.Size(146, 22);
            this.ctxImportDom.Text = "导入Dom(&O)";
            this.ctxImportDom.Click += new System.EventHandler(this.ctxImportDom_Click);
            // 
            // ctxImportShp
            // 
            this.ctxImportShp.Name = "ctxImportShp";
            this.ctxImportShp.Size = new System.Drawing.Size(146, 22);
            this.ctxImportShp.Text = "导入Shp(&P)";
            this.ctxImportShp.Click += new System.EventHandler(this.ctxImportShp_Click);
            // 
            // ctxDelete
            // 
            this.ctxDelete.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctxDeleteNode});
            this.ctxDelete.Name = "ctxDelete";
            this.ctxDelete.Size = new System.Drawing.Size(118, 26);
            // 
            // ctxDeleteNode
            // 
            this.ctxDeleteNode.Name = "ctxDeleteNode";
            this.ctxDeleteNode.Size = new System.Drawing.Size(117, 22);
            this.ctxDeleteNode.Text = "删除(&D)";
            this.ctxDeleteNode.Click += new System.EventHandler(this.ctxDeleteNode_Click);
            // 
            // ctxCaculate
            // 
            this.ctxCaculate.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnBeginCaculate});
            this.ctxCaculate.Name = "ctxCaculate";
            this.ctxCaculate.Size = new System.Drawing.Size(141, 26);
            // 
            // btnBeginCaculate
            // 
            this.btnBeginCaculate.Name = "btnBeginCaculate";
            this.btnBeginCaculate.Size = new System.Drawing.Size(140, 22);
            this.btnBeginCaculate.Text = "开始计算(&C)";
            this.btnBeginCaculate.Click += new System.EventHandler(this.btnBeginCaculate_Click);
            // 
            // bgwTile
            // 
            this.bgwTile.WorkerReportsProgress = true;
            this.bgwTile.WorkerSupportsCancellation = true;
            this.bgwTile.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwTile_DoWork);
            this.bgwTile.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwTile_RunWorkerCompleted);
            // 
            // PnlLeftControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grpCaculate);
            this.Name = "PnlLeftControl";
            this.Size = new System.Drawing.Size(222, 609);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.advTreeMain)).EndInit();
            this.ctxMain.ResumeLayout(false);
            this.ctxDelete.ResumeLayout(false);
            this.ctxCaculate.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox grpCaculate;
        private DevComponents.AdvTree.AdvTree advTreeMain;
        private DevComponents.AdvTree.NodeConnector nodeConnector1;
        private DevComponents.DotNetBar.ElementStyle elementStyle1;
        private System.Windows.Forms.ContextMenuStrip ctxMain;
        private System.Windows.Forms.ToolStripMenuItem ctxImportDom;
        private System.Windows.Forms.ToolStripMenuItem ctxImportShp;
        private System.Windows.Forms.ContextMenuStrip ctxDelete;
        private System.Windows.Forms.ToolStripMenuItem ctxDeleteNode;
        private System.Windows.Forms.ContextMenuStrip ctxCaculate;
        private System.Windows.Forms.ToolStripMenuItem btnBeginCaculate;
        private System.Windows.Forms.ImageList treeImages;
        private System.ComponentModel.BackgroundWorker bgwTile;
    }
}
