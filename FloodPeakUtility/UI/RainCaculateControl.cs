using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using System.IO;
using FloodPeakUtility.Model;

namespace FloodPeakUtility.UI
{
    public partial class RainCaculateControl : UserControl
    {
        private DevComponents.DotNetBar.TabControl _tabControl = null;

        int[] hourArry = new int[] { 1, 3, 6, 12, 24, 48, 72 };
        int[] dayArry = new int[] { 1, 3, 5, 7, 15, 30 };

        List<RainCaculateConditon> lstCondition = null;

        public RainCaculateControl(DevComponents.DotNetBar.TabControl tabControl)
        {
            InitializeComponent();
            _tabControl = tabControl;
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

        private void RainCaculateControl_Load(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(pictureBox1, "根据固定模板进行批量计算指定雨量统计值");
            //动态添加表列
            for (int i = 0; i < hourArry.Length; i++)
            {
                DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
                column.HeaderText = $"RAINFALL_{hourArry[i]}_HOUR";
                column.Name= $"RAINFALL_{hourArry[i]}_HOUR";
                column.ReadOnly = true;
                dataGridView1.Columns.Add(column);

                column = new DataGridViewTextBoxColumn();
                column.HeaderText = $"RAINFALL_{hourArry[i]}_HOUR_TIME";
                column.Name = $"RAINFALL_{hourArry[i]}_HOUR_TIME";
                column.ReadOnly = true;
                dataGridView1.Columns.Add(column);

                column = new DataGridViewTextBoxColumn();
                column.HeaderText = $"RAINFALL_{hourArry[i]}_HOUR_QC";
                column.Name = $"RAINFALL_{hourArry[i]}_HOUR_QC";
                column.ReadOnly = true;
                dataGridView1.Columns.Add(column);

                column = new DataGridViewTextBoxColumn();
                column.HeaderText = $"MAX_{hourArry[i]}_HOUR_MONTH";
                column.Name = $"MAX_{hourArry[i]}_HOUR_MONTH";
                column.ReadOnly = true;
                dataGridView1.Columns.Add(column);

                column = new DataGridViewTextBoxColumn();
                column.HeaderText = $"MAX_{hourArry[i]}_HOUR_MONTH_TIME";
                column.Name = $"MAX_{hourArry[i]}_HOUR_MONTH_TIME"; 
                column.ReadOnly = true;
                dataGridView1.Columns.Add(column);

                column = new DataGridViewTextBoxColumn();
                column.HeaderText = $"MAX_{hourArry[i]}_HOUR_MONTH_QC";
                column.Name = $"MAX_{hourArry[i]}_HOUR_MONTH_QC";
                column.ReadOnly = true;
                dataGridView1.Columns.Add(column); 
            }

            //动态添加表列
            for (int i = 0; i < dayArry.Length; i++)
            {
                DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
                column.HeaderText = $"RAINFALL_{dayArry[i]}_DAY";
                column.Name = $"RAINFALL_{dayArry[i]}_DAY";
                column.ReadOnly = true;
                dataGridView1.Columns.Add(column);

                column = new DataGridViewTextBoxColumn();
                column.HeaderText = $"RAINFALL_{dayArry[i]}_DAY_TIME";
                column.Name = $"RAINFALL_{dayArry[i]}_DAY_TIME";
                column.ReadOnly = true;
                dataGridView1.Columns.Add(column);

                column = new DataGridViewTextBoxColumn();
                column.HeaderText = $"RAINFALL_{dayArry[i]}_DAY_QC";
                column.Name = $"RAINFALL_{dayArry[i]}_DAY_QC";
                column.ReadOnly = true;
                dataGridView1.Columns.Add(column);

                column = new DataGridViewTextBoxColumn();
                column.HeaderText = $"MAX_{dayArry[i]}_DAY_MONTH";
                column.Name = $"MAX_{dayArry[i]}_DAY_MONTH";
                column.ReadOnly = true;
                dataGridView1.Columns.Add(column);

                column = new DataGridViewTextBoxColumn();
                column.HeaderText = $"MAX_{dayArry[i]}_DAY_MONTH_TIME";
                column.Name = $"MAX_{dayArry[i]}_DAY_MONTH_TIME";
                column.ReadOnly = true;
                dataGridView1.Columns.Add(column);

                column = new DataGridViewTextBoxColumn();
                column.HeaderText = $"MAX_{dayArry[i]}_DAY_MONTH_QC";
                column.Name = $"MAX_{dayArry[i]}_DAY_MONTH_QC";
                column.ReadOnly = true;
                dataGridView1.Columns.Add(column);
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            FormOutput.AppendProress(true);
            lstCondition = e.Argument as List<RainCaculateConditon>;
            //开始计算每个值
            if (lstCondition != null && lstCondition.Count > 0)
            {
                for (int i = 0; i < lstCondition.Count; i++)
                {
                    List<RainCaculateResult> tempResult = CaculateRain(lstCondition[i]);
                    backgroundWorker1.ReportProgress((int)((i + 1) * 100 / lstCondition.Count), tempResult);
                }
            }
        }

        private List<RainCaculateResult> CaculateRain(RainCaculateConditon condition)
        {
            List<RainCaculateResult> result = new List<RainCaculateResult>();
            DataSet ds = null;
            //1h,3h,6h,12h,24h,48h,72h 时间范围内最大值
            string commandText = string.Empty;
            for (int i = 0; i < hourArry.Length; i++)
            {
                commandText += string.Format(@"SELECT top 1 
                                   [RAINFALL _{3}_HOUR]
                                   ,TIME
                                   ,[RAINFALL _{3}_HOUR_C]
                                   FROM [DB_RainMonitor].[dbo].[RAINFALL_HOUR] 
                                   where MONITORNUM = '{0}' 
                                   and TIME >= '{1}' 
                                   and TIME <='{2}' order by [RAINFALL _{3}_HOUR] desc;", condition.State, condition.StartTime.ToShortDateString(), condition.EndTime.ToShortDateString(), hourArry[i]);
                commandText+= string.Format(@"SELECT top 1 
                                   [RAINFALL _{3}_HOUR]
                                   ,TIME
                                   ,[RAINFALL _{3}_HOUR_C]
                                   FROM [DB_RainMonitor].[dbo].[RAINFALL_HOUR] 
                                   where MONITORNUM = '{0}' 
                                   and TIME >= '{1}-{4}-1' 
                                   and TIME <'{2}-{5}-1' order by [RAINFALL _{3}_HOUR] desc;", condition.State, condition.StartTime.Year, condition.EndTime.AddMonths(1).Year, hourArry[i], condition.StartTime.Month, condition.EndTime.AddMonths(1).Month);
            }

            ds = SqlHelper.ExecuteDataset(SqlHelper.GetConnection(), CommandType.Text, commandText);
            for (int i = 0; i < hourArry.Length*2; i++)
            {
                if (ds.Tables[i].Rows.Count > 0)
                {
                    if (i % 2 == 0)
                    {

                        DataRow row = ds.Tables[i].Rows[0];
                        RainCaculateResult temp = new RainCaculateResult();
                        temp.MaxValue = ConvertDecimal(row[0]);
                        temp.MaxValueDate = DateTime.Parse(row[1].ToString());
                        temp.MaxValueQc = row[2]?.ToString();
                        temp.EventNum = condition.EventNum;
                        temp.During = hourArry[i / 2];
                        result.Add(temp);
                    }
                    else
                    {
                        DataRow row = ds.Tables[i].Rows[0];
                        RainCaculateResult tempMonth = new RainCaculateResult();
                        tempMonth.MaxValue = ConvertDecimal(row[0]);
                        tempMonth.MaxValueDate = DateTime.Parse(row[1].ToString());
                        tempMonth.MaxValueQc = row[2]?.ToString();
                        tempMonth.MonthMax = true;
                        tempMonth.EventNum = condition.EventNum;
                        tempMonth.During = hourArry[(i - 1) / 2];
                        result.Add(tempMonth);
                    }
                }
            }

            commandText = string.Empty;
            //1d,2d,3d,5d,7d,15d,30d时间范围最大值
            for (int i = 0; i < dayArry.Length; i++)
            {
                commandText += string.Format(@"SELECT top 1 
                                   [RAINFALL _{3}_DAY]
                                   ,TIME
                                   ,[RAINFALL _{3}_DAY_C]
                                   FROM [DB_RainMonitor].[dbo].[RAINFALL_DAY] 
                                   where MONITORNUM = '{0}' 
                                   and TIME >= '{1}' 
                                   and TIME <='{2}' order by [RAINFALL _{3}_DAY] desc;", condition.State, condition.StartTime.ToShortDateString(), condition.EndTime.ToShortDateString(), dayArry[i]);
                commandText+= string.Format(@"SELECT top 1 
                                   [RAINFALL _{3}_DAY]
                                   ,TIME
                                   ,[RAINFALL _{3}_DAY_C]
                                   FROM [DB_RainMonitor].[dbo].[RAINFALL_DAY] 
                                   where MONITORNUM = '{0}' 
                                   and TIME >= '{1}-{4}-1' 
                                   and TIME <'{2}-{5}-1' order by [RAINFALL _{3}_DAY] desc;", condition.State, condition.StartTime.Year, condition.EndTime.AddMonths(1).Year, dayArry[i], condition.StartTime.Month, condition.EndTime.AddMonths(1).Month);            
            }
            ds = SqlHelper.ExecuteDataset(SqlHelper.GetConnection(), CommandType.Text, commandText);
            for (int i = 0; i < dayArry.Length * 2; i++)
            {
                if (ds.Tables[i].Rows.Count > 0)
                {
                    if (i % 2 == 0)
                    {
                        DataRow row = ds.Tables[i].Rows[0];
                        RainCaculateResult temp = new RainCaculateResult();
                        temp.MaxValue = ConvertDecimal(row[0]);
                        temp.MaxValueDate = DateTime.Parse(row[1].ToString());
                        temp.MaxValueQc = row[2]?.ToString();
                        temp.Day = true;
                        temp.EventNum = condition.EventNum;
                        temp.During = dayArry[i / 2];
                        result.Add(temp);
                    }
                    else
                    {
                        DataRow row = ds.Tables[i].Rows[0];
                        RainCaculateResult tempMonth = new RainCaculateResult();
                        tempMonth.MaxValue = ConvertDecimal(row[0]);
                        tempMonth.MaxValueDate = DateTime.Parse(row[1].ToString());
                        tempMonth.MaxValueQc = row[2]?.ToString();
                        tempMonth.MonthMax = true;
                        tempMonth.Day = true;
                        tempMonth.EventNum = condition.EventNum;
                        tempMonth.During = dayArry[(i - 1) / 2];
                        result.Add(tempMonth);
                    }
                }
            }
            return result;
        }

        private static double? ConvertDecimal(object obj)
        {
            double result = 0;
            bool successed = double.TryParse(obj.ToString(), out result);
            if (successed)
                return result;
            return null;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            FormOutput.AppendProress(false);
            button2.Enabled = true;
            button3.Enabled = true;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            panel3.Enabled = radioButton1.Checked;
            panel4.Enabled = radioButton2.Checked;
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {
            panel5.Height = this.Height;
            panel5.Location = new Point((this.Width - panel5.Width) / 2, 0);
        }

        private void RainCaculateControl_Resize(object sender, EventArgs e)
        {
            panel5.Height = this.Height;
            panel5.Location = new Point((this.Width - panel5.Width) / 2, 0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            selectModelDialog.ShowDialog();
        }

        private void selectModelDialog_FileOk(object sender, CancelEventArgs e)
        {
            txtFilePath.Text = selectModelDialog.FileName;
            List<string> lstSheet = ExcelReader.GetExcelSheetName(txtFilePath.Text);
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(lstSheet.ToArray());
            comboBox1.SelectedIndex = 0;
        }

        private void lklblDown_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            saveModelDialog.FileName = "template.xlsx";
            if(saveModelDialog.ShowDialog()==DialogResult.OK)
            {
                File.Copy(Path.Combine(Application.StartupPath, "template.xlsx"), saveModelDialog.FileName);
                MsgBox.ShowInfo("下载完成！");
            }           
        }

        /// <summary>
        /// 开始计算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            List<RainCaculateConditon> lstCondition = new List<RainCaculateConditon>();

            if (radioButton1.Checked)
            {
                if (dateTimePicker1.Value == null || dateTimePicker2.Value == null || string.IsNullOrEmpty(txtState.Text))
                {
                    MsgBox.ShowInfo("计算参数不足,请补充.");
                    return;
                }
                RainCaculateConditon temp = new RainCaculateConditon()
                {
                    StartTime = dateTimePicker1.Value,
                    EndTime = dateTimePicker2.Value,
                    EventNum = "1",
                    State = txtState.Text

                };
                lstCondition.Add(temp);
            }
            else
            {
                if (string.IsNullOrEmpty(txtFilePath.Text))
                {
                    MsgBox.ShowInfo("计算参数不足,请补充.");
                    return;
                }
                //读取Excel模板
                DataTable sheetTable = ExcelReader.GetExcelContext(txtFilePath.Text, comboBox1.Text);

                for (int i = 1; i < sheetTable.Rows.Count; i++)
                {
                    try
                    {
                        RainCaculateConditon temp = new RainCaculateConditon()
                        {

                            StartTime = DateTime.Parse(sheetTable.Rows[i][1].ToString()),
                            EndTime = DateTime.Parse(sheetTable.Rows[i][2].ToString()),
                            EventNum = sheetTable.Rows[i][0].ToString(),
                            State = sheetTable.Rows[i][3].ToString()

                        };
                        lstCondition.Add(temp);
                    }
                    catch
                    {

                    }
                }
            }
            button2.Enabled = false;
            button3.Enabled = false;
            dataGridView1.Rows.Clear();
            backgroundWorker1.RunWorkerAsync(lstCondition);
        }

        /// <summary>
        /// 结果导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Excel模板|*.xls";
            dialog.FileName = "雨量计算结果";
            if (DialogResult.OK==dialog.ShowDialog())
            {
                try
                {
                    DataTable dt = new DataTable();
                    // 列强制转换
                    for (int count = 0; count < dataGridView1.Columns.Count; count++)
                    {
                        DataColumn dc = new DataColumn(dataGridView1.Columns[count].Name.ToString());
                        dt.Columns.Add(dc);
                    }
                    // 循环行
                    for (int count = 0; count < dataGridView1.Rows.Count; count++)
                    {
                        DataRow dr = dt.NewRow();
                        for (int countsub = 0; countsub < dataGridView1.Columns.Count; countsub++)
                        {
                            dr[countsub] = Convert.ToString(dataGridView1.Rows[count].Cells[countsub].Value);
                        }
                        dt.Rows.Add(dr);
                    }
                    XmlHelper.SaveDataToExcelFile(dt, dialog.FileName);
                    MsgBox.ShowInfo("保存成功！");
                }
                catch(Exception ex)
                {
                    MsgBox.ShowError($"保存失败：{ex.Message}");
                }
            }
          
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            FormOutput.AppendProress(e.ProgressPercentage);

            List<RainCaculateResult> result = e.UserState as List<RainCaculateResult>;

            List<string> lstEventNum = result.OrderBy(t => t.EventNum).Select(t => t.EventNum).Distinct().ToList();
            foreach (var item in lstEventNum)
            {
                int index = dataGridView1.Rows.Add();
                //赋值各行数据
                dataGridView1[0, index].Value = item;
                dataGridView1[1, index].Value = lstCondition.Where(t => t.EventNum == item).Select(t => t.StartTime.ToLongDateString()).FirstOrDefault();
                dataGridView1[2, index].Value = lstCondition.Where(t => t.EventNum == item).Select(t => t.EndTime.ToLongDateString()).FirstOrDefault(); ;
                dataGridView1[3, index].Value = lstCondition.Where(t => t.EventNum == item).Select(t => t.State).FirstOrDefault();

                for (int i = 0; i < hourArry.Length; i++)
                {
                    string columName = $"RAINFALL_{hourArry[i]}_HOUR";
                    dataGridView1[columName, index].Value = result.Where(t => t.Day == false && t.EventNum == item && t.MonthMax == false).Select(t => t.MaxValue).FirstOrDefault();

                    columName = $"RAINFALL_{hourArry[i]}_HOUR_TIME";
                    dataGridView1[columName, index].Value = result.Where(t => t.Day == false && t.EventNum == item && t.MonthMax == false).Select(t => t.MaxValueDate).FirstOrDefault();

                    columName = $"RAINFALL_{hourArry[i]}_HOUR_QC";
                    dataGridView1[columName, index].Value = result.Where(t => t.Day == false && t.EventNum == item && t.MonthMax == false).Select(t => t.MaxValueQc).FirstOrDefault();

                    columName = $"MAX_{hourArry[i]}_HOUR_MONTH";
                    dataGridView1[columName, index].Value = result.Where(t => t.Day == false && t.EventNum == item && t.MonthMax == true).Select(t => t.MaxValue).FirstOrDefault();

                    columName = $"MAX_{hourArry[i]}_HOUR_MONTH_TIME";
                    dataGridView1[columName, index].Value = result.Where(t => t.Day == false && t.EventNum == item && t.MonthMax == true).Select(t => t.MaxValueDate).FirstOrDefault();

                    columName = $"MAX_{hourArry[i]}_HOUR_MONTH_QC";
                    dataGridView1[columName, index].Value = result.Where(t => t.Day == false && t.EventNum == item && t.MonthMax == true).Select(t => t.MaxValueQc).FirstOrDefault();

                }

                for (int i = 0; i < dayArry.Length; i++)
                {
                    string columName = $"RAINFALL_{dayArry[i]}_DAY";
                    dataGridView1[columName, index].Value = result.Where(t => t.Day == true && t.EventNum == item && t.MonthMax == false).Select(t => t.MaxValue).FirstOrDefault();

                    columName = $"RAINFALL_{dayArry[i]}_DAY_TIME";
                    dataGridView1[columName, index].Value = result.Where(t => t.Day == true && t.EventNum == item && t.MonthMax == false).Select(t => t.MaxValueDate).FirstOrDefault();

                    columName = $"RAINFALL_{dayArry[i]}_DAY_QC";
                    dataGridView1[columName, index].Value = result.Where(t => t.Day == true && t.EventNum == item && t.MonthMax == false).Select(t => t.MaxValueQc).FirstOrDefault();

                    columName = $"MAX_{dayArry[i]}_DAY_MONTH";
                    dataGridView1[columName, index].Value = result.Where(t => t.Day == true && t.EventNum == item && t.MonthMax == true).Select(t => t.MaxValue).FirstOrDefault();

                    columName = $"MAX_{dayArry[i]}_DAY_MONTH_TIME";
                    dataGridView1[columName, index].Value = result.Where(t => t.Day == true && t.EventNum == item && t.MonthMax == true).Select(t => t.MaxValueDate).FirstOrDefault();

                    columName = $"MAX_{dayArry[i]}_DAY_MONTH_QC";
                    dataGridView1[columName, index].Value = result.Where(t => t.Day == true && t.EventNum == item && t.MonthMax == true).Select(t => t.MaxValueQc).FirstOrDefault();

                }

            }
        }
    }
}
