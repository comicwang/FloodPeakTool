using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using Padding = System.Windows.Forms.Padding;

namespace FloodPeakUtility.UI
{
    public partial class RainCaculateControl2 : UserControl
    {
        private DevComponents.DotNetBar.TabControl _tabControl = null;

        public RainCaculateControl2(DevComponents.DotNetBar.TabControl tabControl)
        {
            InitializeComponent();
            _tabControl = tabControl;
            ChangeFormula();
        }

        /// <summary>
        /// 显示框
        /// </summary>
        /// <param name="title"></param>
        public void ShowDock(string title)
        {
            if (_tabControl == null)
                return;
            if (!this.ContainsTab(title))
            {
                TabItem tabPage = new TabItem();
                tabPage.Text = title;
                tabPage.Name = title;
                tabPage.CloseButtonVisible = true;
                this.Dock = DockStyle.Fill;
                tabPage.AttachedControl = this;
                _tabControl.Tabs.Add(tabPage);
            }
            _tabControl.SelectedTab = _tabControl.Tabs[title];
        }

        private bool ContainsTab(string tabName)
        {
            foreach (TabItem item in _tabControl.Tabs)
            {
                if (item.Name == tabName)
                    return true;
            }
            return false;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            panel1.Location = new Point((this.Width - panel1.Width) / 2, 0);

            panel1.Height = this.Height;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            ChangeFormula();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            ChangeFormula();
        }

        /// <summary>
        /// 改变公式
        /// </summary>

        private void ChangeFormula()
        {
            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.RowStyles.Clear();
            tableLayoutPanel1.ColumnStyles.Clear();
         
            tableLayoutPanel1.ColumnCount = int.Parse(numericUpDown2.Value.ToString());
            tableLayoutPanel1.RowCount = 2;
            DynamicLayout(tableLayoutPanel1, 2, tableLayoutPanel1.ColumnCount);
            double num = double.Parse(numericUpDown1.Value.ToString());

            for (int column = 0; column < tableLayoutPanel1.ColumnCount; column++)
            {
                for (int row = 0; row < 2; row++)
                {
                    Label lbl = new Label();

                    lbl.Dock = DockStyle.Fill;
                    if (row == 0)
                    {
                        lbl.Text = $"第{column}天";
                        lbl.Font = new Font("微软雅黑", 9f, FontStyle.Bold);
                    }
                    else
                    {
                        lbl.Text = $"P{column}*{Math.Round(Math.Pow(num, column), 2)}";
                        lbl.Font = new Font("微软雅黑", 9f);
                        lbl.BackColor = Color.White;
                    }
                    lbl.Margin = new Padding(0);
                    lbl.TextAlign = ContentAlignment.MiddleCenter;
                    lbl.AutoSize = false;
                    tableLayoutPanel1.Controls.Add(lbl, column, row);

                }
            }
        }

        /// <summary>
        /// 动态布局
        /// </summary>
        /// <param name="layoutPanel">布局面板</param>
        /// <param name="row">行</param>
        /// <param name="col">列</param>
        private void DynamicLayout(TableLayoutPanel layoutPanel, int row, int col)
        {
            for (int i = 0; i < row; i++)
            {
                layoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 1f/row));
            }
            for (int i = 0; i < col; i++)
            {
                layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1f/col));
            }
        }

        //开始计算
        private void btnCalculate_Click(object sender, EventArgs e)
        {
            btnCalculate.Enabled = false;
            if(string.IsNullOrEmpty(textBox1.Text))
            {
                MsgBox.ShowInfo("请输入站号信息");
                return;
            }

            if (!backgroundWorker1.IsBusy)
            {
                CaculateInfo info = new CaculateInfo()
                {
                    a0 = double.Parse(numericUpDown1.Value.ToString()),
                    N = int.Parse(numericUpDown2.Value.ToString()),
                    CaculateDate = dateTimePicker1.Value,
                    StateNo = textBox1.Text
                };
                Label label6 = new Label();
                label6.Height = 60;
                label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                label6.Dock = System.Windows.Forms.DockStyle.Top;
                label6.Font = new System.Drawing.Font("楷体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                label6.BackColor = Color.White;
                label6.Location = new System.Drawing.Point(0, 0);
                label6.TabIndex = 0;
                label6.Text = $"（{DateTime.Now}）:";
                label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                this.panel2.Controls.Add(label6);
                info.lbl = label6;
                backgroundWorker1.RunWorkerAsync(info);
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            CaculateInfo info = e.Argument as CaculateInfo;

            double result = 0;
            for (int i = 0; i < info.N; i++)
            {
                double rain = 0;
                //计算雨量值和系数值
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetConnection(), CommandType.Text, $"SELECT RAINFALL_1_DAY FROM RAINFALL_DAY where time = '{info.CaculateDate.AddDays(i * (-1)).ToString("yyyy-MM-dd")}' and monitornum = '{info.StateNo}'");
                if (ds.Tables[0].Rows.Count != 0)
                {
                    rain = double.Parse(ds.Tables[0].Rows[0][0].ToString());
                }

                //系数
                double num = Math.Round(Math.Pow(info.a0, i), 2);

                result += num * rain;

                CaculateInfo temp = null;
                if (i != info.N - 1)
                    temp = new CaculateInfo() { StateNo = $"{ num}*{rain}", lbl = info.lbl };
                else
                    temp = new CaculateInfo() { StateNo = $"{ num}*{rain}", lbl = info.lbl, result = result.ToString() };
                backgroundWorker1.ReportProgress((i + 1) * 100 / info.N, temp);
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            CaculateInfo info = e.UserState as CaculateInfo;
            if (e.ProgressPercentage < 100)
            {
                info.lbl.Text = info.lbl.Text + $"{info.StateNo}+";
            }
            else
            {
                info.lbl.Text = info.lbl.Text + $"{info.StateNo} = {info.result}";
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnCalculate.Enabled = true;
        }
    }

    /// <summary>
    /// 参与计算的基本参数
    /// </summary>
    public class CaculateInfo
    {
        /// <summary>
        /// 计算初始系数
        /// </summary>
        public double a0 { get; set; }

        /// <summary>
        /// 计算天数
        /// </summary>
        public int N { get; set; }

        /// <summary>
        /// 计算站号
        /// </summary>
        public string StateNo { get; set; }

        /// <summary>
        /// 计算日期
        /// </summary>
        public DateTime CaculateDate { get; set; }

        /// <summary>
        /// 计算所用的控件
        /// </summary>
        public Label lbl { get; set; }

        /// <summary>
        /// 结果
        /// </summary>
        public string result { get; set; }
    }
}
    