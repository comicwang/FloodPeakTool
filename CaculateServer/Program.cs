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
        /// <summary>
        /// 水文曲线计算后台
        /// ars1-方法名称
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            if (args != null && args.Length > 0)
            {
                string methodName = args[0];//方法名称

                #region 水文曲线
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
                    C.Dispose();
                    Console.ReadKey();  //不直接关闭
                }
                #endregion

                #region 拟合曲线

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
                    CC.Dispose();
                    Console.ReadKey();
                }

                #endregion

                #region 曲线反查

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
                    if (type == "c1")
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
                    // c3-站号-时间段
                    else if (type == "c3")
                    {
                        string state = args[4].Split('-')[1];
                        string time = args[4].Split('-')[2];
                        //将时间段转换为小时
                        double during = 0;
                        bool success = CollectionCons.DicStrToHour.TryGetValue(time, out during);
                        if (success == false)
                        {
                            Console.WriteLine("参数错误");
                            return;
                        }
                        double value = 0;
                        string commandText = string.Empty;
                        Class1 CC = new Class1();
                        foreach (double item in CollectionCons.StaticsPercents)
                        {
                            try
                            {
                                MWNumericArray kik = new MWNumericArray(item);
                                double[,] Xcha = (double[,])CC.chaxun1(Cvv, Css, XX, kik).ToArray();
                                value = Xcha[0, 0];
                                commandText = string.Format("insert into RAINFALL_PERCENT values(NEWID(),'{0}',{1},{2},{3},{4},{5},{6},{7},'{8}',{9})", state, "null", "null", "null", X, Cv, Cs, item, during, value);
                                SqlHelper.ExecuteNonQuery(SqlHelper.GetConnSting(), System.Data.CommandType.Text, commandText);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("入库失败-" + ex.Message);
                                return;
                            }
                        }
                        Console.WriteLine("入库成功！");
                    }
                }

                #endregion

                #region 暴雨衰减

                else if (methodName == MethodName.RainStormSub)
                {
                    //参数值 1-小于一小时的时间段，2-小于一小时的值，3-大于一小时的时间段，4-大于一小时的值
                    Class1 C = new Class1();

                    List<double> list1 = new List<double>();
                    string[] minHour = args[2].Split(',');
                    Array.ForEach(minHour, t =>
                        {
                            list1.Add(Convert.ToDouble(t));
                        });
                    MWNumericArray MatY1 = new MWNumericArray(list1.ToArray());
                    List<double> list2 = new List<double>();
                    minHour = args[1].Split(',');
                    Array.ForEach(minHour, t =>
                    {
                        list2.Add(Convert.ToDouble(t));
                    });
                    MWNumericArray MatX1 = new MWNumericArray(list2.ToArray());
                    List<double> list3 = new List<double>();
                    minHour = args[4].Split(',');
                    Array.ForEach(minHour, t =>
                    {
                        list3.Add(Convert.ToDouble(t));
                    });
                    MWNumericArray MatY2 = new MWNumericArray(list3.ToArray());
                    List<double> list4 = new List<double>();
                    minHour = args[3].Split(',');
                    Array.ForEach(minHour, t =>
                    {
                        list4.Add(Convert.ToDouble(t));
                    });
                    MWNumericArray MatX2 = new MWNumericArray(list4.ToArray());
                    try
                    {
                        SubCure sub = new SubCure();
                        MWArray polyData3 = C.polyfit_line(MatX1, MatY1);
                        MWArray polyData1 = C.polyfit_line(MatX1, MatY1);
                        MWArray polyData2 = C.polyfit_line(MatX2, MatY2);
                        double[,] DataBox1 = (double[,])polyData1.ToArray();
                        double[,] DataBox2 = (double[,])polyData2.ToArray();
                        double d = (Math.Abs(DataBox2[0, 0]) / Math.Abs(DataBox1[0, 0]) - 1) * 0.3;
                        sub.d = d;
                        double k1 = Math.Log(10 + d);
                        double k2 = Math.Log(0.1 + d);
                        double nd = Math.Abs((DataBox1[0, 0] + DataBox2[0, 0]) / (k1 - k2));
                        double Sd = (Math.Pow((10 + d), nd) / Math.Pow(10, Math.Abs(DataBox2[0, 0]))) * Math.Pow(10, DataBox2[0, 1]);
                        sub.nd = nd;
                        sub.Sd = Sd;
                        sub.n1 = DataBox1[0, 0];
                        sub.j1 = DataBox1[0, 1];
                        sub.n2 = DataBox2[0, 0];
                        sub.j2 = DataBox2[0, 1];
                        XmlHelper.Serialize<SubCure>(sub, Path.Combine(Application.StartupPath, ConfigNames.SubCure));
                        C.Dispose();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        Console.ReadKey();
                    }
                }

                #endregion
            }            
        }
    }
}
