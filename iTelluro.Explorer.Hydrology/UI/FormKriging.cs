using iTelluro.DataTools.Utility.GIS;
using iTelluro.DataTools.Utility.SHP;
using iTelluro.Explorer.Raster;
using iTelluroLib.SpatialIndex;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using OSGeo.GDAL;
using OSGeo.OGR;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iTelluro.DataTools;

namespace FloodPeakToolUI.UI
{
    public partial class FormKriging : Form
    {
        private double _sourceSpan = 500; //源数据查找半径

        private double cellSize = 20;

        /// <summary>
        /// 逆矩阵
        /// </summary>
        private Matrix<double> Kn;
        private double _a;
        private double _c;
        double maxx = -180;
        double minx = 180000000;
        double maxy = -90;
        double miny = 9000000;
        List<iTelluro.DataTools.Utility.GIS.Point> resultList;
        private SpatialPtIdx<iTelluro.DataTools.Utility.GIS.Point> _sourceBlock;
        public FormKriging()
        {
            InitializeComponent();
        }

        private void btnKriging_Click(object sender, EventArgs e)
        {
            cellSize = double.Parse(this.textBox3.Text);
            bool isReadData = false;
            if (!isReadData)
            {
                //读取点数据
                ShpReader shp = new ShpReader(this.textBox1.Text);
                Rect r = ShpReader.GetFirstGeoExtent(this.textBox1.Text);
                List<iTelluro.DataTools.Utility.GIS.Point> points = new List<iTelluro.DataTools.Utility.GIS.Point>();
                //获取四至信息
                Feature ofea;
                while (((ofea = shp.layer.GetNextFeature()) != null))
                {
                    Geometry geo = ofea.GetGeometryRef();
                    Envelope env = new Envelope();
                    geo.GetEnvelope(env);
                    if (env.MinX < minx)
                    {
                        minx = env.MinX;
                    }
                    if (env.MinX > maxx)
                    {
                        maxx = env.MinX;
                    }
                    if (env.MaxY > maxy)
                    {
                        maxy = env.MaxY;
                    }
                    if (env.MaxY < miny)
                    {
                        miny = env.MaxY;
                    }
                    points.Add(new iTelluro.DataTools.Utility.GIS.Point(env.MaxX, env.MaxY, ofea.GetFieldAsDouble("RASTERVALU")));
                }

                _sourceBlock = CreateSpatialIndex(points);

                // //参与计算的预测点个数最多12个
                // _sourceSpan = int.Parse(this.textBox2.Text);
                // //获取点的邻域内的pointCount个点
                //iTelluro.DataTools.Utility.GIS.Point p1 = new iTelluro.DataTools.Utility.GIS.Point(214234, 3295954);
                // List<iTelluro.DataTools.Utility.GIS.Point> resultList = _sourceBlock.Find(
                //     p1.X - _sourceSpan, p1.Y - _sourceSpan,
                //     p1.X + _sourceSpan, p1.Y + _sourceSpan);

                resultList = points;
                Kriging(resultList, 12, true, KrigingModel.Spherical);
                isReadData = true;
            }

            ////获取栅格像元大小
            int width = (int)Math.Ceiling((maxx - minx) / cellSize);
            int height = (int)Math.Ceiling((maxy - miny) / cellSize);

            //生成新的影像文件
            //string tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".tif");
            string tempPath = this.textBox2.Text;
            try
            {
                RasterWriter writer = new RasterWriter(tempPath, 1, width, height, DataType.GDT_Float32);
                writer.SetProjection("");
                writer.SetPosition(minx, maxy, cellSize, cellSize);
                writer.SetNodata(1, 0);
                for (int i = 0; i < height; i++)
                {
                    float[] vals = new float[width];
                    for (int j = 0; j < width; j++)
                    {
                        //double alt = 0;
                        //double[,] d = new double[resultList.Count + 1, 1];
                        //double x = minx + cellSize * j;
                        //double y = maxy - cellSize * i;
                        //for (int n = 0; n < resultList.Count; n++)
                        //{
                        //    d[n, 0] = CalCij(KrigingModel.Spherical, new iTelluro.DataTools.Utility.GIS.Point(x, y), resultList[n], _c, _a);
                        //}
                        //d[resultList.Count, 0] = 1;
                        ////D矩阵
                        //Matrix<double> mD = DenseMatrix.OfArray(d);

                        ////权重矩阵
                        //Matrix<double> mW = Kn.Multiply(mD);
                        //if (resultList.Count + 1 == mW.RowCount)
                        //{
                        //    //拟合的高程值
                        //    for (int m = 0; m < mW.RowCount - 1; m++)
                        //    {
                        //        alt += mW[m, 0] * resultList[m].Z;
                        //    }
                        //}
                        //vals[j] = (float)alt;

                        //样条函数
                        double alt = 0;
                        double x = minx + cellSize * j;
                        double y = maxy - cellSize * i;
                        iTelluro.DataTools.Utility.GIS.Point p1 = new iTelluro.DataTools.Utility.GIS.Point(x, y);
                        List<iTelluro.DataTools.Utility.GIS.Point> calPoints = _sourceBlock.Find(
                     p1.X - _sourceSpan, p1.Y - _sourceSpan,
                     p1.X + _sourceSpan, p1.Y + _sourceSpan);
                        vals[j] = (float)Spline(calPoints, p1);
                    }
                    writer.WriteBand(1, 0, i, width, 1, vals);
                }
                writer.Dispose();

            }
            catch (Exception ex)
            {
                throw new Exception("创建拟合校正DSM文件失败：" + ex.Message);
            }
        }

        /// <summary>
        /// 创建空间索引
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        internal SpatialPtIdx<iTelluro.DataTools.Utility.GIS.Point> CreateSpatialIndex(IEnumerable<iTelluro.DataTools.Utility.GIS.Point> points)
        {
            SpatialPtIdx<iTelluro.DataTools.Utility.GIS.Point> index = new SpatialPtIdx<iTelluro.DataTools.Utility.GIS.Point>();
            foreach (iTelluro.DataTools.Utility.GIS.Point p in points)
            {
                SpatialVertex sp = new SpatialVertex(p.X, p.Y);
                index.Add(sp, p);
            }
            return index;
        }

        class RegressionPoint
        {
            public double distance;
            public double semivariogram;
            public RegressionPoint(double distance, double semivariogram)
            {
                this.distance = distance;
                this.semivariogram = semivariogram;
            }
        }

        /// <summary>
        /// 插值分析
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="pointCount"></param>
        /// <param name="isKriging"></param>
        /// <param name="SEM"></param>
        void Kriging(List<iTelluro.DataTools.Utility.GIS.Point> resultList, int pointCount, bool isKriging, KrigingModel SEM)
        {
            int length = resultList.Count;
            List<RegressionPoint> points = new List<RegressionPoint>();
            double maxDistance = 0;
            double minDistance = 100000000;
            for (int i = 0; i < length; i++)
            {
                iTelluro.DataTools.Utility.GIS.Point p = resultList[i];
                for (int j = i + 1; j < length; j++)
                {
                    iTelluro.DataTools.Utility.GIS.Point p2 = resultList[j];
                    double distance = GetPointDistance(p, p2);
                    if (distance > maxDistance)
                    {
                        maxDistance = distance;
                    }
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                    }
                    points.Add(new RegressionPoint(distance, 0.5 * System.Math.Pow((p.Z - p2.Z), 2)));
                }
            }

            List<RegressionPoint> calPoints = new List<RegressionPoint>();
            int h = (int)maxDistance / 10;//步长
            for (int i = 0; i < 10; i++)
            {
                List<RegressionPoint> tempPoints = Query(points, i * h, (i + 1) * h);
                if (tempPoints.Count > 0)
                {
                    double values = 0;
                    double distance = 0;
                    foreach (RegressionPoint p in tempPoints)
                    {
                        distance += p.distance;
                        values += p.semivariogram;
                    }
                    distance = distance / tempPoints.Count;//平均步长
                    values = values / tempPoints.Count;//平均变异差
                    calPoints.Add(new RegressionPoint(distance, values));
                }
            }
            length = calPoints.Count;
            //取点若干个
            if (isKriging)
            {
                //判断计算模型
                switch (SEM)
                {
                    case KrigingModel.Spherical:
                        //右边侧矩阵系数(两点之间距离与距离的3次方)
                        double[,] leftKnowns = new double[length, 2];

                        //左边侧矩阵系数(高程值)
                        double[,] rightKnowns = new double[length, 1];
                        for (int i = 0; i < length; i++)
                        {
                            double dis = calPoints[i].distance;
                            leftKnowns[i, 0] = dis;
                            leftKnowns[i, 1] = System.Math.Pow(dis, 3);
                            rightKnowns[i, 0] = calPoints[i].semivariogram;
                        }

                        //左侧矩阵
                        Matrix<double> mLeft = DenseMatrix.OfArray(leftKnowns);
                        Matrix<double> mRight = DenseMatrix.OfArray(rightKnowns);

                        //矩阵求解方法MatrixSlove
                        MathNet.Numerics.Control.UseManaged();
                        Matrix<double> rlt;

                        try
                        {
                            rlt = mLeft.Solve(mRight);
                        }
                        catch (System.Exception ex)
                        {
                            throw new Exception("拟合解算失败：" + ex.Message);
                        }

                        //得到未知数a、b、c
                        //double b = rlt[0,0];
                        //double c0 = rlt[0, 0];
                        double x1 = rlt[0, 0];
                        double x2 = rlt[1, 0];
                        double a = System.Math.Sqrt(x1 / 3 * (-x2));
                        double c = 2 * a * x1 / 3;
                        _a = a;
                        _c = c;
                        length = resultList.Count + 1;
                        double[,] r = new double[length, length];
                        double[,] d = new double[length, 1];
                        double[,] w = new double[length, 1];

                        for (int i = 0; i < length - 1; i++)
                        {
                            for (int j = 0; j < length - 1; j++)
                            {
                                r[i, j] = CalCij(KrigingModel.Spherical, resultList[i], resultList[j], c, a);
                            }
                        }
                        for (int i = 0; i < length; i++)
                        {
                            if (i != length - 1)
                            {
                                r[i, length - 1] = 1;
                            }
                            else
                            {
                                for (int j = 0; j < length - 1; j++)
                                {
                                    r[i, j] = 1;
                                }
                            }
                        }
                        r[resultList.Count, resultList.Count] = 0;

                        //权重矩阵等于协方差矩阵的逆矩阵乘以D矩阵
                        //协方差矩阵
                        Matrix<double> s = DenseMatrix.OfArray(r);
                        MathNet.Numerics.Control.UseManaged();
                        Kn = s.Inverse();
                        break;
                    case KrigingModel.Circular:
                        break;
                    case KrigingModel.Gaussian://高斯牛顿法
                        PowerModel model = new PowerModel();
                        GaussNewtonSolver solver = new GaussNewtonSolver(0.001, 0.001, 10000, new DenseVector(new[] { 50.0, 1.5 }));
                        List<Vector<double>> solverIterations = new List<Vector<double>>();

                        double[] x = new double[calPoints.Count];
                        double[] y = new double[calPoints.Count];
                        for (int n = 0; n < calPoints.Count; n++)
                        {
                            //缩小数值，方便计算，之后会还原
                            x[n] = calPoints[n].distance;
                            y[n] = calPoints[n].semivariogram;
                        }

                        Vector<double> dataX = new DenseVector(x);
                        Vector<double> dataY = new DenseVector(y);

                        solver.Estimate(model, calPoints.Count, dataX, dataY, ref solverIterations);

                        double formula_c = solverIterations.Last()[0];
                        double formula_r = solverIterations.Last()[1];
                        break;
                }
            }
        }

        private List<RegressionPoint> Query(List<RegressionPoint> points, int i, int j)
        {
            List<RegressionPoint> list = new List<RegressionPoint>();
            foreach (RegressionPoint p in points)
            {
                if (p.distance > i && p.distance < j)
                {
                    list.Add(p);
                }
            }
            return list;
        }

        /// <summary>
        /// 计算Cij
        /// </summary>
        /// <param name="sem">半变异方程模型</param>
        /// <param name="p1">点1</param>
        /// <param name="p2">点2</param>
        /// <param name="c">系数c</param>
        /// <param name="a">系数a</param>
        /// <returns>Cij值</returns>
        private double CalCij(KrigingModel sem, iTelluro.DataTools.Utility.GIS.Point p1, iTelluro.DataTools.Utility.GIS.Point p2, double c, double a)
        {
            double cij = 0;
            double distance = GetPointDistance(p1, p2);
            if (distance == 0) return 0;
            switch (sem)
            {
                /*球面拟合计算参数a,b,c  
                r(h) = c0 + c*((3h/2a) - 0.5*(h/a)^3)
                公式简化为： r(h) = c0 + (3c/2a)*h - (c/2a^3)h^3
                令 a = c0, b = 3c/2a ,c = -c/2*a^3 , h=x1,h^3 =x2;
                即 f(h) = a + b*x1 + c*x2
                此处计算忽略常数项，计算a,c 进而计算r(h)的表达式:f(h) = C*(1.5*h/a -0.5*(h/a)^3) */
                case KrigingModel.Spherical:
                    cij = c - c * ((1.5 * distance / a) - (0.5 * System.Math.Pow(distance / a, 3)));
                    break;
                case KrigingModel.Circular:
                    break;
                /*指数拟合模型：
                 r(h)= C0+ C*(1-e^(-(h^2/a^2)))
                 */
                case KrigingModel.Exponential:
                    cij = c - c * System.Math.Exp(-3 * distance / a);
                    System.Math.Log(c);
                    break;
                /*高斯模型：                    
                 r(h) = C*(1-e^(-(3h/a)^2))
                 */
                case KrigingModel.Gaussian:
                    cij = c - c * System.Math.Exp(-System.Math.Pow((3 * distance) / a, 2));
                    break;
                default:
                    break;
            }
            return cij;
        }

        private double GetPointDistance(iTelluro.DataTools.Utility.GIS.Point p1, iTelluro.DataTools.Utility.GIS.Point p2)
        {
            double xspan = p1.X - p2.X;
            double yspan = p1.Y - p2.Y;
            return Math.Sqrt(xspan * xspan + yspan * yspan);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Random rm = new Random();
            int n = int.Parse(this.textBox2.Text);
            double[,] d = new double[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    d[i, j] = i * j + 1; //rm.NextDouble() * 10;
                }
            }
            MathNet.Numerics.Control.UseManaged();
            Matrix<double> mS = DenseMatrix.OfArray(d);
            Matrix<double> mD = mS.Inverse();
        }

        private void btnAddPoints_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 趋势面拟合
        /// </summary>
        /// <param name="resultList"></param>
        void Trend(List<iTelluro.DataTools.Utility.GIS.Point> resultList)
        {
            double z = 0;
            double x = 0;
            double y = 0;
            int length = resultList.Count;
            for (int i = 0; i < length; i++)
            {
                iTelluro.DataTools.Utility.GIS.Point p = resultList[i];
                for (int j = i + 1; j < length; j++)
                {
                    //iTelluro.DataTools.Utility.GIS.Point p2 = resultList[j];
                    //double distance = GetPointDistance(p, p2);
                    //if (distance > maxDistance)
                    //{
                    //    maxDistance = distance;
                    //}
                    //if (distance < minDistance)
                    //{
                    //    minDistance = distance;
                    //}
                    //points.Add(new RegressionPoint(distance, 0.5 * System.Math.Pow((p.Z - p2.Z), 2)));
                    x += p.X;
                    y += p.Y;
                    z += p.Z;
                }
            }

            //List<RegressionPoint> showPoints = new List<RegressionPoint>();
            //int h = (int)maxDistance / 10;//步长
            //for (int i = 0; i < 10; i++)
            //{
            //    List<RegressionPoint> tempPoints = Query(points, i * h, (i + 1) * h);
            //    if (tempPoints.Count > 0)
            //    {
            //        double values = 0;
            //        double distance = 0;
            //        foreach (RegressionPoint p in tempPoints)
            //        {
            //            distance += p.distance;
            //            values += p.semivariogram;
            //        }
            //        distance = distance / tempPoints.Count;//平均步长
            //        values = values / tempPoints.Count;//平均变异差
            //        showPoints.Add(new RegressionPoint(distance, values));
            //    }
            //}
            //x = x / 100000;
            //y = y / 100000;
            //z = z / 100000;
            /*三次趋势面数学模拟模型Zxy = b0 + b1x + b2y + b3x^2 + b4xy + b5y^2 + b6x^3 + b7x^2y + b8xy^2 + b9y^3*/
            int n = 3;
            double[,] leftKnowns = new double[n, n];
            leftKnowns[0, 0] = length;
            leftKnowns[0, 1] = x;
            leftKnowns[0, 2] = y;

            leftKnowns[1, 0] = x;
            leftKnowns[1, 1] = x * x;
            leftKnowns[1, 2] = x * y;

            leftKnowns[2, 0] = y;
            leftKnowns[2, 1] = y * x;
            leftKnowns[2, 2] = y * y;

            //double[,] leftKnowns = new double[3, 10];
            //leftKnowns[0, 0] = length;
            //leftKnowns[0, 1] = x;
            //leftKnowns[0, 2] = y;
            //leftKnowns[0, 3] = x * x;
            //leftKnowns[0, 4] = x * y;
            //leftKnowns[0, 5] = y * y;
            //leftKnowns[0, 6] = x * x * x;
            //leftKnowns[0, 7] = x * x * y;
            //leftKnowns[0, 8] = x * y * y;
            //leftKnowns[0, 9] = y * y * y;

            //leftKnowns[1, 0] = x;
            //leftKnowns[1, 1] = x * x;
            //leftKnowns[1, 2] = y * x;
            //leftKnowns[1, 3] = x * x * x;
            //leftKnowns[1, 4] = x * y * x;
            //leftKnowns[1, 5] = y * y * x;
            //leftKnowns[1, 6] = x * x * x * x;
            //leftKnowns[1, 7] = x * x * y * x;
            //leftKnowns[1, 8] = x * y * y * x;
            //leftKnowns[1, 9] = y * y * y * x;

            //leftKnowns[2, 0] = y;
            //leftKnowns[2, 1] = x * y;
            //leftKnowns[2, 2] = y * y;
            //leftKnowns[2, 3] = x * x * y;
            //leftKnowns[2, 4] = x * y * y;
            //leftKnowns[2, 5] = y * y * y;
            //leftKnowns[2, 6] = x * x * x * y;
            //leftKnowns[2, 7] = x * x * y * y;
            //leftKnowns[2, 8] = x * y * y * y;
            //leftKnowns[2, 9] = y * y * y * y;

            double[,] rightKnowns = new double[3, 1];
            rightKnowns[0, 0] = z;
            rightKnowns[1, 0] = z * x;
            rightKnowns[2, 0] = z * y;

            MathNet.Numerics.Control.UseManaged();

            Matrix<double> mLeft = DenseMatrix.OfArray(leftKnowns);
            Matrix<double> mRight = DenseMatrix.OfArray(rightKnowns);

            Matrix<double> kn = mLeft.Inverse();

            Matrix<double> mD = kn.Multiply(mRight);
        }

        double TrendCalij()
        {
            double Cij = 0;

            return Cij;
        }

        /// <summary>
        ///  样条法插值算法:适用范围温度，高程，地下水位，无人浓度等
        ///  计算公式:Zi =T(x,y) + A1*d1^2*logd1 + A2*d2^2*logd2 + ... + An*dn^2*logdn 
        /// </summary>
        /// <param name="resultList"></param>
        double Spline(List<iTelluro.DataTools.Utility.GIS.Point> resultList,iTelluro.DataTools.Utility.GIS.Point p)
        {
            double z = 0;
            int length = resultList.Count;
            double[,] leftKnowns = new double[resultList.Count, resultList.Count];
            double[,] rightKnowns = new double[resultList.Count,1];
            for (int i = 0; i < length; i++)
            {
                double d = 0;
                for (int j = 0; j < length; j++)
                {
                    if (i == j)
                    {
                        leftKnowns[i, j] = 0;
                    }
                    else
                    {
                        d = GetPointDistance(resultList[i], resultList[j]);
                        leftKnowns[i, j] = d * d * System.Math.Log(d);
                    }
                }
                d = GetPointDistance(resultList[i], p);
                rightKnowns[i, 0] = d * d * System.Math.Log(d);
            }
            Matrix<double> mLeft = DenseMatrix.OfArray(leftKnowns);
            Matrix<double> mRight = DenseMatrix.OfArray(rightKnowns);

            MathNet.Numerics.Control.UseManaged();

            Matrix<double> mInv = mLeft.Inverse();
            Matrix<double> mD = mInv * mRight;

            for(int i=0;i<mD.RowCount;i++)
            {
                z += mD[i, 0] * resultList[i].Z;
            }
            return z;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //读取点数据
            ShpReader shp = new ShpReader(this.textBox1.Text);
            Rect r = ShpReader.GetFirstGeoExtent(this.textBox1.Text);
            List<iTelluro.DataTools.Utility.GIS.Point> points = new List<iTelluro.DataTools.Utility.GIS.Point>();
            //获取四至信息
            Feature ofea;
            while (((ofea = shp.layer.GetNextFeature()) != null))
            {
                Geometry geo = ofea.GetGeometryRef();
                Envelope env = new Envelope();
                geo.GetEnvelope(env);
                if (env.MinX < minx)
                {
                    minx = env.MinX;
                }
                if (env.MinX > maxx)
                {
                    maxx = env.MinX;
                }
                if (env.MaxY > maxy)
                {
                    maxy = env.MaxY;
                }
                if (env.MaxY < miny)
                {
                    miny = env.MaxY;
                }

                points.Add(new iTelluro.DataTools.Utility.GIS.Point(env.MaxX, env.MaxY, ofea.GetFieldAsDouble("RASTERVALU")));
            }
            Trend(points);
        }
    }

    public enum KrigingModel
    {
        /// <summary>
        /// 球状模型
        /// </summary>
        Spherical,

        /// <summary>
        /// 圆状模型
        /// </summary>
        Circular,

        /// <summary>
        /// 指数模型
        /// </summary>
        Exponential,

        /// <summary>
        /// 高斯模型
        /// </summary>
        Gaussian,
    }

    ///*拟合计算参数a,b,c  r(h) = c0 + c*((3h/2a) - (h*h*h /2a*a*a))
    //公式简化为： r(h) = c0 + (3c/2a)*h - (c/2a*a*a)h*h*h
    // * 令 a = c0, b = 3c/2a ,c = -c/2*a*a*a , h=x1,h*h*h =x2;
    // * 即 y = a + b*x1 + c*x2
    // * 以此计算a,b,c 进而计算r(h)的表达式
    //*/
    //int sum = 0;
    //for (int i = 0; i < resultList.Count; i++)
    //{
    //    sum += i;
    //}
    //count = sum;
    ////左侧矩阵系数(两点之间距离与距离的3次方)
    //double[,] leftKnowns = new double[count, 3];

    ////右侧矩阵系数(高程值)
    //double[,] rightKnowns = new double[count, 1];
    //int index = 0;
    //for (int i = 0; i < resultList.Count; i++)
    //{
    //    iTelluro.DataTools.Utility.GIS.Point p = resultList[i];
    //    for (int j = i + 1; j < resultList.Count; j++)
    //    {
    //        iTelluro.DataTools.Utility.GIS.Point p2 = resultList[j];
    //        double dis = GetPointDistance(p, p2);
    //        leftKnowns[index, 0] = 1;
    //        leftKnowns[index, 1] = dis;
    //        leftKnowns[index, 2] = dis * dis * dis;
    //        rightKnowns[index,0] = 0.5 * System.Math.Pow((p.Z - p2.Z), 2);
    //        index++;
    //    }
    //}
    ////左侧矩阵
    //Matrix<double> mLeft = DenseMatrix.OfArray(leftKnowns);
    ////右侧矩阵
    //Matrix<double> mRight = DenseMatrix.OfArray(rightKnowns);

    ////矩阵求解方法solve
    //MathNet.Numerics.Control.UseManaged();
    //Matrix<double> rlt;

    //try
    //{
    //    rlt = mLeft.Solve(mRight);
    //}
    //catch (System.Exception ex)
    //{
    //    throw new Exception("拟合解算失败：" + ex.Message);
    //}

    ////得到未知数a、b、c
    //double a = rlt[0, 0];
    //double b = rlt[1, 0];
    //double c = rlt[2, 0];

    //double c0 = a;
    //double c2 = Math.Sqrt(-b / 3 * c);
    //double c1 = 2 * c2 * b / 3;

    //double[,] r = new double[resultList.Count + 1, resultList.Count + 1];
    //double[,] d = new double[resultList.Count + 1, 1];
    //double[,] w = new double[resultList.Count + 1, 1];


    //for (int i = 0; i < resultList.Count; i++)
    //{
    //    if (i != resultList.Count)
    //    {
    //        double s = 0;
    //        for (int j = 0; j < resultList.Count; j++)
    //        {
    //            s = GetPointDistance(resultList[i], resultList[j]);
    //            r[i, j] = c0 + c1 * (3 / 2 * (s / c2) - 1 / 2 * (s * s * s) / c2 * c2);
    //            if (j == resultList.Count) r[i, resultList.Count + 1] = 1;
    //        }
    //        s = GetPointDistance(resultList[0], resultList[i]);
    //        d[i, 0] = c0 + c1 * (3 / 2 * (s / c2) - 1 / 2 * (s * s * s) / c2 * c2);
    //    }
    //    else
    //    {
    //        for (int j = 0; j < resultList.Count; j++)
    //        {
    //            r[i, j] = 1;
    //            if (j == resultList.Count) r[i, resultList.Count] = 0;
    //        }
    //    }
    //}

    ////权重矩阵等于协方差矩阵的逆矩阵乘以D矩阵
    //r = InverseMatrix(r);
    // //协方差矩阵
    //Matrix<double> mR = DenseMatrix.OfArray(r);
    //mR.Inverse();
    ////D矩阵
    //Matrix<double> mD = DenseMatrix.OfArray(d);
    //Matrix<double> mW = mR * mD;
}
