using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FloodPeakUtility
{
    /// <summary>
    /// 提供统一的Msg消息
    /// </summary>
    public class MsgBox
    {
        /// <summary>
        /// 显示提示消息
        /// </summary>
        /// <param name="msg"></param>
        public static void ShowInfo(string msg)
        {
            MessageBox.Show(msg, "提示消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 显示错误消息
        /// </summary>
        /// <param name="msg"></param>
        public static void ShowError(string msg)
        {
            MessageBox.Show(msg, "错误消息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            LogHelper.LogErro(msg);
        }

        /// <summary>
        /// 显示询问消息-是、否、取消
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static DialogResult ShowThreeAsk(string msg)
        {
            return MessageBox.Show(msg, "消息", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
        }

        /// <summary>
        /// 显示询问消息-是、否
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static DialogResult ShowAsk(string msg)
        {
            return MessageBox.Show(msg, "消息", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }
    }
}
