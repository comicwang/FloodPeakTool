using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace FloodPeakUtility
{
    public class RunExeHelper
    {
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        /// <summary>
        /// 记录计算后台，在程序退出的时候，杀死这些后台
        /// </summary>
        private static List<Process> _processes = new List<Process>();

        /// <summary>
        /// 运行一个计算的后台
        /// </summary>
        /// <param name="methodName">需要计算的后台名称</param>
        public static void RunMethod(string methodName)
        {
            string exePath = Path.Combine(Application.StartupPath, "CaculateServer.exe");
            if (File.Exists(exePath) == false)
                throw new ArgumentNullException("指定的文件不存在");
            Process process = new Process();
            ProcessStartInfo psInfo = new ProcessStartInfo(exePath);
            psInfo.WindowStyle = ProcessWindowStyle.Hidden;
            psInfo.Arguments = methodName;
            process.StartInfo = psInfo;
            process.Start();
            _processes.Add(process);
            // process.WaitForExit();
            // return process.ExitCode == 0;
        }

        /// <summary>
        /// 杀死所有计算的后台进程
        /// </summary>
        public static void KillAll()
        {
            if (_processes.Count > 0)
                foreach (Process prs in _processes)
                {
                    prs.Kill();
                }
        }

        /// <summary>
        /// 寻找Figure窗口，并且加入委托事件
        /// </summary>
        /// <param name="todo"></param>
        public static void FindFigureAndTodo(Action<IntPtr> todo)
        {
            Thread thread = new Thread(new ParameterizedThreadStart(delegate
            {
                //初始化曲线图
                IntPtr hwndControl = IntPtr.Zero;
                while (true)
                {
                    hwndControl = FindWindow("SunAwtFrame", "Figure 1");
                    if (hwndControl != IntPtr.Zero)
                    {
                        //找到之后延时100ms，以防后台和前台资源占用.
                        Thread.Sleep(100);
                        todo(hwndControl);
                        break;
                    }
                }

            }));
            thread.IsBackground = true;
            thread.Start();
        }
    }
}
