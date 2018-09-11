namespace FloodPeakToolUI.UI
{
    partial class MainCaculateControl
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
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.button2 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.txttc = new System.Windows.Forms.TextBox();
            this.txteps2 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txteps1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtQm = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label6 = new System.Windows.Forms.Label();
            this.txtp1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(22, 141);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(67, 26);
            this.button2.TabIndex = 19;
            this.button2.Text = "查看参数";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 117);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(119, 12);
            this.label4.TabIndex = 18;
            this.label4.Text = "产流历时初值tc_0(h)";
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(160, 141);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(67, 26);
            this.button1.TabIndex = 15;
            this.button1.Text = "点击计算";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txttc
            // 
            this.txttc.Location = new System.Drawing.Point(132, 112);
            this.txttc.Name = "txttc";
            this.txttc.Size = new System.Drawing.Size(95, 21);
            this.txttc.TabIndex = 11;
            // 
            // txteps2
            // 
            this.txteps2.Location = new System.Drawing.Point(132, 87);
            this.txteps2.Name = "txteps2";
            this.txteps2.Size = new System.Drawing.Size(95, 21);
            this.txteps2.TabIndex = 12;
            this.txteps2.Text = "0.000001";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(53, 92);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 12);
            this.label5.TabIndex = 7;
            this.label5.Text = "迭代精度eps2";
            // 
            // txteps1
            // 
            this.txteps1.Location = new System.Drawing.Point(132, 62);
            this.txteps1.Name = "txteps1";
            this.txteps1.Size = new System.Drawing.Size(95, 21);
            this.txteps1.TabIndex = 13;
            this.txteps1.Text = "0.00001";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(53, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "迭代精度eps1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "洪峰历时系数p1_0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 12);
            this.label1.TabIndex = 10;
            this.label1.Text = "清水洪峰流量初值Q _0";
            // 
            // txtQm
            // 
            this.txtQm.Location = new System.Drawing.Point(132, 12);
            this.txtQm.Name = "txtQm";
            this.txtQm.Size = new System.Drawing.Size(95, 21);
            this.txtQm.TabIndex = 14;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 6F);
            this.label6.Location = new System.Drawing.Point(109, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(9, 8);
            this.label6.TabIndex = 20;
            this.label6.Text = "B";
            // 
            // txtp1
            // 
            this.txtp1.Location = new System.Drawing.Point(132, 37);
            this.txtp1.Name = "txtp1";
            this.txtp1.Size = new System.Drawing.Size(95, 21);
            this.txtp1.TabIndex = 14;
            // 
            // MainCaculateControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label6);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txttc);
            this.Controls.Add(this.txteps2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txteps1);
            this.Controls.Add(this.txtQm);
            this.Controls.Add(this.txtp1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "MainCaculateControl";
            this.Size = new System.Drawing.Size(240, 240);
            this.Load += new System.EventHandler(this.MainCaculateControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txttc;
        private System.Windows.Forms.TextBox txteps2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txteps1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtQm;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtp1;
    }
}
