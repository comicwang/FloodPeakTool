namespace iTelluro.Explorer.Hydrology
{
    partial class HydrologyControl
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
            this.linkLabel3 = new System.Windows.Forms.LinkLabel();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.txtLat = new System.Windows.Forms.TextBox();
            this.txtLon = new System.Windows.Forms.TextBox();
            this.linkLblClear = new System.Windows.Forms.LinkLabel();
            this.linkLblSlope = new System.Windows.Forms.LinkLabel();
            this.linkLblSlopeLength = new System.Windows.Forms.LinkLabel();
            this.linkLblWatershed = new System.Windows.Forms.LinkLabel();
            this.linkLblFlowLength = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.lallayerlist = new System.Windows.Forms.Label();
            this.linkLblLocation = new System.Windows.Forms.LinkLabel();
            this.linkLblLonGradient = new System.Windows.Forms.LinkLabel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.linkLabel4 = new System.Windows.Forms.LinkLabel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.linkLabel3);
            this.panel1.Controls.Add(this.linkLabel2);
            this.panel1.Controls.Add(this.linkLabel1);
            this.panel1.Controls.Add(this.txtLat);
            this.panel1.Controls.Add(this.txtLon);
            this.panel1.Controls.Add(this.linkLblClear);
            this.panel1.Controls.Add(this.linkLblSlope);
            this.panel1.Controls.Add(this.linkLblSlopeLength);
            this.panel1.Controls.Add(this.linkLblWatershed);
            this.panel1.Controls.Add(this.linkLabel4);
            this.panel1.Controls.Add(this.linkLblFlowLength);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.lallayerlist);
            this.panel1.Controls.Add(this.linkLblLocation);
            this.panel1.Controls.Add(this.linkLblLonGradient);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(246, 488);
            this.panel1.TabIndex = 0;
            // 
            // linkLabel3
            // 
            this.linkLabel3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkLabel3.AutoSize = true;
            this.linkLabel3.Location = new System.Drawing.Point(3, 460);
            this.linkLabel3.Name = "linkLabel3";
            this.linkLabel3.Size = new System.Drawing.Size(77, 12);
            this.linkLabel3.TabIndex = 27;
            this.linkLabel3.TabStop = true;
            this.linkLabel3.Text = "坡面流速系数";
            this.linkLabel3.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel3_LinkClicked);
            // 
            // linkLabel2
            // 
            this.linkLabel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Location = new System.Drawing.Point(86, 460);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(77, 12);
            this.linkLabel2.TabIndex = 26;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "河槽流速系数";
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(166, 130);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(77, 12);
            this.linkLabel1.TabIndex = 25;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "暴雨损失参数";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // txtLat
            // 
            this.txtLat.Location = new System.Drawing.Point(93, 94);
            this.txtLat.Name = "txtLat";
            this.txtLat.Size = new System.Drawing.Size(51, 21);
            this.txtLat.TabIndex = 24;
            this.txtLat.Text = "30";
            // 
            // txtLon
            // 
            this.txtLon.Location = new System.Drawing.Point(7, 94);
            this.txtLon.Name = "txtLon";
            this.txtLon.Size = new System.Drawing.Size(51, 21);
            this.txtLon.TabIndex = 23;
            this.txtLon.Text = "110";
            // 
            // linkLblClear
            // 
            this.linkLblClear.AutoSize = true;
            this.linkLblClear.Location = new System.Drawing.Point(7, 132);
            this.linkLblClear.Name = "linkLblClear";
            this.linkLblClear.Size = new System.Drawing.Size(29, 12);
            this.linkLblClear.TabIndex = 22;
            this.linkLblClear.TabStop = true;
            this.linkLblClear.Text = "清除";
            this.linkLblClear.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLblClear_LinkClicked);
            // 
            // linkLblSlope
            // 
            this.linkLblSlope.AutoSize = true;
            this.linkLblSlope.Location = new System.Drawing.Point(178, 97);
            this.linkLblSlope.Name = "linkLblSlope";
            this.linkLblSlope.Size = new System.Drawing.Size(53, 12);
            this.linkLblSlope.TabIndex = 17;
            this.linkLblSlope.TabStop = true;
            this.linkLblSlope.Text = "坡度计算";
            this.linkLblSlope.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLblSlope_LinkClicked);
            // 
            // linkLblSlopeLength
            // 
            this.linkLblSlopeLength.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkLblSlopeLength.AutoSize = true;
            this.linkLblSlopeLength.Location = new System.Drawing.Point(169, 460);
            this.linkLblSlopeLength.Name = "linkLblSlopeLength";
            this.linkLblSlopeLength.Size = new System.Drawing.Size(65, 12);
            this.linkLblSlopeLength.TabIndex = 16;
            this.linkLblSlopeLength.TabStop = true;
            this.linkLblSlopeLength.Text = "D8算法实现";
            this.linkLblSlopeLength.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLblSlopeLength_LinkClicked);
            // 
            // linkLblWatershed
            // 
            this.linkLblWatershed.AutoSize = true;
            this.linkLblWatershed.Location = new System.Drawing.Point(142, 56);
            this.linkLblWatershed.Name = "linkLblWatershed";
            this.linkLblWatershed.Size = new System.Drawing.Size(89, 12);
            this.linkLblWatershed.TabIndex = 15;
            this.linkLblWatershed.TabStop = true;
            this.linkLblWatershed.Text = "平均坡度和坡长";
            this.linkLblWatershed.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLblWatershed_LinkClicked);
            // 
            // linkLblFlowLength
            // 
            this.linkLblFlowLength.AutoSize = true;
            this.linkLblFlowLength.Location = new System.Drawing.Point(8, 56);
            this.linkLblFlowLength.Name = "linkLblFlowLength";
            this.linkLblFlowLength.Size = new System.Drawing.Size(53, 12);
            this.linkLblFlowLength.TabIndex = 14;
            this.linkLblFlowLength.TabStop = true;
            this.linkLblFlowLength.Text = "长度计算";
            this.linkLblFlowLength.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLblFlowLength_LinkClicked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 13;
            this.label1.Text = "主河道参数计算";
            // 
            // lallayerlist
            // 
            this.lallayerlist.AutoSize = true;
            this.lallayerlist.Location = new System.Drawing.Point(8, 9);
            this.lallayerlist.Name = "lallayerlist";
            this.lallayerlist.Size = new System.Drawing.Size(53, 12);
            this.lallayerlist.TabIndex = 13;
            this.lallayerlist.Text = "图层列表";
            // 
            // linkLblLocation
            // 
            this.linkLblLocation.AutoSize = true;
            this.linkLblLocation.Location = new System.Drawing.Point(181, 9);
            this.linkLblLocation.Name = "linkLblLocation";
            this.linkLblLocation.Size = new System.Drawing.Size(53, 12);
            this.linkLblLocation.TabIndex = 12;
            this.linkLblLocation.TabStop = true;
            this.linkLblLocation.Text = "图层定位";
            this.linkLblLocation.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLblLocation_LinkClicked);
            // 
            // linkLblLonGradient
            // 
            this.linkLblLonGradient.AutoSize = true;
            this.linkLblLonGradient.Location = new System.Drawing.Point(67, 56);
            this.linkLblLonGradient.Name = "linkLblLonGradient";
            this.linkLblLonGradient.Size = new System.Drawing.Size(65, 12);
            this.linkLblLonGradient.TabIndex = 11;
            this.linkLblLonGradient.TabStop = true;
            this.linkLblLonGradient.Text = "纵降比计算";
            this.linkLblLonGradient.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLblLonGradient_LinkClicked);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(3, 162);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(240, 285);
            this.textBox1.TabIndex = 10;
            // 
            // linkLabel4
            // 
            this.linkLabel4.AutoSize = true;
            this.linkLabel4.Location = new System.Drawing.Point(42, 132);
            this.linkLabel4.Name = "linkLabel4";
            this.linkLabel4.Size = new System.Drawing.Size(65, 12);
            this.linkLabel4.TabIndex = 14;
            this.linkLabel4.TabStop = true;
            this.linkLabel4.Text = "主河槽提取";
            this.linkLabel4.Visible = false;
            this.linkLabel4.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel4_LinkClicked);
            // 
            // HydrologyControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "HydrologyControl";
            this.Size = new System.Drawing.Size(246, 488);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.LinkLabel linkLblLocation;
        private System.Windows.Forms.LinkLabel linkLblLonGradient;
        private System.Windows.Forms.Label lallayerlist;
        private System.Windows.Forms.LinkLabel linkLblSlope;
        private System.Windows.Forms.LinkLabel linkLblSlopeLength;
        private System.Windows.Forms.LinkLabel linkLblWatershed;
        private System.Windows.Forms.LinkLabel linkLblFlowLength;
        private System.Windows.Forms.LinkLabel linkLblClear;
        private System.Windows.Forms.TextBox txtLat;
        private System.Windows.Forms.TextBox txtLon;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.LinkLabel linkLabel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel linkLabel4;




    }
}
