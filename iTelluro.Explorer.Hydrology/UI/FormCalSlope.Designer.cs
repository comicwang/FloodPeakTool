namespace FloodPeakToolUI.UI
{
    partial class FormCalSlope
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnAddContour = new System.Windows.Forms.Button();
            this.btnAddFlow = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.btnCal = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(24, 21);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(502, 21);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "E:\\Project\\shpData\\水文分析\\河道.shp";
            // 
            // btnAddContour
            // 
            this.btnAddContour.Location = new System.Drawing.Point(532, 21);
            this.btnAddContour.Name = "btnAddContour";
            this.btnAddContour.Size = new System.Drawing.Size(75, 23);
            this.btnAddContour.TabIndex = 1;
            this.btnAddContour.Text = "添加河网";
            this.btnAddContour.UseVisualStyleBackColor = true;
            this.btnAddContour.Click += new System.EventHandler(this.btnAddContour_Click);
            // 
            // btnAddFlow
            // 
            this.btnAddFlow.Location = new System.Drawing.Point(532, 62);
            this.btnAddFlow.Name = "btnAddFlow";
            this.btnAddFlow.Size = new System.Drawing.Size(75, 23);
            this.btnAddFlow.TabIndex = 3;
            this.btnAddFlow.Text = "流域面";
            this.btnAddFlow.UseVisualStyleBackColor = true;
            this.btnAddFlow.Click += new System.EventHandler(this.btnAddFlow_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(24, 62);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(502, 21);
            this.textBox2.TabIndex = 2;
            this.textBox2.Text = "E:\\Project\\shpData\\水文分析\\流域面1.shp";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(532, 106);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "集水点";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(24, 106);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(502, 21);
            this.textBox3.TabIndex = 4;
            this.textBox3.Text = "E:\\Project\\shpData\\水文分析\\实验积水点.shp";
            // 
            // btnCal
            // 
            this.btnCal.Location = new System.Drawing.Point(254, 154);
            this.btnCal.Name = "btnCal";
            this.btnCal.Size = new System.Drawing.Size(120, 23);
            this.btnCal.TabIndex = 7;
            this.btnCal.Text = "计算坡度与坡长";
            this.btnCal.UseVisualStyleBackColor = true;
            this.btnCal.Click += new System.EventHandler(this.btnCal_Click);
            // 
            // FormCalSlope
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(626, 199);
            this.Controls.Add(this.btnCal);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.btnAddFlow);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.btnAddContour);
            this.Controls.Add(this.textBox1);
            this.Name = "FormCalSlope";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnAddContour;
        private System.Windows.Forms.Button btnAddFlow;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Button btnCal;
    }
}