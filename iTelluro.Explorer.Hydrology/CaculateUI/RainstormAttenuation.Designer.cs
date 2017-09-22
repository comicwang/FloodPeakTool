namespace FloodPeakToolUI.UI
{
    partial class RainstormAttenuation
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
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.cmbLevel = new System.Windows.Forms.ComboBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCaculate = new System.Windows.Forms.Button();
            this.txtd = new System.Windows.Forms.TextBox();
            this.txtnd = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtSd = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtn2 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtn1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtState = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // cmbLevel
            // 
            this.cmbLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLevel.FormattingEnabled = true;
            this.cmbLevel.Items.AddRange(new object[] {
            "100年一遇",
            "50年一遇",
            "20年一遇",
            "10年一遇",
            "5年一遇"});
            this.cmbLevel.Location = new System.Drawing.Point(107, 1);
            this.cmbLevel.Name = "cmbLevel";
            this.cmbLevel.Size = new System.Drawing.Size(121, 20);
            this.cmbLevel.TabIndex = 21;
            // 
            // btnSave
            // 
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Location = new System.Drawing.Point(145, 182);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(82, 41);
            this.btnSave.TabIndex = 19;
            this.btnSave.Text = "保存结果";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCaculate
            // 
            this.btnCaculate.Enabled = false;
            this.btnCaculate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCaculate.Location = new System.Drawing.Point(21, 182);
            this.btnCaculate.Name = "btnCaculate";
            this.btnCaculate.Size = new System.Drawing.Size(82, 41);
            this.btnCaculate.TabIndex = 18;
            this.btnCaculate.Text = "点击计算";
            this.btnCaculate.UseVisualStyleBackColor = true;
            this.btnCaculate.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtd
            // 
            this.txtd.Location = new System.Drawing.Point(106, 144);
            this.txtd.Name = "txtd";
            this.txtd.Size = new System.Drawing.Size(122, 21);
            this.txtd.TabIndex = 13;
            // 
            // txtnd
            // 
            this.txtnd.Location = new System.Drawing.Point(106, 120);
            this.txtnd.Name = "txtnd";
            this.txtnd.Size = new System.Drawing.Size(122, 21);
            this.txtnd.TabIndex = 17;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label7.Location = new System.Drawing.Point(13, 148);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(83, 12);
            this.label7.TabIndex = 10;
            this.label7.Text = "衰减时间参数d";
            // 
            // txtSd
            // 
            this.txtSd.Location = new System.Drawing.Point(107, 96);
            this.txtSd.Name = "txtSd";
            this.txtSd.Size = new System.Drawing.Size(121, 21);
            this.txtSd.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label6.Location = new System.Drawing.Point(31, 124);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 5;
            this.label6.Text = "衰减指数nd";
            // 
            // txtn2
            // 
            this.txtn2.Location = new System.Drawing.Point(107, 72);
            this.txtn2.Name = "txtn2";
            this.txtn2.Size = new System.Drawing.Size(121, 21);
            this.txtn2.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(31, 100);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "暴雨雨力Sd";
            // 
            // txtn1
            // 
            this.txtn1.Location = new System.Drawing.Point(106, 48);
            this.txtn1.Name = "txtn1";
            this.txtn1.Size = new System.Drawing.Size(122, 21);
            this.txtn1.TabIndex = 15;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(55, 76);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "斜率n2";
            // 
            // txtState
            // 
            this.txtState.Location = new System.Drawing.Point(106, 24);
            this.txtState.Name = "txtState";
            this.txtState.Size = new System.Drawing.Size(122, 21);
            this.txtState.TabIndex = 16;
            this.txtState.TextChanged += new System.EventHandler(this.txtState_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(55, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "斜率n1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "雨量站编号";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 11;
            this.label1.Text = "暴雨剧烈程度";
            // 
            // RainstormAttenuation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cmbLevel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCaculate);
            this.Controls.Add(this.txtd);
            this.Controls.Add(this.txtnd);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtSd);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtn2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtn1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtState);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "RainstormAttenuation";
            this.Size = new System.Drawing.Size(240, 240);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ComboBox cmbLevel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCaculate;
        private System.Windows.Forms.TextBox txtd;
        private System.Windows.Forms.TextBox txtnd;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtSd;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtn2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtn1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtState;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}
