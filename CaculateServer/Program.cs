using FloodPeakUtility;
using FuncforHFLL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CaculateServer
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args != null && args.Length > 0)
            {
                string methodName = args[0];
                if (methodName == "SWCure")
                {
                    Class1 C = new Class1();
                    double[,] CC = (double[,])C.miaodian().ToArray();
                    Console.WriteLine("Cv:" + CC[0, 0]);
                    Console.WriteLine("Cs:" + CC[0, 1]);
                    Console.WriteLine("X:" + CC[0, 2]);
                    Console.WriteLine("Nihe:" + CC[0, 3]);
                    XmlHelper.Serialize<CvCure>(new CvCure()
                    {
                        Cv = CC[0, 0],
                        Cs = CC[0, 1],
                        X = CC[0, 2],
                        Nihe = CC[0, 3].ToString()

                    }, Path.Combine(Application.StartupPath, "Cv.xml"));
                }
            }
            Console.ReadKey();
        }
    }
}
