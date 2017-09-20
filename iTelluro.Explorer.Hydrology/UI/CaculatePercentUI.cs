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

namespace FloodPeakToolUI.UI
{
    public partial class CaculatePercentUI : UserControl
    {
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
        public void BindResult(double[,] args)
        {
            //初始化参数
            if (args != null && args.Length > 4)
            {
                numX.Value = Convert.ToDecimal(args[0, 2]);
                numCv.Value = Convert.ToDecimal(args[0, 0]);
                numCs.Value = Convert.ToDecimal(args[0, 1]);
                txtNihe.Text = args[0, 3].ToString();
            }

            //初始化曲线图
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
