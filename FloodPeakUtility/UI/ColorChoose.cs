/********************************************************************************
    ** auth： 王冲
    ** date： 2017/10/30 15:59:37
    ** desc： 尚未编写描述
    ** Ver.:  V1.0.0
*********************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FloodPeakUtility.UI
{
    public partial class ColorChoose : UserControl
    {
        public ColorChoose()
        {
            InitializeComponent();
        }

        public ColorChoose(double value, Color color)
            : this()
        {
            ColorValue = value;
            SelectColor = color;
        }

        /// <summary>
        /// 颜色值
        /// </summary>
        public double ColorValue
        {
            get { return Convert.ToDouble(numericUpDown1.Value); }
            set { numericUpDown1.Value = Convert.ToDecimal(value); }
        }

        /// <summary>
        /// 颜色
        /// </summary>
        public Color SelectColor
        {
            get { return panel1.BackColor; }
            set { panel1.BackColor = value; }
        }

        /// <summary>
        /// 是否显示移除按钮
        /// </summary>
        public bool ShowRemove
        {
            get { return pictureBox1.Visible; }
            set { pictureBox1.Visible = value; }
        }

        /// <summary>
        /// 显示线
        /// </summary>
        public bool ShowDock
        {
            get { return panel2.Visible; }
            set { panel2.Visible = value; }
        }

        public bool ValueEnable
        {
            get { return numericUpDown1.Enabled; }
            set { numericUpDown1.Enabled = value; }
        }

        public string LeftText
        {
            get { return label1.Text; }
            set { label1.Text = value; }
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog();
            dialog.Color = SelectColor;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                SelectColor = dialog.Color;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Parent.Controls.Remove(this);
        }
    }
}
