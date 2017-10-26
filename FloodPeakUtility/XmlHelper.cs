using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using Microsoft.Office.Interop.Excel;
using System.Threading;

namespace FloodPeakUtility
{
    /// <summary>
    /// Author:王冲
    /// Date:2017-09-12
    /// xml文件操作的帮助类
    /// </summary>
    public class XmlHelper
    {
        /// <summary>
        /// 将xml文件序列化为实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlPath"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string xmlPath) where T : class
        {
            if (File.Exists(xmlPath))
            {
                //解决文件占用的问题
                while (true)
                {
                    try
                    {
                        using (StreamReader reader = new StreamReader(xmlPath))
                        {
                            XmlSerializer xs = new XmlSerializer(typeof(T));
                            return xs.Deserialize(reader) as T;
                        }
                    }
                    catch
                    {

                    }
                }
            }
            return default(T);
        }

        /// <summary>
        /// 将实体存储到xml文件中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="xmlPath"></param>
        public static void Serialize<T>(T obj, string xmlPath) where T : class
        {
            using (StreamWriter writer = new StreamWriter(xmlPath))
            {
                XmlSerializer xs = new XmlSerializer(typeof(T));
                xs.Serialize(writer, obj);
            }
        }

        /// <summary>
        /// 将DataTable转换为List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static IList<T> DataTableToList<T>(System.Data.DataTable dt) where T : class,new()
        {

            IList<T> ts = new List<T>();// 定义集合
            Type type = typeof(T); // 获得此模型的类型
            string tempName = "";
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                PropertyInfo[] propertys = t.GetType().GetProperties();// 获得此模型的公共属性
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;
                    if (dt.Columns.Contains(tempName))
                    {
                        if (!pi.CanWrite) continue;
                        object value = dr[tempName];
                        if (value != DBNull.Value)
                            pi.SetValue(t, value, null);
                    }
                }
                ts.Add(t);
            }
            return ts;
        }

        /// <summary>
        /// 保存集合数据到xls文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="studentList"></param>
        /// <param name="filePath"></param>
        public static void SaveDataToExcelFile<T>(List<T> studentList, string filePath) where T:class
        {
            object misValue = System.Reflection.Missing.Value;
            Application xlApp = new Application();
            Workbook xlWorkBook = xlApp.Workbooks.Add(misValue);
            Worksheet xlWorkSheet = (Worksheet)xlWorkBook.Worksheets.get_Item(1);


            PropertyInfo[] propertys = typeof(T).GetProperties();// 获得此模型的公共属性
            for (int i = 0; i < studentList.Count; i++)
            {
                for (int j = 0; j < propertys.Length; j++)
                {
                    xlWorkSheet.Cells[i + 1, j + 1] = propertys[j].GetValue(studentList[i], null);
                }           
            }
            try
            {
                xlWorkBook.SaveAs(filePath, XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();
            }
            catch (Exception ex)
            { }

        }

        /// <summary>
        /// 保存DataTable到Excel中
        /// </summary>
        /// <param name="table"></param>
        /// <param name="filePath"></param>
        public static void SaveDataToExcelFile(System.Data.DataTable table, string filePath)
        {
            object misValue = System.Reflection.Missing.Value;
            Application xlApp = new Application();
            Workbook xlWorkBook = xlApp.Workbooks.Add(misValue);
            Worksheet xlWorkSheet = (Worksheet)xlWorkBook.Worksheets.get_Item(1);
            if (table.Columns.Count > 0)
                //添加表头
                for (int col = 0; col < table.Columns.Count; col++)
                {
                    xlWorkSheet.Cells[1, col + 1] = table.Columns[col].ColumnName;
                }
            if (table.Rows.Count > 0)
                //添加内容
                for (int row = 0; row < table.Rows.Count; row++)
                {
                    for (int col = 0; col < table.Columns.Count; col++)
                    {
                        xlWorkSheet.Cells[row + 2, col + 1] = table.Rows[row][col];
                    }
                }
            try
            {
                xlWorkBook.SaveAs(filePath, XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();
            }
            catch (Exception ex)
            { }
        }

        /// <summary>
        /// 保存二位数组到Excel中
        /// </summary>
        /// <param name="table"></param>
        /// <param name="filePath"></param>
        public static void SaveDataToExcelFile(double[,] matrix, string filePath)
        {
            object misValue = System.Reflection.Missing.Value;
            Application xlApp = new Application();
            Workbook xlWorkBook = xlApp.Workbooks.Add(misValue);
            Worksheet xlWorkSheet = (Worksheet)xlWorkBook.Worksheets.get_Item(1);

            if (matrix.Length > 0)
                //添加内容
                for (int row = 0; row < matrix.GetLength(0); row++)
                {
                    for (int col = 0; col < matrix.GetLength(1); col++)
                    {
                        xlWorkSheet.Cells[row + 1, col + 1] = matrix[row, col];
                    }
                }
            try
            {
                xlWorkBook.SaveAs(filePath, XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();
            }
            catch (Exception ex)
            { }
        }


    }
}
