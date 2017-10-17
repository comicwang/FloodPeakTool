﻿using FloodPeakUtility;
using FloodPeakUtility.Model;
using FuncforHFLL;
using MathWorks.MATLAB.NET.Arrays;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CaculateServer
{
    /// <summary>
    /// 洪峰流量计算后台方法
    /// （解决版本x86不兼容的问题）
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            if (args != null && args.Length > 0)
            {
                string methodName = args[0];//方法名称
                Class1 C = null;

                #region 水文曲线
                if (methodName == MethodName.SWCure)
                {
                    try
                    {
                        C = new Class1();
                        //删除文件
                        string filePath = Path.Combine(Application.StartupPath, ConfigNames.SvCure);
                        if (File.Exists(filePath))
                            File.Delete(filePath);
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

                        }, filePath);
                        Console.ReadKey();  //不直接关闭
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        if (C != null)
                            C.Dispose();
                    }
                }
                #endregion

                #region 拟合曲线

                else if (methodName == MethodName.NiHeCure)
                {
                    try
                    {
                        double X = Convert.ToDouble(args[1]);
                        MWNumericArray XX = new MWNumericArray(X);
                        double Cv = Convert.ToDouble(args[2]);
                        MWNumericArray Cvv = new MWNumericArray(Cv);
                        double Cs = Convert.ToDouble(args[3]);
                        MWNumericArray Css = new MWNumericArray(Cs);
                        C = new Class1();
                        double[,] Nihe = (double[,])C.peixian(Cvv, Css, XX).ToArray();
                        Console.WriteLine("Nihe:" + Nihe[0, 0]);
                        XmlHelper.Serialize<string>(Nihe[0, 0].ToString(), Path.Combine(Application.StartupPath, ConfigNames.TempName));
                        C.Dispose();
                        Console.ReadKey();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        if (C != null)
                            C.Dispose();
                    }
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
                        C = new Class1();
                        double[,] Xcha = (double[,])C.chaxun1(Cvv, Css, XX, kik).ToArray();
                        Console.WriteLine(Xcha[0, 0]);
                    }
                    else if (type == "c2")
                    {
                        double k = Convert.ToDouble(args[4].Split('-')[1]);
                        MWNumericArray kik = new MWNumericArray(k);
                        C = new Class1();
                        double[,] Xcha = (double[,])C.chaxun2(Cvv, Css, XX, kik).ToArray();
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
                        C = new Class1();
                        foreach (double item in CollectionCons.StaticsPercents)
                        {
                            try
                            {
                                MWNumericArray kik = new MWNumericArray(item);
                                double[,] Xcha = (double[,])C.chaxun1(Cvv, Css, XX, kik).ToArray();
                                value = Xcha[0, 0];
                                //先查询数据是否存在，存在更新，不存在新增
                                commandText = string.Format("SELECT * FROM [DB_RainMonitor].[dbo].[RAINFALL_PERCENT] where [PERCENT]={0} and During={1} and MONITORNUM='{2}'", item, during, state);
                                object obj = SqlHelper.ExecuteScalar(SqlHelper.GetConnSting(), System.Data.CommandType.Text, commandText);
                                if (obj == null)
                                    commandText = string.Format("insert into RAINFALL_PERCENT values(NEWID(),'{0}',{1},{2},{3},{4},{5},{6},{7},'{8}',{9})", state, "null", "null", "null", X, Cv, Cs, item, during, value);
                                else
                                    commandText = string.Format("update RAINFALL_PERCENT set [VALUE] = {0} where [PERCENT]={1} and During={2} and MONITORNUM='{3}'", value, item, during, state);
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
                        C = new Class1();
                        SubCure sub = new SubCure();
                        //删除文件
                        string filePath = Path.Combine(Application.StartupPath, ConfigNames.SubCure);
                        if (File.Exists(filePath))
                            File.Delete(filePath);
                        MWArray polyData3 = C.polyfit_line(MatX1, MatY1);
                        MWArray polyData1 = C.polyfit_line(MatX1, MatY1);
                        MWArray polyData2 = C.polyfit_line(MatX2, MatY2);
                        double[,] DataBox1 = (double[,])polyData1.ToArray();
                        double[,] DataBox2 = (double[,])polyData2.ToArray();
                        double d = (Math.Abs(DataBox2[0, 0]) / Math.Abs(DataBox1[0, 0]) - 1) * 0.3;
                        sub.d = d;
                        double k1 = Math.Log10(10 + d);
                        double k2 = Math.Log10(0.1 + d);
                        double nd = Math.Abs((DataBox1[0, 0] + DataBox2[0, 0]) / (k1 - k2));
                        double Sd = (Math.Pow((10 + d), nd) / Math.Pow(10, Math.Abs(DataBox2[0, 0]))) * Math.Pow(10, DataBox2[0, 1]);
                        sub.nd = nd;
                        sub.Sd = Sd;
                        sub.n1 = DataBox1[0, 0];
                        sub.j1 = DataBox1[0, 1];
                        sub.n2 = DataBox2[0, 0];
                        sub.j2 = DataBox2[0, 1];
                        XmlHelper.Serialize<SubCure>(sub, filePath);
                        Console.ReadKey();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        if (C != null)
                            C.Dispose();
                    }
                }

                #endregion

                #region d=0

                else if (methodName == MethodName.RainStormSub0)
                {
                    //参数值 1-小于一小时的时间段，2-小于一小时的值，3-大于一小时的时间段，4-大于一小时的值

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
                    try
                    {
                        C = new Class1();
                        //删除文件
                        string filePath = Path.Combine(Application.StartupPath, ConfigNames.SubCure0);
                        if (File.Exists(filePath))
                            File.Delete(filePath);
                        MWArray polyData3 = C.polyfit_line(MatX1, MatY1);
                        MWArray polyData1 = C.polyfit_line(MatX1, MatY1);
                        double[,] DataBox1 = (double[,])polyData1.ToArray();
                        SubCure sub = new SubCure();
                        double nd = Math.Abs(2 * (DataBox1[0, 0]) / (Math.Log(10) - Math.Log(0.1)));
                        double Sd = (Math.Pow(10, nd) / Math.Pow(10, Math.Abs(DataBox1[0, 0]))) * Math.Pow(10, DataBox1[0, 1]);
                        sub.nd = nd;
                        sub.Sd = Sd;
                        sub.n1 = DataBox1[0, 0];
                        sub.j1 = DataBox1[0, 1];
                        XmlHelper.Serialize<SubCure>(sub, filePath);
                        Console.ReadKey();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        if (C != null)
                            C.Dispose();
                    }
                }

                #endregion

                #region 洪峰流量

                else if (methodName == MethodName.FloodPeak)
                {
                    //p1,Qm,eps1,sd,R,d,nd,r1,F,L1,L2,I1,I2,A1,A2,tc,eps2,项目路径

                    MWNumericArray p1_0 = new MWNumericArray(Convert.ToDouble(args[1]));
                    //MWArray p1_0 = MWArray(p);
                    MWNumericArray Qm_0 = new MWNumericArray(Convert.ToDouble(args[2]));
                    //MWArray Qm_0 = MWArray(Q);
                    MWNumericArray eps = new MWNumericArray(Convert.ToDouble(args[3]));
                    //MWArray eps = MWArray(ee);
                    MWNumericArray sd = new MWNumericArray(Convert.ToDouble(args[4]));
                    //MWArray sd = MWArray(s);
                    MWNumericArray R = new MWNumericArray(Convert.ToDouble(args[5]));
                    //MWArray R = MWArray(RR);
                    MWNumericArray d = new MWNumericArray(Convert.ToDouble(args[6]));
                    //MWArray d = MWArray(dd);
                    MWNumericArray nd = new MWNumericArray(Convert.ToDouble(args[7]));
                    //MWArray nd = MWArray(ndd);
                    MWNumericArray r1 = new MWNumericArray(Convert.ToDouble(args[8]));
                    //MWArray r1 = MWArray(rr1);
                    MWNumericArray F = new MWNumericArray(Convert.ToDouble(args[9]));
                    //MWArray F = MWArray(FF);
                    MWNumericArray L1 = new MWNumericArray(Convert.ToDouble(args[10]));
                    //MWArray L1 = MWArray(LL1);
                    MWNumericArray L2 = new MWNumericArray(Convert.ToDouble(args[11]));
                    //MWArray L2 = MWArray(LL2);
                    MWNumericArray I1 = new MWNumericArray(Convert.ToDouble(args[12]));
                    //MWArray I1 = MWArray(II1);
                    MWNumericArray I2 = new MWNumericArray(Convert.ToDouble(args[13]));
                    //MWArray I2 = MWArray(II2);
                    MWNumericArray A1 = new MWNumericArray(Convert.ToDouble(args[14]));
                    //MWArray A1 = MWArray(AA1);
                    MWNumericArray A2 = new MWNumericArray(Convert.ToDouble(args[15]));
                    //MWArray A2 = MWArray(AA2);
                    MWNumericArray tc = new MWNumericArray(Convert.ToDouble(args[16]));
                    MWNumericArray eps1 = new MWNumericArray(Convert.ToDouble(args[17]));
                    try
                    {
                        C = new Class1();
                        MainResult result = new MainResult();
                        //删除文件
                        if (File.Exists(args[18]))
                            File.Delete(args[18]);
                        MWArray A = C.fun_main(p1_0, Qm_0, eps, sd, R, d, nd, r1, F, L1, L2, I1, I2, A1, A2);
                        double[,] AA = (double[,])A.ToArray();
                        result.Qm = AA[0, 0];
                        result.p1 = AA[0, 1];
                        result.tQ = AA[0, 2];
                        result.d1 = AA[0, 3];

                        MWArray B = C.func_getTc(sd, R, d, nd, r1, AA[0, 0], AA[0, 1], AA[0, 2], F, tc, eps1);
                        double[,] BB = (double[,])B.ToArray();
                        result.t = BB[0, 0];
                        result.a1tc = BB[0, 1];
                        result.d2 = BB[0, 5];
                        XmlHelper.Serialize<MainResult>(result, args[18]);
                        Console.ReadKey();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        if (C != null)
                            C.Dispose();
                    }
                }

                #endregion

                #region 导出Sd-Qm查询表

                else if (methodName == MethodName.SdQmTable)
                {
                    // 项目路径-保存表路径
                    string projectForlder = args[1];
                    string xmlPath = Path.Combine(projectForlder, ConfigNames.RainStormSub);
                    //暴雨衰减赋值
                    BYSJResult bysj = null;
                    if (File.Exists(xmlPath))
                    {
                        bysj = XmlHelper.Deserialize<BYSJResult>(xmlPath);
                    }
                    //暴雨损失赋值
                    BYSSResult byss = null;
                    xmlPath = Path.Combine(projectForlder, ConfigNames.RainStormLoss);
                    if (File.Exists(xmlPath))
                    {
                        byss = XmlHelper.Deserialize<BYSSResult>(xmlPath);
                    }
                    //河槽汇流赋值
                    HCHLResult hchl = null;
                    xmlPath = Path.Combine(projectForlder, ConfigNames.RiverConfluence);
                    if (File.Exists(xmlPath))
                    {
                        hchl = XmlHelper.Deserialize<HCHLResult>(xmlPath);
                    }
                    //坡面汇流赋值
                    xmlPath = Path.Combine(projectForlder, ConfigNames.SlopeConfluence);
                    PMHLResult pmhl = null;
                    if (File.Exists(xmlPath))
                    {
                        pmhl = XmlHelper.Deserialize<PMHLResult>(xmlPath);
                    }
                    //默认参数
                    xmlPath = Path.Combine(projectForlder, ConfigNames.DefaultArgModel);
                    DefaultArgModel defaultArg = null;
                    if (File.Exists(xmlPath))
                    {
                        defaultArg = XmlHelper.Deserialize<DefaultArgModel>(xmlPath);
                    }
                    if (bysj == null || byss == null || hchl == null || pmhl == null || defaultArg == null)
                    {
                        Console.WriteLine("参数不全！");
                        return;
                    }
                    double p1 = Convert.ToDouble(defaultArg.p1);
                    double Qm_v = Convert.ToDouble(defaultArg.Qm);
                    double eps_0 = Convert.ToDouble(defaultArg.esp1);
                    MWNumericArray p1_0 = new MWNumericArray(p1);
                    MWNumericArray Qm_0 = new MWNumericArray(Qm_v); ;
                    MWNumericArray eps = new MWNumericArray(eps_0);
                    MWNumericArray R = new MWNumericArray(byss.R);
                    MWNumericArray d = new MWNumericArray(bysj.d);
                    MWNumericArray nd = new MWNumericArray(bysj.nd);
                    MWNumericArray r1 = new MWNumericArray(byss.r1);
                    MWNumericArray F = new MWNumericArray(byss.F);
                    MWNumericArray L1 = new MWNumericArray(hchl.L1);
                    MWNumericArray L2 = new MWNumericArray(pmhl.L2);
                    MWNumericArray I1 = new MWNumericArray(hchl.l1);
                    MWNumericArray I2 = new MWNumericArray(pmhl.l2);
                    MWNumericArray A1 = new MWNumericArray(hchl.A1);
                    MWNumericArray A2 = new MWNumericArray(pmhl.A2);

                    DataTable aTable = new DataTable();
                    DataColumn Sd = new DataColumn("Sd", typeof(double));
                    DataColumn Qm = new DataColumn("Qm", typeof(double));
                    aTable.Columns.Add(Sd);
                    aTable.Columns.Add(Qm);
                    try
                    {
                        C = new Class1();
                        double sd_0 = 10;
                        for (int i = 0; i < 1001; i++)
                        {
                            MWNumericArray sd = new MWNumericArray(sd_0);
                            MWArray A = C.fun_main(p1_0, Qm_0, eps, sd, R, d, nd, r1, F, L1, L2, I1, I2, A1, A2);
                            double[,] AA = (double[,])A.ToArray();
                            DataRow row = aTable.NewRow();
                            row["Sd"] = sd_0;
                            row["Qm"] = AA[0, 0];
                            aTable.Rows.Add(row);
                            sd_0 = sd_0 + 0.1;
                        }
                        XmlHelper.SaveDataToExcelFile(aTable, args[2]);
                        Console.WriteLine("导出完成！");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("导出异常：" + ex.Message);
                    }
                    finally
                    {
                        if (C != null)
                            C.Dispose();
                    }
                }

                #endregion
            }         
        }
    }
}
