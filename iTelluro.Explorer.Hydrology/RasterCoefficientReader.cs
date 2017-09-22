using FloodPeakUtility;
using FloodPeakUtility.UI;
using iTelluro.DataTools.Utility.Img;
using iTelluro.Explorer.Raster;
using iTelluro.GlobeEngine.Analyst;
using iTelluro.GlobeEngine.DataSource.Geometry;
using OSGeo.GDAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FloodPeakToolUI
{
    public class RasterCoefficientReader
    {
        /// <summary>
        /// 用于计算shp文件中的系数平均值
        /// </summary>
        /// <param name="shppath">shp路径</param>
        /// <param name="inputDemPath">tif路径</param>
        /// <returns></returns>
        public static double? ReadCoeficient(string shppath, string inputDemPath)
        {
            FormOutput.AppendLog("按规定范围裁剪栅格..");
            //创建一个临时目录
            string outDemPath = Path.Combine(Path.GetTempPath(), "WDEM.tif");
            RasterReader raster = null;
            Band band = null;
            try
            {
                ImgCut.CutTiff(shppath, inputDemPath, outDemPath);
                if (File.Exists(outDemPath) == false)
                {
                    FormOutput.AppendLog("裁剪失败！");
                    return null;
                }
                FormOutput.AppendLog("开始读取裁剪后的栅格数据..");
                raster = new RasterReader(inputDemPath);
                int row = raster.RowCount;
                int col = raster.ColumnCount;
               // int count = raster.RasterCount;

                int xsize = raster.DataSet.RasterXSize;
                int ysize = raster.DataSet.RasterYSize;
                band = raster.DataSet.GetRasterBand(1);

                //无效值
                //FormOutput.AppendLog("获取栅格数据无效值..");
                double nodatavalue;
                int hasval;
                band.GetNoDataValue(out nodatavalue, out hasval);
               // FormOutput.AppendLog("栅格数据无效值为" + nodatavalue);
                double[] readData = new double[row * col];
                band.ReadRaster(0, 0, xsize, ysize, readData, row, col, 0, 0);

                //FormOutput.AppendLog("开始整理损失指数数据..");
                var res = readData.GroupBy(t => t).Select(t => new { count = t.Count(), Key = t.Key }).ToArray();
                double total = 0;
                double totalcount = 0;
                foreach (var s in res)
                {
                    if (s.Key != nodatavalue)
                    {
                        total += s.Key * s.count;
                        totalcount += s.count;
                    }
                }
                double R = total / totalcount;
                R = R / 1000;  //转换？？

                return R;
            }
            catch (Exception ex)
            {
                FormOutput.AppendLog("获取栅格数据异常：" + ex.Message);
                return null;
            }
            finally
            {
                //销毁
                if (raster != null)
                {
                    band.Dispose();
                    raster.Dispose();
                }
            }
        }

        public static double ReadBand(RasterReader reader, int col, int row)
        {
            double[] d = null;
            reader.ReadBand(col, row, 1, 1, out d);
            return d[0];
        }

        /// <summary>
        /// 求两点距离
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static double Length(Point3d from, Point3d to)
        {
            double l = SpatialAnalysis.CaculateGCDistance(from.Y, from.X, to.Y, to.X);
            return l;
        }
    }
}
