using FloodPeakUtility;
using FuncforHFLL;
using MathWorks.MATLAB.NET.Arrays;
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
                string methodName = args[0];//方法名称

                //水文曲线
                if (methodName == MethodName.SWCure)
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

                    }, Path.Combine(Application.StartupPath, ConfigNames.SvCure));
                    Console.ReadKey();  //不直接关闭
                }

                //拟合曲线
                else if (methodName == MethodName.NiHeCure)
                { 
                    double X = Convert.ToDouble(args[1]);
                    MWNumericArray XX = new MWNumericArray(X);
                    double Cv = Convert.ToDouble(args[2]);
                    MWNumericArray Cvv = new MWNumericArray(Cv);
                    double Cs = Convert.ToDouble(args[3]);
                    MWNumericArray Css = new MWNumericArray(Cs);
                    Class1 CC = new Class1();
                    double[,] Nihe = (double[,])CC.peixian(Cvv, Css, XX).ToArray();
                    Console.WriteLine("Nihe:" + Nihe[0, 0]);
                    XmlHelper.Serialize<string>(Nihe[0, 0].ToString(), Path.Combine(Application.StartupPath, ConfigNames.TempName));
                    Console.ReadKey();
                }
                //曲线反查
                else if (methodName == MethodName.ResearchCure)
                {
                    double X = Convert.ToDouble(args[1]);
                    MWNumericArray XX = new MWNumericArray(X);
                    double Cv = Convert.ToDouble(args[2]);
                    MWNumericArray Cvv = new MWNumericArray(Cv);
                    double Cs = Convert.ToDouble(args[3]);
                    MWNumericArray Css = new MWNumericArray(Cs);
                    //反查类型 c1-根据概率查值，c2-根据值查询概率，c3-查询时间段内所有内定概率
                    string type = args[4].Split('-')[0];
                    if(type=="c1")
                    {
                        double k = Convert.ToDouble(args[4].Split('-')[1]);
                        MWNumericArray kik = new MWNumericArray(k);
                        Class1 CC = new Class1();
                        double[,] Xcha = (double[,])CC.chaxun1(Cvv, Css, XX, kik).ToArray();
                        Console.WriteLine(Xcha[0, 0]);
                    }
                    else if (type == "c2")
                    {
                        double k = Convert.ToDouble(args[4].Split('-')[1]);
                        MWNumericArray kik = new MWNumericArray(k);
                        Class1 CC = new Class1();
                        double[,] Xcha = (double[,])CC.chaxun2(Cvv, Css, XX, kik).ToArray();
                        Console.WriteLine(Xcha[0, 0]);
                    }
                    else if(type=="c3")
                    {

                    }
                }
            }            
        }
    }
}
