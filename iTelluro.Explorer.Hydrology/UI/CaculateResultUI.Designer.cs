namespace FloodPeakToolUI.UI
{
    partial class CaculateResultUI
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtQm = new System.Windows.Forms.TextBox();
            this.txtt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtP1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtd2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtd1 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txta1tc = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txttQ = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.groupBox1);
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
            this.panel2.Size = new System.Drawing.Size(701, 297);
            this.panel2.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtQm);
            this.groupBox1.Controls.Add(this.txtt);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtP1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtd2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtd1);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txta1tc);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txttQ);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 297);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(701, 202);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "洪峰流量统计结果";
            // 
            // txtQm
            // 
            this.txtQm.Location = new System.Drawing.Point(186, 33);
            this.txtQm.Name = "txtQm";
            this.txtQm.Size = new System.Drawing.Size(126, 21);
            this.txtQm.TabIndex = 27;
            // 
            // txtt
            // 
            this.txtt.Location = new System.Drawing.Point(444, 68);
            this.txtt.Name = "txtt";
            this.txtt.Size = new System.Drawing.Size(126, 21);
            this.txtt.TabIndex = 28;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(113, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 21;
            this.label2.Text = "洪峰流量Qm";
            // 
            // txtP1
            // 
            this.txtP1.Location = new System.Drawing.Point(444, 33);
            this.txtP1.Name = "txtP1";
            this.txtP1.Size = new System.Drawing.Size(126, 21);
            this.txtP1.TabIndex = 29;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(113, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 22;
            this.label3.Text = "造峰历时tQ";
            // 
            // txtd2
            // 
            this.txtd2.Location = new System.Drawing.Point(513, 108);
            this.txtd2.Name = "txtd2";
            this.txtd2.Size = new System.Drawing.Size(57, 21);
            this.txtd2.TabIndex = 30;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(345, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 23;
            this.label1.Text = "洪峰历时系数P1";
            // 
            // txtd1
            // 
            this.txtd1.Location = new System.Drawing.Point(444, 107);
            this.txtd1.Name = "txtd1";
            this.txtd1.Size = new System.Drawing.Size(57, 21);
            this.txtd1.TabIndex = 31;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(65, 110);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(113, 12);
            this.label5.TabIndex = 24;
            this.label5.Text = "产流期净雨强度a1tc";
            // 
            // txta1tc
            // 
            this.txta1tc.Location = new System.Drawing.Point(186, 105);
            this.txta1tc.Name = "txta1tc";
            this.txta1tc.Size = new System.Drawing.Size(126, 21);
            this.txta1tc.TabIndex = 32;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(351, 73);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 12);
            this.label4.TabIndex = 25;
            this.label4.Text = "洪水上涨历时t";
            // 
            // txttQ
            // 
            this.txttQ.Location = new System.Drawing.Point(186, 68);
            this.txttQ.Name = "txttQ";
            this.txttQ.Size = new System.Drawing.Size(126, 21);
            this.txttQ.TabIndex = 33;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(381, 112);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 26;
            this.label6.Text = "迭代次数";
            // 
            // CaculateResultUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panel1);
            this.Name = "CaculateResultUI";
            this.Size = new System.Drawing.Size(709, 507);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtQm;
        private System.Windows.Forms.TextBox txtt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtP1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtd2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtd1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txta1tc;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txttQ;
        private System.Windows.Forms.Label label6;

    }
}
