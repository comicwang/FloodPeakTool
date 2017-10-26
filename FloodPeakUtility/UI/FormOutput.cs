﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace FloodPeakUtility.UI
{
    /// <summary>
    /// 模拟控制台窗口
    /// </summary>
    public partial class FormOutput : Form
    {
        private static FormOutput _form = null;
        public FormOutput()
        {
            InitializeComponent();
            //读取位置坐标
            try
            {
                OutputLocation location = XmlHelper.Deserialize<OutputLocation>(Path.Combine(Application.StartupPath, ConfigNames.OutputLocation));
                if (location != null)
                    this.Location = new Point(location.X, location.Y);
                else
                    this.StartPosition = FormStartPosition.CenterScreen;
            }
            catch { }
        }

        /// <summary>
        /// 绑定控制台的信息
        /// </summary>
        public void BindConsole()
        {
            textBox1.BindConsole();
        }

        /// <summary>
        /// 设置进度条进度
        /// </summary>
        /// <param name="process"></param>
        public void SetProgress(int process)
        {
            this.progressBar1.Value = process;
        }

        /// <summary>
        /// 设置进度条状态
        /// </summary>
        /// <param name="visible"></param>
        public void SetProgress(bool visible)
        {
            this.progressBar1.Visible = visible;
        }

        /// <summary>
        /// 记录输出信息
        /// </summary>
        /// <param name="content">日志信息</param>
        public static void AppendLog(string content)
        {
            try
            {
                if (_form == null || _form.IsDisposed)
                {
                    _form = new FormOutput();
                    _form.BindConsole();
                }
                if (_form.InvokeRequired)
                {
                    _form.Invoke(new Action(() =>
                        {
                            _form.Show();
                            _form.WindowState = FormWindowState.Normal;
                            _form.Activate();
                        }));
                }
                else
                {
                    _form.Show();
                    _form.WindowState = FormWindowState.Normal;
                    _form.Activate();
                }
                MyConsole.AppendLine(content);
            }
            catch { }
        }

        /// <summary>
        /// 设置进度条状态值
        /// </summary>
        /// <param name="percent"></param>
        public static void AppendProress(int percent)
        {
            try
            {
                if (_form == null || _form.IsDisposed)
                {
                    return;
                }
                if (_form.InvokeRequired)
                {
                    _form.Invoke(new Action(() =>
                        {
                            _form.SetProgress(percent);
                        }));
                }
                else
                {
                    _form.SetProgress(percent);
                }
            }
            catch { }
        }

        /// <summary>
        /// 设置进度条可见状态
        /// </summary>
        /// <param name="visiable"></param>
        public static void AppendProress(bool visiable)
        {
            try
            {
                if (_form == null || _form.IsDisposed)
                {
                    return;
                }
                if (_form.InvokeRequired)
                {
                    _form.Invoke(new Action(() =>
                    {
                        _form.SetProgress(visiable);
                        _form.SetProgress(0);
                    }));
                }
                else
                {
                    _form.SetProgress(visiable);
                    _form.SetProgress(0);
                }
            }
            catch { }
        }

        private void buttonItem2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 保存日志文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonItem1_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "文本文件|*.txt";
            if(dialog.ShowDialog()==DialogResult.OK)
            {
                File.WriteAllText(dialog.FileName, textBox1.Text);
                System.Diagnostics.Process.Start("Explorer.exe", Path.GetDirectoryName(dialog.FileName));
            }
        }

        /// <summary>
        /// 记录位置坐标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormOutput_FormClosing(object sender, FormClosingEventArgs e)
        {
            XmlHelper.Serialize<OutputLocation>(new OutputLocation()
                {
                    X = this.Location.X,
                    Y = this.Location.Y

                }, Path.Combine(Application.StartupPath, ConfigNames.OutputLocation));

            this.Hide();
            e.Cancel = true;
        }

        #region

        [DllImport("user32.dll")]//*********************拖动无窗体的控件  
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;

        private void gPanelTitleBack_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);//*********************调用移动无窗体控件函数  
        }

        #endregion

    }
}
