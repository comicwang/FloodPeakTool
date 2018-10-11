using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FloodPeakUtility.Model
{
    /// <summary>
    /// 网格数据结构
    /// </summary>
    public class MyGrid
    {
        /// <summary>
        /// 栅格原数据
        /// </summary>
        private static double[,] src;

        //行
        private int row = 0;

        //列
        private int col = 0;

        //行数
        private static int rowCount = 0;

        //列数
        private static int colCount = 0;

        //是否填洼
        public bool IsFill = false;

        //流向值
        public int Direction = 0;

        //当前网格高程值
        public double ALT = 0;

        //可能的流向网格
        public List<MyGrid> MaybeDirections = null;

        //周围最大流向差值
        public double MaxDirection = 0;

        //周围最低流向差值
        public double MinDirection = 0;

        public double MaxAlt = 0;

        public double MinAlt = 0;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        public MyGrid(int i, int j)
        {
            this.row = i;
            this.col = j;
            ALT = Src[i, j];
        }

        //原始数据
        public static double[,] Src
        {
            get => src;
            set
            {
                src = value;
                rowCount = value.GetLength(0);
                colCount = value.GetLength(1);
            }
        }

        /// <summary>
        /// 重新计算值
        /// </summary>
        public void Caculate()
        {
            ALT = Src[row, col];
            double? S, N, E, SE, NE, NW, W, SW;
            double M;

            double Sqrt2 = Math.Sqrt(2);
            S = null;
            N = null;
            E = null;
            SE = null;
            NE = null;
            NW = null;
            W = null;
            SW = null;
            if ((row != (rowCount - 1)))
                S = (Src[row, col] - Src[row + 1, col]);
            if (row != (rowCount - 1) && col != (colCount - 1))
                SE = (Src[row, col] - Src[row + 1, col + 1]) / Sqrt2;
            if ((row != 0))
                N = (Src[row, col] - Src[row - 1, col]);
            if (col != (colCount - 1))
                E = (Src[row, col] - Src[row, col + 1]);
            if (row != 0 && col != (colCount - 1))
                NE = (Src[row, col] - Src[row - 1, col + 1]) / Sqrt2;
            if ((row != 0 && col != 0))
                NW = (Src[row, col] - Src[row - 1, col - 1]) / Sqrt2;
            if (col != 0)
                W = (Src[row, col] - Src[row, col - 1]);
            if (row != (rowCount - 1) && col != 0)
                SW = (Src[row, col] - Src[row + 1, col - 1]) / Sqrt2;

            //判断其中最小值
            M = new List<double?>() { S, SE, N, E, NE, NW, W, SW }.Where(t => t != null).Min().Value;

            MinDirection = M;

            MaxDirection= new List<double?>() { S, SE, N, E, NE, NW, W, SW }.Where(t => t != null).Max().Value;

            ////取最大的坡降
            int flow = 0;
            List<MyGrid> lstDirections = new List<MyGrid>();

            if (M == S)
            {
                flow += 4;
                lstDirections.Add(new MyGrid(row + 1, col));
            }
            if (M == SE)
            {
                flow += 2;
                lstDirections.Add(new MyGrid(row + 1, col + 1));
            }
            if (M == N)
            {
                flow += 64;
                lstDirections.Add(new MyGrid(row - 1, col));
            }
            if (M == E)
            {
                flow += 1;
                lstDirections.Add(new MyGrid(row, col + 1));
            }
            if (M == NE)
            {
                flow += 128;
                lstDirections.Add(new MyGrid(row - 1, col + 1));
            }
            if (M == NW)
            {
                flow += 32;
                lstDirections.Add(new MyGrid(row - 1, col - 1));
            }
            if (M == W)
            {
                flow += 16;
                lstDirections.Add(new MyGrid(row, col - 1));
            }
            if (M == SW)
            {
                flow += 8;
                lstDirections.Add(new MyGrid(row + 1, col - 1));
            }
            this.Direction = flow;
            IsFill = lstDirections.Count() == 1;
        }

        /// <summary>
        /// 填充洼地
        /// </summary>
        /// <returns></returns>
        public double Fill()
        {
            double min = MaybeDirections.Select(t => t.MinDirection).Min();
            List<MyGrid> minGrids = MaybeDirections.Where(t => t.MinDirection == min).ToList();
            if (minGrids.Count == 1)
            {
                return 0;
            }
            return 0;
        }

        /// <summary>
        /// 获取原始数据的网格数据集合
        /// </summary>
        /// <returns></returns>
        public static List<MyGrid> GetSourceGrids()
        {
            List<MyGrid> result = new List<MyGrid>();
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    result.Add(new MyGrid(i, j));
                }
            }
            return result;
        }
    }
}
