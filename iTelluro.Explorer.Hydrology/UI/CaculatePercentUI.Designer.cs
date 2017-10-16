namespace FloodPeakToolUI.UI
{
    partial class CaculatePercentUI
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.expandablePanel1 = new DevComponents.DotNetBar.ExpandablePanel();
            this.grpSS = new System.Windows.Forms.GroupBox();
            this.btnCaculate = new System.Windows.Forms.Button();
            this.nihedu = new System.Windows.Forms.Label();
            this.txtNihe = new System.Windows.Forms.TextBox();
            this.label60 = new System.Windows.Forms.Label();
            this.numCv = new System.Windows.Forms.NumericUpDown();
            this.numCs = new System.Windows.Forms.NumericUpDown();
            this.numX = new System.Windows.Forms.NumericUpDown();
            this.label61 = new System.Windows.Forms.Label();
            this.label62 = new System.Windows.Forms.Label();
            this.grpResult = new System.Windows.Forms.GroupBox();
            this.btnInsert = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.label59 = new System.Windows.Forms.Label();
            this.label58 = new System.Windows.Forms.Label();
            this.numkik = new System.Windows.Forms.NumericUpDown();
            this.numQm = new System.Windows.Forms.NumericUpDown();
            this.bgwCaculate = new System.ComponentModel.BackgroundWorker();
            this.panel1.SuspendLayout();
            this.expandablePanel1.SuspendLayout();
            this.grpSS.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numX)).BeginInit();
            this.grpResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numkik)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQm)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.expandablePanel1);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(703, 501);
            this.panel1.TabIndex = 1;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(701, 306);
            this.panel2.TabIndex = 2;
            // 
            // expandablePanel1
            // 
            this.expandablePanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.expandablePanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.expandablePanel1.Controls.Add(this.grpSS);
            this.expandablePanel1.Controls.Add(this.grpResult);
            this.expandablePanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.expandablePanel1.Location = new System.Drawing.Point(0, 306);
            this.expandablePanel1.Name = "expandablePanel1";
            this.expandablePanel1.Size = new System.Drawing.Size(701, 193);
            this.expandablePanel1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.expandablePanel1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.expandablePanel1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.expandablePanel1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.expandablePanel1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
            this.expandablePanel1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandablePanel1.Style.GradientAngle = 90;
            this.expandablePanel1.TabIndex = 0;
            this.expandablePanel1.TitleStyle.Alignment = System.Drawing.StringAlignment.Center;
            this.expandablePanel1.TitleStyle.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.expandablePanel1.TitleStyle.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.expandablePanel1.TitleStyle.Border = DevComponents.DotNetBar.eBorderType.RaisedInner;
            this.expandablePanel1.TitleStyle.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandablePanel1.TitleStyle.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.expandablePanel1.TitleStyle.GradientAngle = 90;
            this.expandablePanel1.TitleText = "水文曲线计算结果";
            // 
            // grpSS
            // 
            this.grpSS.BackColor = System.Drawing.Color.White;
            this.grpSS.Controls.Add(this.btnCaculate);
            this.grpSS.Controls.Add(this.nihedu);
            this.grpSS.Controls.Add(this.txtNihe);
            this.grpSS.Controls.Add(this.label60);
            this.grpSS.Controls.Add(this.numCv);
            this.grpSS.Controls.Add(this.numCs);
            this.grpSS.Controls.Add(this.numX);
            this.grpSS.Controls.Add(this.label61);
            this.grpSS.Controls.Add(this.label62);
            this.grpSS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpSS.Location = new System.Drawing.Point(0, 26);
            this.grpSS.Name = "grpSS";
            this.grpSS.Size = new System.Drawing.Size(701, 84);
            this.grpSS.TabIndex = 4;
            this.grpSS.TabStop = false;
            this.grpSS.Text = "适线调整计算";
            // 
            // btnCaculate
            // 
            this.btnCaculate.Location = new System.Drawing.Point(273, 53);
            this.btnCaculate.Name = "btnCaculate";
            this.btnCaculate.Size = new System.Drawing.Size(75, 23);
            this.btnCaculate.TabIndex = 61;
            this.btnCaculate.Text = "适线计算";
            this.btnCaculate.UseVisualStyleBackColor = true;
            this.btnCaculate.Click += new System.EventHandler(this.btnCaculate_Click);
            // 
            // nihedu
            // 
            this.nihedu.AutoSize = true;
            this.nihedu.Location = new System.Drawing.Point(84, 58);
            this.nihedu.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.nihedu.Name = "nihedu";
            this.nihedu.Size = new System.Drawing.Size(41, 12);
            this.nihedu.TabIndex = 60;
            this.nihedu.Text = "拟合度";
            // 
            // txtNihe
            // 
            this.txtNihe.Location = new System.Drawing.Point(133, 54);
            this.txtNihe.Margin = new System.Windows.Forms.Padding(2);
            this.txtNihe.Name = "txtNihe";
            this.txtNihe.Size = new System.Drawing.Size(92, 21);
            this.txtNihe.TabIndex = 59;
            // 
            // label60
            // 
            this.label60.AutoSize = true;
            this.label60.Location = new System.Drawing.Point(271, 22);
            this.label60.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(65, 12);
            this.label60.TabIndex = 58;
            this.label60.Text = "变差系数Cv";
            // 
            // numCv
            // 
            this.numCv.DecimalPlaces = 4;
            this.numCv.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numCv.Location = new System.Drawing.Point(342, 18);
            this.numCv.Margin = new System.Windows.Forms.Padding(2);
            this.numCv.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numCv.Name = "numCv";
            this.numCv.Size = new System.Drawing.Size(92, 21);
            this.numCv.TabIndex = 57;
            // 
            // numCs
            // 
            this.numCs.DecimalPlaces = 4;
            this.numCs.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numCs.Location = new System.Drawing.Point(562, 18);
            this.numCs.Margin = new System.Windows.Forms.Padding(2);
            this.numCs.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numCs.Name = "numCs";
            this.numCs.Size = new System.Drawing.Size(92, 21);
            this.numCs.TabIndex = 56;
            // 
            // numX
            // 
            this.numX.DecimalPlaces = 4;
            this.numX.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numX.Location = new System.Drawing.Point(133, 18);
            this.numX.Margin = new System.Windows.Forms.Padding(2);
            this.numX.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numX.Name = "numX";
            this.numX.Size = new System.Drawing.Size(92, 21);
            this.numX.TabIndex = 55;
            // 
            // label61
            // 
            this.label61.AutoSize = true;
            this.label61.Location = new System.Drawing.Point(492, 22);
            this.label61.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label61.Name = "label61";
            this.label61.Size = new System.Drawing.Size(65, 12);
            this.label61.TabIndex = 54;
            this.label61.Text = "偏态系数Cs";
            // 
            // label62
            // 
            this.label62.AutoSize = true;
            this.label62.Location = new System.Drawing.Point(38, 22);
            this.label62.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label62.Name = "label62";
            this.label62.Size = new System.Drawing.Size(89, 12);
            this.label62.TabIndex = 53;
            this.label62.Text = "统计样本平均值";
            // 
            // grpResult
            // 
            this.grpResult.BackColor = System.Drawing.Color.White;
            this.grpResult.Controls.Add(this.btnInsert);
            this.grpResult.Controls.Add(this.btnSearch);
            this.grpResult.Controls.Add(this.label59);
            this.grpResult.Controls.Add(this.label58);
            this.grpResult.Controls.Add(this.numkik);
            this.grpResult.Controls.Add(this.numQm);
            this.grpResult.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpResult.Location = new System.Drawing.Point(0, 110);
            this.grpResult.Name = "grpResult";
            this.grpResult.Size = new System.Drawing.Size(701, 83);
            this.grpResult.TabIndex = 63;
            this.grpResult.TabStop = false;
            this.grpResult.Text = "频率计算";
            // 
            // btnInsert
            // 
            this.btnInsert.Location = new System.Drawing.Point(359, 49);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(75, 23);
            this.btnInsert.TabIndex = 62;
            this.btnInsert.Text = "统一入库";
            this.btnInsert.UseVisualStyleBackColor = true;
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(245, 49);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 61;
            this.btnSearch.Text = "曲线反查";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // label59
            // 
            this.label59.AutoSize = true;
            this.label59.Location = new System.Drawing.Point(19, 55);
            this.label59.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(107, 12);
            this.label59.TabIndex = 55;
            this.label59.Text = "理论流量值(0.1mm)";
            // 
            // label58
            // 
            this.label58.AutoSize = true;
            this.label58.Location = new System.Drawing.Point(67, 20);
            this.label58.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label58.Name = "label58";
            this.label58.Size = new System.Drawing.Size(59, 12);
            this.label58.TabIndex = 54;
            this.label58.Text = "概率值(%)";
            // 
            // numkik
            // 
            this.numkik.DecimalPlaces = 2;
            this.numkik.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numkik.Location = new System.Drawing.Point(133, 16);
            this.numkik.Margin = new System.Windows.Forms.Padding(2);
            this.numkik.Name = "numkik";
            this.numkik.Size = new System.Drawing.Size(92, 21);
            this.numkik.TabIndex = 53;
            // 
            // numQm
            // 
            this.numQm.DecimalPlaces = 4;
            this.numQm.Increment = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            this.numQm.Location = new System.Drawing.Point(133, 51);
            this.numQm.Margin = new System.Windows.Forms.Padding(2);
            this.numQm.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numQm.Name = "numQm";
            this.numQm.Size = new System.Drawing.Size(92, 21);
            this.numQm.TabIndex = 52;
            // 
            // bgwCaculate
            // 
            this.bgwCaculate.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwCaculate_DoWork);
            this.bgwCaculate.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwCaculate_RunWorkerCompleted);
            // 
            // CaculatePercentUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panel1);
            this.Name = "CaculatePercentUI";
            this.Size = new System.Drawing.Size(709, 507);
            this.panel1.ResumeLayout(false);
            this.expandablePanel1.ResumeLayout(false);
            this.grpSS.ResumeLayout(false);
            this.grpSS.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numX)).EndInit();
            this.grpResult.ResumeLayout(false);
            this.grpResult.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numkik)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQm)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.ComponentModel.BackgroundWorker bgwCaculate;
        private DevComponents.DotNetBar.ExpandablePanel expandablePanel1;
        private System.Windows.Forms.GroupBox grpSS;
        private System.Windows.Forms.Button btnCaculate;
        private System.Windows.Forms.Label nihedu;
        private System.Windows.Forms.TextBox txtNihe;
        private System.Windows.Forms.Label label60;
        private System.Windows.Forms.NumericUpDown numCv;
        private System.Windows.Forms.NumericUpDown numCs;
        private System.Windows.Forms.NumericUpDown numX;
        private System.Windows.Forms.Label label61;
        private System.Windows.Forms.Label label62;
        private System.Windows.Forms.GroupBox grpResult;
        private System.Windows.Forms.Button btnInsert;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label label59;
        private System.Windows.Forms.Label label58;
        private System.Windows.Forms.NumericUpDown numkik;
        private System.Windows.Forms.NumericUpDown numQm;

    }
}
