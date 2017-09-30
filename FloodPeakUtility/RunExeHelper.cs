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
        private static List<Process> _lstPrc = new List<Process>();
        private static Dictionary<IntPtr, Process> _dicProcPtr = new Dictionary<IntPtr, Process>();

        //记录当前进程，用于标识进程和Figure的关系
        private static Process _currentProcess = null;


        /// <summary>
        /// 运行一个计算的后台
        /// </summary>
        /// <param name="argsment">需要计算的后台名称</param>
        public static void RunMethod(string argsment)
        {
            string exePath = Path.Combine(Application.StartupPath, "CaculateServer.exe");
            if (File.Exists(exePath) == false)
                throw new ArgumentNullException("指定的文件不存在");
            Process process = new Process();
            ProcessStartInfo psInfo = new ProcessStartInfo(exePath);
            psInfo.WindowStyle = ProcessWindowStyle.Hidden;
            psInfo.Arguments = argsment;
            process.StartInfo = psInfo;
            process.Start();
            _lstPrc.Add(process);
        }

        public static string RunMethodExit(string argsment)
        {
            string exePath = Path.Combine(Application.StartupPath, "CaculateServer.exe");
            if (File.Exists(exePath) == false)
                throw new ArgumentNullException("指定的文件不存在");
            Process process = new Process();
            ProcessStartInfo psInfo = new ProcessStartInfo(exePath);
            psInfo.UseShellExecute = false;
            psInfo.RedirectStandardOutput = true;
            psInfo.CreateNoWindow = true;
            psInfo.WindowStyle = ProcessWindowStyle.Hidden;
            psInfo.Arguments = argsment;
            process.StartInfo = psInfo;
            process.Start();
            return process.StandardOutput.ReadToEnd();
        }

        /// <summary>
        /// 杀死所有计算的后台进程
        /// </summary>
        public static void KillAll()
        {
            if (_lstPrc.Count > 0)
                foreach (var prs in _lstPrc)
                {
                    if (prs != null)
                        prs.Kill();
                }
        }

        /// <summary>
        /// 根据句柄来关闭进程
        /// </summary>
        /// <param name="ptr">句柄值</param>
        public static void KillByIntPtr(IntPtr ptr)
        {
            if (_dicProcPtr.Count > 0)
                foreach (var prs in _dicProcPtr)
                {
                    if (prs.Key == ptr)
                    {
                        if (prs.Value != null)
                            prs.Value.Kill();
                        _lstPrc.Remove(prs.Value);
                        break;
                    }
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
                        if (!_dicProcPtr.ContainsKey(hwndControl))
                            _dicProcPtr.Add(hwndControl, _currentProcess);
                        break;
                    }
                }

            }));
            thread.IsBackground = true;
            thread.Start();
        }
    }
}
