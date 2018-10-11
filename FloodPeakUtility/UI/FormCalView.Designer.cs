namespace FloodPeakUtility.UI
{
    partial class FormCalView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCalView));
            this.pnlDock = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlColor = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlDock.SuspendLayout();
            this.pnlColor.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlDock
            // 
            this.pnlDock.AutoScroll = true;
            this.pnlDock.Controls.Add(this.panel1);
            this.pnlDock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDock.Location = new System.Drawing.Point(0, 0);
            this.pnlDock.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlDock.Name = "pnlDock";
            this.pnlDock.Size = new System.Drawing.Size(820, 598);
            this.pnlDock.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(819, 592);
            this.panel1.TabIndex = 0;
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
            // 
            // pnlColor
            // 
            this.pnlColor.Controls.Add(this.label1);
            this.pnlColor.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlColor.Location = new System.Drawing.Point(820, 0);
            this.pnlColor.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlColor.Name = "pnlColor";
            this.pnlColor.Size = new System.Drawing.Size(241, 598);
            this.pnlColor.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 574);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 15);
            this.label1.TabIndex = 0;
            // 
            // FormCalView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1061, 598);
            this.Controls.Add(this.pnlDock);
            this.Controls.Add(this.pnlColor);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FormCalView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "影像数据预览";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.pnlDock.ResumeLayout(false);
            this.pnlColor.ResumeLayout(false);
            this.pnlColor.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlDock;
        private System.Windows.Forms.Panel pnlColor;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
    }
}