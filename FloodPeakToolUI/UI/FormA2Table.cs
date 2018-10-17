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
    public partial class FormA12Table : Form
    {
        public FormA12Table()
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
                { "类别","地表特征","变化范围","一般情况" },
                {
                   "路面","平整夯实的土、石质路面","0.05-0.08",0.07
                },
                {
                   "光坡","无草的土、石质地面；水土流失严重造成许多冲沟的坡地","0.035-0.05",0.045
                },
                {
                    "疏草地","种有旱作物、植被较差的坡地；稀疏草地；戈壁滩。对于坡面平顺，植被较差，水土流失明显的坡地；卵石较少的戈壁滩，取较大值。对土层薄有大片基岩外露，植被覆盖差、有些小坑洼的坡面取较小值。","0.02-0.035",0.025
                },
                {
                   "荒草坡、疏林地、梯田","覆盖度在50%左右的中等密草地；郁闭度为30%左右的稀疏林地。对无树木的北方旱作物坡耕地区较大值。对疏林内有中密草丛、带田埂的梯地或水田者取较小值。","0.01-0.02",0.015
                },
                {
                   "一般树林及平坦区水田","树林郁闭度占50%左右。林下有中密草丛；灌木丛生较密的草丛；地形较平坦、治理较好的大片水田流域。对中等密度的幼林；丘陵梯（水）田取较大值。对郁闭度50%以上的成林；地形平坦、简易蓄水工程（如冬水田、小塘、堰等）较多的大片水田地区取较小值。","0.005-0.01",0.007
                },
                {
                   "森林密草","森林郁闭度70%以上，林下并有草被或落叶层；茂密的草灌丛林。对原始森林及林下有大量枯枝落叶层者取最小值。","0.003-0.005",0.004
                }
            };

            for (int x = 0; x < tableData.GetLength(0); x++)
            {
                for (int y = 0; y < tableData.GetLength(1); y++)
                {
                    AddLabelText(y, x, tableData[x,y].ToString());
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
            if (y > 0 && x == 3)
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
