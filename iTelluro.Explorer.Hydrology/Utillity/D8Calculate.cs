using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FloodPeakToolUI
{
    class D8Calculate
    {
        private int ncols;
        private int nrows;
        private double NODATA_value;
        private double[,] d3X3=new double[3,3];

        public D8Calculate(int nrows0, int ncols0, double NODATA_value0)
        {
            this.nrows = nrows0;
            this.ncols = ncols0;
            this.NODATA_value = NODATA_value0;
        }

        public void cal(double[,] dem,ref double[,] dir)
        {
            int i, j;
            int ii, jj;
            for (j = 0; j < nrows; j++)
            {
                for (i = 0; i < ncols; i++)
                {
                    for (jj = 0; jj < 3; jj++)
                    {
                        for (ii  = 0; ii < 3; ii++)
                        {
                            //set d3X3 data from DEM
                            if (ii - 1 + i >= 0 && ii - 1 + i < ncols && jj - 1 + j >= 0 && jj - 1 + j < nrows)
                            {
                                d3X3[ii, jj] = dem[i + ii - 1, j + jj - 1];
                            }
                            else 
                            {
                                d3X3[ii, jj] = NODATA_value;
                            }
                        }
                    }
                    dir[i, j] = cal3X3();
                }
            }
        }

        private int cal3X3()
        {
            int ii, jj;
            int ii0 = 0, jj0 = 0, ii1 = 0, jj1 = 0;
            int ncorner;//判断无效点数
            int nn, loc;
            double k = 1 / Math.Sqrt(2);
            double h0=d3X3[1,1];
            double low = 0, high = 0;
            double val = 0;
            nn = 0; loc = 0;

            ncorner = 0;
            for (jj = 0; jj < 3; jj++)
            {
                for (ii = 0; ii < 3; ii++)
                {
                    //忽视无效的值
                    if(d3X3[ii,jj]<=NODATA_value+0.1)
                    {
                        ncorner++;//当无效点数为5时，表示为角点
                        continue;
                    }
                    if(ii==1&&jj==1)//中心点
                    {
                        continue;
                    }
                    if (ii == jj || Math.Abs(ii - jj) == 2)
                    {
                        val = (h0 - d3X3[ii, jj]) * k;//角点
                    }
                    else 
                    {
                        val = (h0 - d3X3[ii, jj]);
                    }

                    //查找最低的点
                    if (val < low || nn == 0)
                    {
                        low = val;
                        ii0 = ii;
                        jj0 = jj;
                    }
                    //查找最高点
                    if (val > high || nn == 0) 
                    {
                        high = val;
                        ii1 = ii;
                        jj1 = jj;
                    }
                    nn++;
                }
            }
            //当中心点是最低点，因洼池填平已处理，但现在可以判断是DEM边缘
            if (low < 0)
            {
                ii0 = 2 - ii1;
                jj0 = 2 - jj1;
                //除了四个角可以有斜向的流向外，边缘部分的外流流向现定义为垂直向外
                if (ncorner < 5) 
                {
                    if (d3X3[1, 0] < NODATA_value + 0.1) 
                    {
                        ii0 = 1;
                        jj0 = 0;
                    }
                    else if (d3X3[2, 1] < NODATA_value + 0.1)
                    {
                        ii0 = 2;
                        jj0 = 1;
                    }
                    else if (d3X3[1, 2] < NODATA_value + 0.1)
                    {
                        ii0 = 1;
                        jj0 = 2;
                    }
                    else if (d3X3[0, 1] < NODATA_value + 0.1)
                    {
                        ii0 = 0;
                        jj0 = 1;
                    }
                }
            }

            //单元格设置流向
            if (ii0 == 2 && jj0 == 1) 
            {
                loc = 1;
            }
            else if (ii0 == 2 && jj0 == 2)
            {
                loc = 2;
            }
            else if (ii0 == 1 && jj0 == 2)
            {
                loc = 4;
            }
            else if (ii0 == 0 && jj0 == 2)
            {
                loc = 8;
            }
            else if (ii0 == 0 && jj0 == 1)
            {
                loc = 16;
            }
            else if (ii0 == 0 && jj0 == 0)
            {
                loc = 32;
            }
            else if (ii0 == 1 && jj0 == 0)
            {
                loc = 64;
            }
            else if (ii0 == 2 && jj0 == 0)
            {
                loc = 128;
            }
            return (loc);
        }

        //遍历流向数据的单元格，计算流向这个单元格的所有单元个数
        //public void GetFlowCell(double[,] dir)
        //{
        //    int i1, j1;
        //    for (int i = 0; i < nrows; i++)
        //    {
        //        for (int j = 0; j < ncols; j++)
        //        {
        //            i1 = i;
        //            j1 = j;
        //        }
        //    }
        //}
    }
}
