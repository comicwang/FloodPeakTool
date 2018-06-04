using System;
using System.Windows.Forms;
using iTelluro.GlobeEngine.MapControl3D;
using System.IO;
using iTelluro.Explorer.Location;
using iTelluro.Explorer.Controls;
using Microsoft.Practices.Unity;

using iTelluro.Explorer.Infrastructure.CrossCutting.SSO;
using iTelluro.Explorer.Infrastructure.CrossCutting.Caching;


namespace FloodPeakTool
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(args));
        }
    }
}
