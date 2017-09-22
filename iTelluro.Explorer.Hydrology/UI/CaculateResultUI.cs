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

        public void BindResult(MainResult args, IntPtr windowPtr)
        {
            //初始化参数
            if (args != null)
            {
                txtQm.Text = args.Qm.ToString();
                txtP1.Text = args.p1.ToString();
                txttQ.Text = args.tQ.ToString();
                txtt.Text = args.t.ToString();
                txta1tc.Text = args.a1tc.ToString();
                txtd1.Text = args.d1.ToString();
                txtd2.Text = args.d2.ToString();
            }
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
    }
}
