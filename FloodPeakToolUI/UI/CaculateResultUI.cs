using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FloodPeakUtility;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using FloodPeakUtility.UI;

namespace FloodPeakToolUI.UI
{
    public partial class CaculateResultUI : UserControl
    {
        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [DllImport("user32.dll")]
        public static extern int GetClientRect(IntPtr hwnd, ref RECT rc);

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        /// <summary>最大化窗口，最小化窗口，正常大小窗口
        /// nCmdShow:0隐藏,3最大化,6最小化，5正常显示
        /// </summary>
        [DllImport("user32.dll", EntryPoint = "ShowWindow")]
        public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);
        public class _SW
        {
            public const int SW_HIDE = 0;
            public const int SW_SHOWNORMAL = 1;
            public const int SW_SHOWMINIMIZED = 2;
            public const int SW_SHOWMAXIMIZED = 3;
            public const int SW_MAXIMIZE = 3;
            public const int SW_SHOWNOACTIVATE = 4;
            public const int SW_SHOW = 5;
            public const int SW_MINIMIZE = 6;
            public const int SW_SHOWMINNOACTIVE = 7;
            public const int SW_SHOWNA = 8;
            public const int SW_RESTORE = 9;
        }

        internal const int
           WS_CHILD = 0x40000000,
           WS_VISIBLE = 0x10000000,
           LBS_NOTIFY = 0x00000001,
           HOST_ID = 0x00000002,
           LISTBOX_ID = 0x00000001,
           WS_VSCROLL = 0x00200000,
           WS_BORDER = 0x00800000,
           GWL_STYLE = -16,
           WS_DLGFRAME = 0x00400000;

        /// <summary>
        /// 当前Figure窗口句柄值
        /// </summary>
        private IntPtr _currentPtr = IntPtr.Zero;
        public CaculateResultUI()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            panel1.Location = new Point((this.Width - panel1.Width) / 2, 0);
        }

        private string _filePath = string.Empty;
        public void BindResult(string filePath, IntPtr windowPtr)
        {
            _filePath = filePath;
            //开启一个线程来读取xml文件，因为数据是在图形绘制完之后才出来
            Thread thread = new Thread(new ThreadStart(delegate {            
                while(true)
                {
                    if(File.Exists(filePath))
                    {
                        this.Invoke(new Action(() =>
                            {
                                MainResult cv = XmlHelper.Deserialize<MainResult>(filePath);
                                //初始化参数
                                if (cv != null)
                                {
                                    StringBuilder builder = new StringBuilder();
                                    builder.AppendLine("计算结果：");
                                    builder.AppendLine("洪峰流量Qm = " + cv.Qm);
                                    builder.AppendLine("洪峰历时系数p1_0 = " + cv.p1);
                                    builder.AppendLine("造峰历时tQ = " + cv.tQ);
                                    builder.AppendLine("洪峰上涨历时t = " + cv.t);
                                    builder.AppendLine("产流期净雨强a1tc = " + cv.a1tc);
                                    builder.AppendLine("迭代次数:" + cv.d1 + "-" + cv.d2);
                                    FormOutput.AppendLog(builder.ToString());
                                    txtQm.Text = cv.Qm.ToString();
                                    txtP1.Text = cv.p1.ToString();
                                    txttQ.Text = cv.tQ.ToString();
                                    txtt.Text = cv.t.ToString();
                                    txta1tc.Text = cv.a1tc.ToString();
                                    txtd1.Text = cv.d1.ToString();
                                    txtd2.Text = cv.d2.ToString();
                                }
                            }));
                        break;
                    }
                }

            }));
            thread.IsBackground = true;
            thread.Start();
         
            //初始化曲线
            this.DockFigure(windowPtr);
        }

        /// <summary>
        /// 将Figure窗口Dock到Panel上
        /// </summary>
        /// <param name="windowPtr"></param>
        private void DockFigure(IntPtr windowPtr)
        {
            if (windowPtr != IntPtr.Zero)
            {
                //杀死其他进程
                RunExeHelper.KillByIntPtr(_currentPtr);

                IntPtr hwndHost = this.panel2.Handle;
                Int32 wndStyle = GetWindowLong(windowPtr, GWL_STYLE);
                wndStyle &= ~WS_BORDER;
                //wndStyle &= ~WS_THICKFRAME;
                SetWindowLong(windowPtr, GWL_STYLE, wndStyle);
                ShowWindow(windowPtr, _SW.SW_HIDE);
                RECT pr;
                GetWindowRect(hwndHost, out pr);
                // GetClientRect(hwndHost,ref pr);
                SetParent(windowPtr, hwndHost);
                MoveWindow(windowPtr, 0, 0, pr.right - pr.left, pr.bottom - pr.top, true);
                ShowWindow(windowPtr, _SW.SW_SHOW);
                _currentPtr = windowPtr;
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Excel文件|*.xls";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(MethodName.SdQmTable);
                builder.Append(" ");
                builder.Append(Path.GetDirectoryName(_filePath));
                builder.Append(" ");
                builder.Append(dialog.FileName);
                string result = RunExeHelper.RunMethodExit(builder.ToString());
                //输出字符串过多！
                if (result.Contains("导出完成"))
                {
                    MsgBox.ShowInfo("导出完成！");
                    System.Diagnostics.Process.Start("Explorer.exe", Path.GetDirectoryName(dialog.FileName));
                }
                else
                {
                    MsgBox.ShowInfo(result);
                }
            }
        }
    }
}
