using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FloodPeakToolUI.UI
{
    public partial class FormA1Table : Form
    {
        public FormA1Table()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 获取选中的参数值
        /// </summary>
        public string SelectedValue { get; private set; }

        private void FormA1Table_Load(object sender, EventArgs e)
        {
            object[,] tableData = new object[,]
            {
                { "     a\n   A1\nml\n",1,2,3,4,5,7,10,15,20,30,50,"主沟道形态特征" },
                {
                    5,0.095,0.084,0.077,0.071,0.068,0.062,0.057,0.050,0.047,0.041,0.036,"丛林郁闭度占75%以上的河沟；有大量漂石堵塞的山区型弯曲大的河床；草丛密生的河滩。"
                },
                {
                    7,0.120,0.106,0.098,0.092,0.087,0.079,0.072,0.064,0.060,0.053,0.046,"丛林郁闭度占60%以上的河沟；有较多漂石堵塞的山区型弯曲河床；有杂草、死水的沼泽型河沟；平坦地区的梯田漫滩地。"
                },
                {
                    10,0.154,0.137,0.126,0.117,0.111,0.102,0.092,0.083,0.076,0.068,0.059,"植物覆盖度50%以上，有漂石堵塞的河床；河床弯曲有漂石及跌水的山区型沟道；山丘区的冲田滩地。"
                },
                {
                    15,0.21,0.18,0.17,0.15,0.15,0.14,0.12,0.11,0.10,0.09,0.08,"植被覆盖度占50%以下有少量堵塞物的河床。"
                },
                {
                    20,0.25,0.22,0.20,0.19,0.18,0.16,0.15,0.13,0.12,0.11,0.10,"弯曲或生长杂草的河床。"
                },
                {
                    25,0.29,0.26,0.24,0.22,0.21,0.19,0.18,0.16,0.15,0.13,0.11,"杂草稀疏，较为平坦、顺直的河床。"
                },
                {
                    30,0.34,0.30,0.27,0.25,0.24,0.22,0.20,0.18,0.17,0.15,0.13,"平坦通畅顺直的河床。"
                }
            };

            for (int x = 0; x < tableData.GetLength(0); x++)
            {
                for (int y = 0; y < tableData.GetLength(1); y++)
                {
                    AddLabelText(y, x, tableData[x, y].ToString());
                }
            }
        }

        /// <summary>
        /// 在Table布局中加一个Label标签
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="text"></param>
        private void AddLabelText(int x, int y, string text)
        {
            Label lbl = new Label();
            lbl.Text = text;
            lbl.Dock = DockStyle.Fill;
            if (x == 0 || y == 0)
            {
                lbl.Font = new Font("微软雅黑", 9f, FontStyle.Bold);
            }
            else
            {
                lbl.Font = new Font("微软雅黑", 9f);
                lbl.BackColor = Color.White;
            }
            if (x > 0 && x < 12 && y > 0)
            {
                lbl.Click += Lbl_Click;
                toolTip1.SetToolTip(lbl, "单击选中参数");
                lbl.Cursor = Cursors.Hand;
            }
            lbl.Margin = new Padding(0);
            lbl.TextAlign = ContentAlignment.MiddleCenter;
            lbl.AutoSize = false;
            tableLayoutPanel1.Controls.Add(lbl, x, y);
        }

        private void Lbl_Click(object sender, EventArgs e)
        {
            Label lbl = sender as Label;
            SelectedValue = lbl.Text;
            DialogResult = DialogResult.OK;
        }
    }
}
