using iTelluro.Explorer.Raster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FloodPeakUtility.Algorithm
{
    public class DEMReader
    {
        /// <summary>
        /// 获取高程矩阵
        /// </summary>
        public static double[,] GetElevation(RasterReader read)
        {
            int rowCount = read.RowCount;
            int colCount = read.ColumnCount;
            double[,] result = new double[rowCount, colCount];//源数据高程值
            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < colCount; col++)
                {
                    result[row, col] = ReadBand(read, row, col);
                }
            }
            return result;
        }

        /// <summary>
        /// 读取特定像元值
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        private static double ReadBand(RasterReader read, int row, int col)
        {
            double[] d = null;
            read.ReadBand(col, row, 1, 1, out d);
            return d[0];
        }
    }
}
