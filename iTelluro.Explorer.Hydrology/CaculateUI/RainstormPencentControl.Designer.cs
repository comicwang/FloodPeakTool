namespace FloodPeakToolUI.UI
{
    partial class RainstormAttenuationControl
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
            this.cmbPercent = new System.Windows.Forms.ComboBox();
            this.btnCaculate = new System.Windows.Forms.Button();
            this.txtState = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblExport = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // cmbPercent
            // 
            this.cmbPercent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPercent.FormattingEnabled = true;
            this.cmbPercent.Location = new System.Drawing.Point(107, 1);
            this.cmbPercent.Name = "cmbPercent";
            this.cmbPercent.Size = new System.Drawing.Size(121, 20);
            this.cmbPercent.TabIndex = 19;
            // 
            // btnCaculate
            // 
            this.btnCaculate.Enabled = false;
            this.btnCaculate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCaculate.Location = new System.Drawing.Point(160, 59);
            this.btnCaculate.Name = "btnCaculate";
            this.btnCaculate.Size = new System.Drawing.Size(67, 26);
            this.btnCaculate.TabIndex = 17;
            this.btnCaculate.Text = "点击计算";
            this.btnCaculate.UseVisualStyleBackColor = true;
            this.btnCaculate.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtState
            // 
            this.txtState.Location = new System.Drawing.Point(106, 29);
            this.txtState.Name = "txtState";
            this.txtState.Size = new System.Drawing.Size(122, 21);
            this.txtState.TabIndex = 15;
            this.txtState.TextChanged += new System.EventHandler(this.txtState_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 33);
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
            this.label1.TabIndex = 10;
            this.label1.Text = "频率统计项目";
            // 
            // lblExport
            // 
            this.lblExport.AutoSize = true;
            this.lblExport.Location = new System.Drawing.Point(19, 66);
            this.lblExport.Name = "lblExport";
            this.lblExport.Size = new System.Drawing.Size(53, 12);
            this.lblExport.TabIndex = 32;
            this.lblExport.TabStop = true;
            this.lblExport.Text = "频率导出";
            // 
            // RainstormAttenuationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblExport);
            this.Controls.Add(this.cmbPercent);
            this.Controls.Add(this.btnCaculate);
            this.Controls.Add(this.txtState);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "RainstormAttenuationControl";
            this.Size = new System.Drawing.Size(240, 240);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ComboBox cmbPercent;
        private System.Windows.Forms.Button btnCaculate;
        private System.Windows.Forms.TextBox txtState;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel lblExport;
    }
}
