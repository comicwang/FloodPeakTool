using OSGeo.GDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloodPeakUtility.Algorithm
{
    public class RasterWriter
    {
        private double[] _pixels1;
        OSGeo.GDAL.Dataset _dataSet;
        OSGeo.GDAL.Band _band1;

        string _fileName;

        private ResultInfo _resultInfo;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nFileName"></param>
        /// <param name="resultInfo"></param>
        public RasterWriter(string nFileName, ResultInfo resultInfo)
        {
            try
            {
                _fileName = nFileName;
                _resultInfo = resultInfo;

                OSGeo.GDAL.Gdal.AllRegister();
                OSGeo.GDAL.Driver nDriver = OSGeo.GDAL.Gdal.GetDriverByName("GTiff");

                double[] inGeo = _resultInfo.InGeo;
                string[] strs = new string[] { "INTERLEAVE = PIXEL" };
                string prjWGS84 = "GEOGCS[\"WGS 84\",DATUM[\"WGS_1984\",SPHEROID[\"WGS 84\",6378137,298.257223563,AUTHORITY[\"EPSG\",\"7030\"]],AUTHORITY[\"EPSG\",\"6326\"]],PRIMEM[\"Greenwich\",0,AUTHORITY[\"EPSG\",\"8901\"]],UNIT[\"degree\",0.0174532925199433,AUTHORITY[\"EPSG\",\"912\"]],AUTHORITY[\"EPSG\",\"4326\"]]";

                _dataSet = nDriver.Create(_fileName,
                                          _resultInfo.Width,
                                          _resultInfo.Height,
                                         1,
                                          OSGeo.GDAL.DataType.GDT_Float32,
                                          strs);

                _pixels1 = new double[_resultInfo.Width * _resultInfo.Height];


                _dataSet.SetGeoTransform(inGeo);
                _dataSet.SetProjection(prjWGS84);
                _dataSet.Dispose();
            }
            catch (System.Exception err)
            {
                throw new Exception("数据写入失败。" + err.Message);
            }
        }

        public void WriterRasterResult(string nFileName, ResultInfo resultInfo)
        {
            try
            {
                _fileName = nFileName;
                _resultInfo = resultInfo;

                OSGeo.GDAL.Gdal.AllRegister();
                OSGeo.GDAL.Driver nDriver = OSGeo.GDAL.Gdal.GetDriverByName("GTiff");

                double[] inGeo = _resultInfo.InGeo;
                string[] strs = new string[] { "INTERLEAVE = PIXEL" };
                string prjWGS84 = "GEOGCS[\"WGS 84\",DATUM[\"WGS_1984\",SPHEROID[\"WGS 84\",6378137,298.257223563,AUTHORITY[\"EPSG\",\"7030\"]],AUTHORITY[\"EPSG\",\"6326\"]],PRIMEM[\"Greenwich\",0,AUTHORITY[\"EPSG\",\"8901\"]],UNIT[\"degree\",0.0174532925199433,AUTHORITY[\"EPSG\",\"912\"]],AUTHORITY[\"EPSG\",\"4326\"]]";

                _dataSet = nDriver.Create(_fileName,
                                          _resultInfo.Width,
                                          _resultInfo.Height,
                                          1,
                                          OSGeo.GDAL.DataType.GDT_Byte,
                                          strs);

                _pixels1 = new double[_resultInfo.Width * _resultInfo.Height];


                _dataSet.SetGeoTransform(inGeo);
                _dataSet.SetProjection(prjWGS84);
                _dataSet.Dispose();
            }
            catch (System.Exception err)
            {
                throw new Exception("数据写入失败。" + err.Message);
            }
        }

        public void SetValue(int x, int y, double value)
        {
            try
            {
                _pixels1[x + y * _resultInfo.Width] = value;
            }
            catch
            {
            }
        }

        /// <summary>
        /// 数据保存
        /// </summary>
        public void Save()
        {
            try
            {
                OSGeo.GDAL.Gdal.AllRegister();
                _dataSet = OSGeo.GDAL.Gdal.Open(_fileName, OSGeo.GDAL.Access.GA_Update);

                _band1 = _dataSet.GetRasterBand(1);
                CPLErr err1 = _band1.WriteRaster(0,
                                                 0,
                                                 _resultInfo.Width,
                                                 _resultInfo.Height,
                                                 _pixels1,
                                                 _resultInfo.Width,
                                                 _resultInfo.Height,
                                                 0,
                                                 0);

                _dataSet.Dispose();
                _band1.Dispose();

            }
            catch (Exception ex)
            {
                throw new Exception("数据写入失败。" + ex.Message);
            }
        }


        public void Save(int xoff, int yoff, int buf_xsize, int buf_ysize, double[] buffer)
        {
            try
            {
                OSGeo.GDAL.Gdal.AllRegister();
                _dataSet = OSGeo.GDAL.Gdal.Open(_fileName, OSGeo.GDAL.Access.GA_Update);

                _band1 = _dataSet.GetRasterBand(1);
                CPLErr err1 = _band1.WriteRaster(xoff,
                                                 yoff,
                                                 _resultInfo.Width,
                                                 buf_ysize,
                                                 buffer,
                                                 buf_xsize,
                                                buf_ysize,
                                                 0,
                                                 0);

                _dataSet.Dispose();
                _band1.Dispose();

            }
            catch (Exception ex)
            {
                throw new Exception("数据写入失败。" + ex.Message);
            }
        }
    }

    public class ResultInfo
    {
        public int Width { get;  set; }
        public int Height { get;  set; }
        public double[] InGeo { get;  set; }
    }
}

