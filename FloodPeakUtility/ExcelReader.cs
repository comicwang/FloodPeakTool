using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Reflection;
using System.Text;

namespace FloodPeakUtility
{
    public static class ExcelReader
    {
        public static OleDbConnection GetOleDbConnection(string excelPath)
        {
            string conStr = "Provider=Microsoft.jet.oledb.4.0;data source=" + excelPath + ";extended properties='Excel 8.0;HDR=yes;IMEX=1'";
            string conStrX = "Provider=Microsoft.ACE.OLEDB.12.0;data source=" + excelPath + ";Extended Properties='Excel 12.0;HDR=yes;IMEX=1'";

            OleDbConnection con = null;
            try
            {
                if (excelPath.EndsWith(".xls", StringComparison.CurrentCultureIgnoreCase))
                {
                    con = new OleDbConnection(conStr);
                }
                else if (excelPath.EndsWith(".xlsx", StringComparison.CurrentCultureIgnoreCase))
                {
                    con = new OleDbConnection(conStrX);
                }
                else if (excelPath.EndsWith(".xlsb", StringComparison.CurrentCultureIgnoreCase))
                {
                    con = new OleDbConnection(conStrX);
                }
                else
                {
                    throw new Exception("不支持的excel文件格式");
                }
                con.Open();
            }
            catch
            {
                throw new Exception("不支持的excel文件格式");
            }
            return con;
        }

        public static List<string> GetExcelSheetName(string excelPath)
        {
            OleDbConnection con = GetOleDbConnection(excelPath);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            //获取sheets名称
            DataTable schamaTbl = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" });
            List<string> sheetsList = new List<string>();
            for (int i = 0; i < schamaTbl.Rows.Count; i++)
            {
                sheetsList.Add(schamaTbl.Rows[i]["TABLE_NAME"].ToString());
            }
            con.Close();
            return sheetsList;
        }

        public static DataTable GetExcelContext(string excelPath, string sheetName)
        {
            OleDbConnection con = GetOleDbConnection(excelPath);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            string cmdStr = "select * from [" + sheetName + "]";
            OleDbDataAdapter adapter = new OleDbDataAdapter(cmdStr, con);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);

            con.Close();
            DataTable table = dataSet.Tables[0];
            adapter.Dispose();

            return table;
        }
    }
}
