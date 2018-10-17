using FloodPeakUtility;
using FloodPeakUtility.Model;
/********************************************************************************
    ** auth： 王冲
    ** date： 2017/10/16 14:24:48
    ** desc： 尚未编写描述
    ** Ver.:  V1.0.0
*********************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FloodPeakToolUI.UI
{
    public partial class FormPercentExport : Form
    {
        public FormPercentExport()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 初始化Box
        /// </summary>
        private void InitializeCombox()
        {
            List<SimpleModel> dataSource = new List<SimpleModel>()
            {
                 new SimpleModel(){ Display="全部",Value="0"},
                new SimpleModel(){ Display="100年一遇",Value="1"},
                new SimpleModel(){Display="50年一遇",Value="2"},
                new SimpleModel(){Display="20年一遇" ,Value="5"},
                new SimpleModel(){Display="10年一遇" ,Value="10"},
                new SimpleModel(){Display="5年一遇" ,Value="20"},
            };

            cmbLevel.DataSource = dataSource;
            cmbLevel.DisplayMember = "Display";
            cmbLevel.ValueMember = "Value";
            cmbLevel.SelectedIndex = 0;
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="where"></param>
        private void Query(string where="")
        {
            string commandText = "select MONITORNUM as '站号',During as '时间间隔（时）',[PERCENT] as '概率（%）',[VALUE] as '概率值（0.1mm）',LON as '高程',LAT as '经度',LON as '纬度', HCP as '拟合度',CV as '离差系数',CS as '偏态系数' from rainfall_percent";
            if (!string.IsNullOrEmpty(where))
                commandText += where;
            try
            {
                //执行查询
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetConnSting(), CommandType.Text, commandText);

                dataGridView1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowError("查询异常：" + ex.Message);
            }
        }

        private void btnskin_Click(object sender, EventArgs e)
        {
            List<string> condition = new List<string>();
            //筛选概率
            if (cmbLevel.SelectedIndex != 0)
                condition.Add(string.Format("[PERCENT]='{0}'", cmbLevel.SelectedValue));
            //筛选站号
            if (!string.IsNullOrWhiteSpace(txtState.Text))
            {
                string state = string.Empty;
                string[] states = txtState.Text.Split(',');
                Array.ForEach(states, t =>
                {
                    state += "'";
                    state += t;
                    state += "',";
                });
                condition.Add(string.Format("MONITORNUM in ({0})", state.Substring(0, state.Length - 1)));
            }
            //筛选时间段
            if (!string.IsNullOrWhiteSpace(textBox1.Text))
            {
                string during = string.Empty;
                string[] durings = textBox1.Text.Split(',');
                Array.ForEach(durings, t =>
                {
                    during += t;
                    during += ",";
                });
                condition.Add(string.Format("During in ({0})", during.Substring(0, during.Length - 1)));
            }
            if (condition.Count > 0)
            {
                string where = string.Empty;
                for (int i = 0; i < condition.Count; i++)
                {
                    if (i == 0)
                        where += " where ";
                    else
                        where += " and ";
                    where += condition[i];
                }
                Query(where);
            }
            else
            {
                Query();
            }
        }

        private void FormPercentExport_Load(object sender, EventArgs e)
        {
            InitializeCombox();
            Query();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Excel文件|*.xls";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                XmlHelper.SaveDataToExcelFile(dataGridView1.DataSource as DataTable, dialog.FileName);
                System.Diagnostics.Process.Start("Explorer.exe", Path.GetDirectoryName(dialog.FileName));
            }
        }
    }
}
