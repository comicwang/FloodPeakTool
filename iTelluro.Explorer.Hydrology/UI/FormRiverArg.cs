using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FloodPeakToolUI.UI
{
    public partial class FormRiverArg : Form
    {
        /// <summary>
        /// 获取河网阀值
        /// </summary>
        public int RiverThreshold { get { return (int)numericUpDown1.Value; } }

        /// <summary>
        /// 获取填洼Z值
        /// </summary>
        public int FillThreshold { get { return (int)numericUpDown2.Value; } }

        /// <summary>
        /// 获取河网输出路径
        /// </summary>
        public string RiverPath { get { return textBox1.Text; } }

        /// <summary>
        /// 获取填洼输出路径
        /// </summary>
        public string FillPath { get { return textBox2.Text; } }

        /// <summary>
        /// 获取流向输出路径
        /// </summary>
        public string DirectionPath { get { return textBox3.Text; } }

        /// <summary>
        /// 获取流量输出路径
        /// </summary>
        public string AccumulationPath { get { return textBox4.Text; } }

        public FormRiverArg()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog svDialog = new SaveFileDialog();
            svDialog.Filter = "栅格文件|*.tif";

            Control parent = (sender as Control).Parent;
            TextBox txtBox = parent.Controls.Find($"textbox{parent.Name[parent.Name.Length - 1]}", false).First() as TextBox;
            if (txtBox != null)
            {
                if (svDialog.ShowDialog() == DialogResult.OK)
                {
                    txtBox.Text = svDialog.FileName;
                }
                else
                {
                    txtBox.Text = string.Empty;
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            btnOk.Enabled = !string.IsNullOrEmpty(textBox1.Text);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
