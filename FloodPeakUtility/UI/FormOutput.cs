using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
        }

        /// <summary>
        /// 绑定控制台的信息
        /// </summary>
        public void BindConsole()
        {
            textBox1.BindConsole();
        }

        /// <summary>
        /// 记录输出信息
        /// </summary>
        /// <param name="content">日志信息</param>
        public static void AppendLog(string content)
        {
            if (_form == null || _form.IsDisposed)
            {
                _form = new FormOutput();
                _form.StartPosition = FormStartPosition.CenterScreen;
            }
            if (_form.InvokeRequired)
            {
                _form.Invoke(new Action(() =>
                    {
                        _form.BindConsole();
                        _form.Show();
                        _form.WindowState = FormWindowState.Normal;
                        _form.Activate();
                    }));
            }
            else
            {
                _form.BindConsole();
                _form.Show();
                _form.WindowState = FormWindowState.Normal;
                _form.Activate();
            }
            MyConsole.AppendLine(content);
        }

    }
}
