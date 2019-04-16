using OSGeo.GDAL;
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
        public static double[,] GetElevation(iTelluro.Explorer.Raster.RasterReader read)
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
        private static double ReadBand(iTelluro.Explorer.Raster.RasterReader read, int row, int col)
        {
            double[] d = null;
            read.ReadBand(col, row, 1, 1, out d);
            return d[0];
        }

        /// <summary>
        /// 保存Dem文件
        /// </summary>
        /// <param name="read"></param>
        /// <param name="Accumulation"></param>
        /// <param name="funcVal"></param>
        /// <param name="savePath"></param>
        public static void SaveDem(iTelluro.Explorer.Raster.RasterReader read, int[,] Accumulation, Func<double, double> funcVal, string savePath)
        {

            double[] inGeo = new double[6];
            read.DataSet.GetGeoTransform(inGeo);
            RasterWriter write = new RasterWriter(savePath, new ResultInfo() { Width = Accumulation.GetLength(1), Height = Accumulation.GetLength(0), InGeo = inGeo });

            List<double> data = new List<double>();

            for (int i = 0; i < Accumulation.GetLength(0); i++)
            {
                for (int j = 0; j < Accumulation.GetLength(1); j++)
                {
                    if (funcVal != null)
                    {
                        write.SetValue(j, i, funcVal(Accumulation[i, j]));
                    }

                    else
                    {
                        write.SetValue(j, i, Accumulation[i, j]);
                    }
                }
            }

            write.Save();
            //重新生成汇流栅格
            //RasterWriter writer = new RasterWriter(savePath, 1, Accumulation.GetLength(1), Accumulation.GetLength(0), DataType.GDT_Float32);
            //writer.SetProjection(read.DataSet.GetProjectionRef());
            //writer.SetPosition(read.MapRectEast, read.MapRectWest, read.CellSizeX, read.CellSizeY);
            //writer.SetNodata(1, -1);//设置无数据
            //double[] buffer = data.ToArray();
            //writer.WriteBand(1, 0, 0, Accumulation.GetLength(1), Accumulation.GetLength(0), buffer);
            //writer.Dispose();
        }

        /// <summary>
        /// 保存Dem文件
        /// </summary>
        /// <param name="read"></param>
        /// <param name="Accumulation"></param>
        /// <param name="funcVal"></param>
        /// <param name="savePath"></param>
        public static void SaveDem(iTelluro.Explorer.Raster.RasterReader read, double[,] Accumulation, Func<double, double> funcVal, string savePath)
        {
            double[] inGeo = new double[6];
            read.DataSet.GetGeoTransform(inGeo);
            RasterWriter write = new RasterWriter(savePath, new ResultInfo() { Width = Accumulation.GetLength(1), Height = Accumulation.GetLength(0), InGeo = inGeo });

            List<double> data = new List<double>();

            for (int i = 0; i < Accumulation.GetLength(0); i++)
            {
                for (int j = 0; j < Accumulation.GetLength(1); j++)
                {
                    if (funcVal != null)
                    {
                        write.SetValue(j, i, funcVal(Accumulation[i, j]));
                    }

                    else
                    {
                        write.SetValue(j, i, Accumulation[i, j]);
                    }
                }
            }

            write.Save();
            ////重新生成汇流栅格
            //RasterWriter writer = new RasterWriter(savePath, 1, Accumulation.GetLength(1), Accumulation.GetLength(0), DataType.GDT_Float32);
            //writer.SetProjection(read.DataSet.GetProjectionRef());
            //writer.SetPosition(read.MapRectWest, read.MapRectNorth, read.CellSizeX, read.CellSizeY);
            //writer.SetNodata(1, -1);//设置无数据
            //double[] buffer = data.ToArray();
            //writer.WriteBand(1, 0, 0, Accumulation.GetLength(1), Accumulation.GetLength(0), buffer);
            //writer.Dispose();
        }
    }
}
