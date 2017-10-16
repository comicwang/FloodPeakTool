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
using FloodPeakUtility.UI;
using System.IO;
using FloodPeakUtility.Model;

namespace FloodPeakToolUI.UI
{
    public partial class CaculateFirgureUI : UserControl
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
        private string _xmlPath;
        public CaculateFirgureUI()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            panel1.Location = new Point((this.Width - panel1.Width) / 2, 0);
        }

        /// <summary>
        /// 绑定UI数据
        /// </summary>
        /// <param name="args"></param>
        public void BindResult(DataSet ds,string xmlPath)
        {
            _xmlPath = xmlPath;
            tabControl1.SelectedTab = tabControl1.Tabs[0];
            //绑定数据
            if (ds == null || ds.Tables.Count == 0)
                return;
            int a = 1;  //记录控件名称
            int A = 6;
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                double value = 0;
                double during = 0;
              
                try
                {
                    during = Convert.ToDouble(row[0]);
                    value = Convert.ToDouble(row[1]);

                    // a=p/t(单位时间雨强大小)
                    value = value / during;
                }
                catch
                {
                    continue;
                }
                //判断时间间隔大于10小时的不需要加入
                if (during > 10)
                    continue;
                if (during < 1)
                {
                    bool success = this.SetArgControl("a" + a, value.ToString());
                    success = this.SetArgControl("t" + a, during.ToString());
                    if (success)
                        a++;
                }
                else if (during > 1)
                {
                    bool success = this.SetArgControl("a" + A, value.ToString());
                    success = this.SetArgControl("t" + A, during.ToString());
                    if (success)
                        A++;
                }
                    //为1的小时段，都需要加入，为交点
                else
                {
                    bool success = this.SetArgControl("a" + a, value.ToString());
                    success = this.SetArgControl("t" + a, during.ToString());
                    if (success)
                        a++;

                    success = this.SetArgControl("a" + A, value.ToString());
                    success = this.SetArgControl("t" + A, during.ToString());
                    if (success)
                        A++;
                }
            }
        
        }

        /// <summary>
        /// 根据名称设置控件的值
        /// </summary>
        /// <param name="name">控件名称</param>
        /// <param name="value">控件的值</param>
        /// <returns>是否赋值成功</returns>
        private bool SetArgControl(string name,string value)
        {
            foreach (Control ctl in pnlArg.Controls)
            {
                if (ctl.GetType() != typeof(TextBox))
                    continue;
                if (ctl.Name == name)
                {
                    ctl.Text = value;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取控件上的参数值
        /// </summary>
        /// <param name="nameIndex"></param>
        /// <returns></returns>
        private double[] GetArgControl(int nameIndex)
        {
            double[] result = new double[2];
            foreach (Control ctl in pnlArg.Controls)
            {
                if (ctl.GetType() != typeof(TextBox))
                    continue;
                if (ctl.Name.Contains(nameIndex.ToString())&&!string.IsNullOrWhiteSpace(ctl.Text))
                {
                    if (ctl.Name.Contains("a"))
                        result[1] = Convert.ToDouble(ctl.Text);
                    else
                        result[0] = Convert.ToDouble(ctl.Text);
                }
            }
            return result;
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

                IntPtr hwndHost = this.pnlFirgue.Handle;
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            //保存获取结果值
            BYSJResult result = new BYSJResult()
            {
                Sd = string.IsNullOrEmpty(SdData.Text) ? 0 : Convert.ToDouble(SdData.Text),
                nd = string.IsNullOrEmpty(ndData.Text) ? 0 : Convert.ToDouble(ndData.Text),
                d = string.IsNullOrEmpty(dData.Text) ? 0 : Convert.ToDouble(dData.Text)
            };
            XmlHelper.Serialize<BYSJResult>(result, _xmlPath);
            MsgBox.ShowInfo("保存成功！");
        }

        #region 暴雨衰减计算

        private void button1_Click(object sender, EventArgs e)
        {
            Dictionary<int,double[]> dicArgs=new Dictionary<int,double[]>();
            //获取参数值
            for (int i = 1; i < 11; i++)
            {
                double[] temp = this.GetArgControl(i);
                if (temp != null && temp.Length == 2 && temp[0] != 0 && temp[1] != 0)
                {
                    dicArgs.Add(i, temp);
                }
            }
            if (!backgroundWorker1.IsBusy)
            {
                FormOutput.AppendLog("开始计算...");
                backgroundWorker1.RunWorkerAsync(dicArgs);
            }
            else
            {
                FormOutput.AppendLog("当前后台正在计算...");
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Dictionary<int, double[]> dicArgs = e.Argument as Dictionary<int, double[]>;
            //将数据按照大于一小时和小于一小时来分类
            string minHour = string.Empty;
            string minHourValue = string.Empty;
            string maxHour = string.Empty;
            string maxHourValue = string.Empty;
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("*******************计算参数***********************");
            foreach (var item in dicArgs)
            {
                double during = item.Value[0];  //范围-值
                double value = item.Value[1];
                if (item.Key <= 5)
                {
                    minHour += during;
                    minHourValue += value;
                    minHour += ",";
                    minHourValue += ",";
                }
                else
                {
                    maxHour += during;
                    maxHourValue += value;
                    maxHourValue += ",";
                    maxHour += ",";
                }
                builder.AppendLine(string.Format("a{0} = {1}", item.Key, value));
                builder.AppendLine(string.Format("t{0} = {1}", item.Key, during));
            }
            builder.AppendLine("***************************************************");
            builder.AppendLine("开始计算暴雨衰减参数...");
            FormOutput.AppendLog(builder.ToString());

            builder = new StringBuilder();
            builder.Append(MethodName.RainStormSub);
            builder.Append(" ");
            builder.Append(minHour.Substring(0, minHour.Length - 1));
            builder.Append(" ");
            builder.Append(" ");
            builder.Append(minHourValue.Substring(0, minHourValue.Length - 1));
            builder.Append(" ");
            builder.Append(" ");
            builder.Append(maxHour.Substring(0, maxHour.Length - 1));
            builder.Append(" ");
            builder.Append(" ");
            builder.Append(maxHourValue.Substring(0, maxHourValue.Length - 1));
            builder.Append(" ");         
            RunExeHelper.RunMethod(builder.ToString());
            e.Result = "1";
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                FormOutput.AppendLog("计算期间发生异常:" + e.Error.Message);
                return;
            }
            if (e.Result != null)
                RunExeHelper.FindFigureAndTodo(ShowResult);
        }

        private void ShowResult(IntPtr windowPtr)
        {
            SubCure cv = XmlHelper.Deserialize<SubCure>(Path.Combine(Application.StartupPath, ConfigNames.SubCure));
            if (cv != null)
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                    {
                        this.DockFigure(windowPtr);
                        _currentPtr = windowPtr;
                        StringBuilder build = new StringBuilder();
                        build.AppendLine("计算结果:");
                        build.AppendLine("暴雨雨力Sd = " + cv.Sd);
                        build.AppendLine("衰减指数nd = " + cv.nd);
                        build.AppendLine("衰减时间参数d = " + cv.d);
                        build.AppendLine("t<=1时拟合曲线特性");
                        build.AppendLine("斜率n1 = " + cv.n1);
                        build.AppendLine("截距s1 = " + cv.j1);
                        build.AppendLine("t>1时拟合曲线特性");
                        build.AppendLine("斜率n2 = " + cv.n2);
                        build.AppendLine("截距s2 = " + cv.j2);
                        build.AppendLine("n2与n1的比为n2/n1 = " + cv.n2 / cv.n1);
                        if (cv.n2 / cv.n1 > 1.5)
                            build.AppendLine("斜率比大于1.5,结果无效！");

                        else
                            build.AppendLine("斜率比小于1.5,结果有效！");

                        FormOutput.AppendLog(build.ToString());
                        dData.Text = cv.d.ToString();
                        SdData.Text = cv.Sd.ToString();
                        ndData.Text = cv.nd.ToString();
                        JiejuS1.Text = cv.j1.ToString();
                        JiejuS2.Text = cv.j2.ToString();
                        Xielvn1.Text = cv.n1.ToString();
                        Xielvn2.Text = cv.n2.ToString();
                        tabControl1.SelectedTab = tabControl1.Tabs[1];
                    }));
                }
            }
        }

        #endregion

        #region d=0时的近似计算

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Dictionary<int, double[]> dicArgs = new Dictionary<int, double[]>();
            //获取参数值
            for (int i = 1; i < 11; i++)
            {
                double[] temp = this.GetArgControl(i);
                if (temp != null && temp.Length == 2 && temp[0] != 0 && temp[1] != 0)
                {
                    dicArgs.Add(i, temp);
                }
            }
            if (!backgroundWorker2.IsBusy)
            {
                FormOutput.AppendLog("开始计算...");
                backgroundWorker2.RunWorkerAsync(dicArgs);
            }
            else
            {
                FormOutput.AppendLog("当前后台正在计算...");
            }
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            Dictionary<int, double[]> dicArgs = e.Argument as Dictionary<int, double[]>;
            //将数据按照大于一小时和小于一小时来分类
            string minHour = string.Empty;
            string minHourValue = string.Empty;
            string maxHour = string.Empty;
            string maxHourValue = string.Empty;
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("*******************计算参数***********************");
            foreach (var item in dicArgs)
            {
                double during = item.Value[0];  //范围-值
                double value = item.Value[1];
                minHour += during;
                minHourValue += value;
                minHour += ",";
                minHourValue += ",";
                builder.AppendLine(string.Format("a{0} = {1}", item.Key, value));
                builder.AppendLine(string.Format("t{0} = {1}", item.Key, during));
            }
            builder.AppendLine("***************************************************");
            builder.AppendLine("开始计算暴雨衰减参数...");
            FormOutput.AppendLog(builder.ToString());

            builder = new StringBuilder();
            builder.Append(MethodName.RainStormSub0);
            builder.Append(" ");
            builder.Append(minHour.Substring(0, minHour.Length - 1));
            builder.Append(" ");
            builder.Append(minHourValue.Substring(0, minHourValue.Length - 1));
            RunExeHelper.RunMethod(builder.ToString());
            e.Result = "1";
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                FormOutput.AppendLog("计算期间发生异常:" + e.Error.Message);
                return;
            }
            if (e.Result != null)
                RunExeHelper.FindFigureAndTodo(ShowResult0);
        }

        private void ShowResult0(IntPtr windowPtr)
        {
            SubCure cv = XmlHelper.Deserialize<SubCure>(Path.Combine(Application.StartupPath, ConfigNames.SubCure0));
            if (cv != null)
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                    {
                        this.DockFigure(windowPtr);
                        _currentPtr = windowPtr;
                        StringBuilder build = new StringBuilder();
                        build.AppendLine("计算结果:");
                        build.AppendLine("暴雨雨力Sd = " + cv.Sd);
                        build.AppendLine("衰减指数nd = " + cv.nd);
                        build.AppendLine("斜率n = " + cv.n1);
                        build.AppendLine("截距s = " + cv.j1);
                        FormOutput.AppendLog(build.ToString());
                        SdData2.Text = cv.Sd.ToString();
                        ndData2.Text = cv.nd.ToString();
                        Jieju.Text = cv.j1.ToString();
                        Xielv.Text = cv.n1.ToString();
                        tabControl1.SelectedTab = tabControl1.Tabs[2];
                    }));
                }

            }
        }

        #endregion

        private void pnlBottom_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
