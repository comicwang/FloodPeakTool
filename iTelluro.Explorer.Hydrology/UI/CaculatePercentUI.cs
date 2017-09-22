using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MathWorks.MATLAB.NET.Arrays;
using FuncforHFLL;
using FloodPeakUtility;
using System.Runtime.InteropServices;
using System.Threading;

namespace FloodPeakToolUI.UI
{
    public partial class CaculatePercentUI : UserControl
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
        /// 计算频率UI
        /// </summary>
        public CaculatePercentUI()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 绑定UI数据
        /// </summary>
        /// <param name="args"></param>
        public void BindResult(CvCure args, IntPtr windowPtr)
        {
            //初始化参数
            if (args != null)
            {
                numX.Value = Convert.ToDecimal(args.X);
                numCv.Value = Convert.ToDecimal(args.Cv);
                numCs.Value = Convert.ToDecimal(args.Cs);
                txtNihe.Text = args.Nihe;
            }
            //初始化曲线
            if (windowPtr != IntPtr.Zero)
            {
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
            }
        }

        /// <summary>
        /// 调整位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            panel1.Location = new Point((this.Width - panel1.Width) / 2, 0);
        }

        /// <summary>
        /// 适线计算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCaculate_Click(object sender, EventArgs e)
        {
            double X = Convert.ToDouble(numX.Value);
            MWNumericArray XX = new MWNumericArray(X);
            double Cv = Convert.ToDouble(numCv.Value);
            MWNumericArray Cvv = new MWNumericArray(Cv);
            double Cs = Convert.ToDouble(numCs.Value);
            MWNumericArray Css = new MWNumericArray(Cs);
            //double k = Convert.ToDouble(numkik.Value);
            //MWNumericArray kik = new MWNumericArray(k);
            Class1 CC = new Class1();
            double[,] Nihe = (double[,])CC.peixian(Cvv, Css, XX).ToArray();
            txtNihe.Text = Nihe[0, 0].ToString();
        }

        /// <summary>
        /// 概率反查
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                double X = Convert.ToDouble(numX.Value);
                MWNumericArray XX = new MWNumericArray(X);
                double Cv = Convert.ToDouble(numCv.Value);
                MWNumericArray Cvv = new MWNumericArray(Cv);
                double Cs = Convert.ToDouble(numCs.Value);
                MWNumericArray Css = new MWNumericArray(Cs);
                double k = Convert.ToDouble(numkik.Value);
                MWNumericArray kik = new MWNumericArray(k);
                double Q = Convert.ToDouble(numQm.Value);
                MWNumericArray Qm = new MWNumericArray(Q);
                Class1 CC = new Class1();
                if (k == 0 && Q == 0)
                    return;
                else if (k != 0 && Q != 0)
                    return;
                else if (k != 0 && Q == 0)
                {
                    double[,] Xcha = (double[,])CC.chaxun1(Cvv, Css, XX, kik).ToArray();
                    numQm.Value = Convert.ToDecimal(Xcha[0, 0]);
                }
                else if (k == 0 && Q != 0)
                {
                    double[,] Xcha = (double[,])CC.chaxun2(Cvv, Css, XX, Qm).ToArray();
                    numkik.Value = Convert.ToDecimal(Xcha[0, 0]);
                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        /// <summary>
        /// 将当前时间段当前站点的数据入库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInsert_Click(object sender, EventArgs e)
        {
         
        }

        private void bgwCaculate_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void bgwCaculate_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }
    }
}
