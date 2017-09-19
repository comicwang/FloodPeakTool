using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace FloodPeakTool
{
    public partial class NewProject : Form
    {
        public NewProject()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 项目文件所在路径
        /// </summary>
        public string ProjectPath { get;private set; }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "请选择项目所在文件夹";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtFileRoad.Text = dialog.SelectedPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //创建项目必要文件
            string path = txtFileRoad.Text;
            string file = txtFileName.Text;
            string FullPath = Path.Combine(path, file);
            if (Directory.Exists(FullPath) == false)
                Directory.CreateDirectory(FullPath);
            string fullName = Path.Combine(FullPath, file + ".hfll");
            using (File.Create(fullName))
            {

            }
            ProjectPath = fullName;
            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// 控制输入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtFileName_TextChanged(object sender, EventArgs e)
        {
            btnOk.Enabled = !string.IsNullOrWhiteSpace(txtFileName.Text) && !string.IsNullOrWhiteSpace(txtFileRoad.Text) && Directory.Exists(txtFileRoad.Text);
        }


    }
}
