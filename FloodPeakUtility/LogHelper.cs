using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FloodPeakUtility
{
    /// <summary>
    /// 日志记录帮助类
    /// </summary>
    public class LogHelper
    {
        private static string _path = string.Empty;
        private static StreamWriter streamWriter; //写文件 
 
        /// <summary>
        /// 绑定日志信息
        /// </summary>
        /// <param name="path">日志文件路径</param>
        public static void BindLog(string path)
        {
            _path = path;
        }

        /// <summary>
        /// 记录日志信息
        /// </summary>
        /// <param name="log"></param>
        /// <param name="needTile"></param>
        public static void LogInfo(string log, bool needTile = false)
        {
            try
            {

                if (streamWriter == null)
                {
                    streamWriter = !File.Exists(_path) ? File.CreateText(_path) : File.AppendText(_path);    //判断文件是否存在如果不存在则创建，如果存在则添加。
                }
                if (needTile)
                {
                    streamWriter.WriteLine("***********************************************************************");
                    streamWriter.WriteLine(DateTime.Now.ToString());
                    streamWriter.WriteLine("输出信息：记录信息");
                }
                if (log != null)
                {
                    streamWriter.WriteLine(log);
                }
            }
            finally
            {
                if (streamWriter != null)
                {
                    streamWriter.Flush();
                    streamWriter.Dispose();
                    streamWriter = null;
                }
            }
        }

        /// <summary>
        /// 记录错误信息
        /// </summary>
        /// <param name="log"></param>
        public static void LogErro(string log)
        {
            try
            {

                if (streamWriter == null)
                {
                    streamWriter = !File.Exists(_path) ? File.CreateText(_path) : File.AppendText(_path);    //判断文件是否存在如果不存在则创建，如果存在则添加。
                }
                streamWriter.WriteLine("***********************************************************************");
                streamWriter.WriteLine(DateTime.Now.ToString());
                streamWriter.WriteLine("输出信息：错误信息");
                if (log != null)
                {
                    streamWriter.WriteLine("异常信息：\r\n" + log);
                }
            }
            finally
            {
                if (streamWriter != null)
                {
                    streamWriter.Flush();
                    streamWriter.Dispose();
                    streamWriter = null;
                }
            }
        }

    }
}
