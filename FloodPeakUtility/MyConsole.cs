using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FloodPeakUtility
{
    /// <summary>
    /// 自定义一个输出控制台
    /// </summary>
    public static class MyConsole
    {
        static MyConsole() { }

        static TextBox _textBox = null;

        /// <summary>
        /// 绑定该输出控制台（程序唯一）
        /// </summary>
        /// <param name="textBox">文本控件-Mutiline为True</param>
        public static void BindConsole(this TextBox textBox)
        {
            textBox.Multiline = true;
            textBox.ScrollBars = ScrollBars.Vertical;
            _textBox = textBox;
        }

        /// <summary>
        /// 给控制台输出一段文本
        /// </summary>
        /// <param name="content">文本内容</param>
        /// MyConsole未绑定而是用报错ArgumentNullException
        public static void AppendLine(string content)
        {
            if (_textBox == null)
                throw new ArgumentNullException("console is unbind");
            if (_textBox.InvokeRequired)
                _textBox.Invoke(new Action<string>((t) => { _textBox.AppendText(t + Environment.NewLine + Environment.NewLine); }), content);
            else
                _textBox.AppendText(content + Environment.NewLine + Environment.NewLine);
        }
    }
}
