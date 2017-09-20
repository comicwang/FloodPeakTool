namespace FloodPeakToolUI.UI
{
    partial class SlopeConfluenceControl
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.fileChooseControl2 = new FloodPeakToolUI.UI.FileChooseControl();
            this.fileChooseControl1 = new FloodPeakToolUI.UI.FileChooseControl();
            this.fileChooseControl3 = new FloodPeakToolUI.UI.FileChooseControl();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.textBox3);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 72);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(240, 168);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "输出";
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(143, 96);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(82, 41);
            this.button2.TabIndex = 5;
            this.button2.Text = "保存结果";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(24, 96);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(82, 41);
            this.button1.TabIndex = 2;
            this.button1.Text = "点击计算";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(112, 64);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(115, 21);
            this.textBox3.TabIndex = 1;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(112, 39);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(115, 21);
            this.textBox2.TabIndex = 1;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(112, 14);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(115, 21);
            this.textBox1.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "坡面流速系数A2";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "流域平均坡度l2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "流域平均坡长L2";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // fileChooseControl2
            // 
            this.fileChooseControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.fileChooseControl2.FileType = FloodPeakUtility.Model.ImportType.Shp;
            this.fileChooseControl2.LeftTitle = "流速系数";
            this.fileChooseControl2.Location = new System.Drawing.Point(0, 48);
            this.fileChooseControl2.Name = "fileChooseControl2";
            this.fileChooseControl2.Size = new System.Drawing.Size(240, 24);
            this.fileChooseControl2.TabIndex = 1;
            this.fileChooseControl2.OnSelectIndexChanged += new System.EventHandler(this.fileChooseControl3_OnSelectIndexChanged);
            // 
            // fileChooseControl1
            // 
            this.fileChooseControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.fileChooseControl1.FileType = FloodPeakUtility.Model.ImportType.Dom;
            this.fileChooseControl1.LeftTitle = "网河影像";
            this.fileChooseControl1.Location = new System.Drawing.Point(0, 24);
            this.fileChooseControl1.Name = "fileChooseControl1";
            this.fileChooseControl1.Size = new System.Drawing.Size(240, 24);
            this.fileChooseControl1.TabIndex = 0;
            this.fileChooseControl1.OnSelectIndexChanged += new System.EventHandler(this.fileChooseControl3_OnSelectIndexChanged);
            // 
            // fileChooseControl3
            // 
            this.fileChooseControl3.Dock = System.Windows.Forms.DockStyle.Top;
            this.fileChooseControl3.FileType = FloodPeakUtility.Model.ImportType.Shp;
            this.fileChooseControl3.LeftTitle = "主河槽";
            this.fileChooseControl3.Location = new System.Drawing.Point(0, 0);
            this.fileChooseControl3.Name = "fileChooseControl3";
            this.fileChooseControl3.Size = new System.Drawing.Size(240, 24);
            this.fileChooseControl3.TabIndex = 3;
            this.fileChooseControl3.OnSelectIndexChanged += new System.EventHandler(this.fileChooseControl3_OnSelectIndexChanged);
            // 
            // SlopeConfluenceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.fileChooseControl2);
            this.Controls.Add(this.fileChooseControl1);
            this.Controls.Add(this.fileChooseControl3);
            this.Name = "SlopeConfluenceControl";
            this.Size = new System.Drawing.Size(240, 240);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private FileChooseControl fileChooseControl1;
        private FileChooseControl fileChooseControl2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private FileChooseControl fileChooseControl3;
        private System.Windows.Forms.Button button2;
    }
}
