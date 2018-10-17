using FloodPeakUtility.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FloodPeakUtility.Algorithm
{
    /// <summary>
    /// 河网提取算法
    /// </summary>
    public static class Hydrographic
    {
        /// <summary>
        /// 1．洼地填平 
        /// 洼地（水流积聚地）有真是洼地和数据精度不够高所造成的洼地。
        /// 洼地填平的主要作用是避免DEM的精度不够高所产生的（假的）水流积聚地。
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static double[,] Fill(this double[,] src)
        {
            int rowCount = src.GetLength(0);
            int colCount = src.GetLength(1);
            double[,] result = new double[rowCount, colCount];

            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < colCount; col++)
                {
                    if (row > rowCount - 1 || col > colCount - 1 || row < 0 || col < 0)
                    {
                        throw new ArgumentException("传入参数错误!");
                    }
                    double? S, N, E, SE, NE, NW, W, SW;
                    double M;
                    GetAround(rowCount, colCount, row, col, src, out S, out N, out E, out SE, out NE, out NW, out W, out SW, out M);

                    //填洼
                    if (M < 0)
                    {
                        result[row, col] = src[row, col] - M;
                    }
                    else
                    {
                        result[row, col] = src[row, col];
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 2．水流方向计算 
        /// 水流方向计算就可以使用上一步所生成的DEM为源数据了
        ///（如果使用未经洼地填平处理的数据，可能会造成精度下降）
        /// 算法说明：计算某个单元格附近八个（不足的不考虑）的最小高程（最大高程差）
        /// 结果说明：若有一个最小的，流向指向最小的；若有多个最小的继续分别计算几个最小值附近的最小值
        /// 水流方向计算之后返回的是1-128表示流向的值
        ///一直递归找到最小值的那一项 </summary>
        /// <param name="src"></param>
        /// <returns></returns>

        public static double[,] FlowDirection(this double[,] src)
        {
            int rowCount = src.GetLength(0);
            int colCount = src.GetLength(1);
            double[,] result = new double[rowCount, colCount];

            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < colCount; col++)
                {
                    result[row, col] = CalFlowDirection(rowCount, colCount, row, col, src);
                    FormCalView.SetColor(row, col, (int)result[row, col]);
                }
            }

            return result;
        }

        /// <summary>
        /// 3．水流积聚计算 
        /// 可以看到，生成的水流积聚栅格已经可以看到所产生的河网了。
        /// 现在所需要做的就是把这些河网栅格提取出来。可以把产生的河网的支流的象素值作为阀值来提取河网栅格。
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>

        public static double[,] Accumulation(this double[,] src)
        {
            int rowCount = src.GetLength(0);
            int colCount = src.GetLength(1);
            double[,] result = new double[rowCount, colCount];  //定义结果


            Dictionary<Grid, List<Grid>> dicCaledAll = new Dictionary<Grid, List<Grid>>();
            Queue<Grid> queueGrid = new Queue<Grid>();
            Dictionary<Grid, int> unFindOutGrid = new Dictionary<Grid, int>();


            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < colCount; col++)
                {
                    //将数据添加到方程组中
                    Grid tempGrid = new Grid(row, col);
                    queueGrid.Enqueue(tempGrid);
                    unFindOutGrid.Add(tempGrid, 0);  //添加默认为零
                    dicCaledAll.Add(tempGrid, GetFlowAround(src, tempGrid));
                }
            }

            Dictionary<Grid, int> findOutGrid = new Dictionary<Grid, int>();
            FormCalView.SetAllSize(rowCount, colCount);

            FormOutput.AppendProress(true);
            //开始解方程
            while (queueGrid.Count > 0)
            {
                FormOutput.AppendProress(findOutGrid.Count * 100 / dicCaledAll.Count);
                Grid tGrid = queueGrid.Dequeue();
                List<Grid> lstGrid = dicCaledAll[tGrid];
                //找到数据
                if (lstGrid.Count == 0)
                {
                    result[tGrid.i, tGrid.j] = unFindOutGrid[tGrid];
                    findOutGrid.Add(tGrid, unFindOutGrid[tGrid]);

                    dicCaledAll.Remove(tGrid);
                    //unFindOutGrid.Remove(tGrid);
                    FormCalView.SetColor(tGrid.i, tGrid.j, unFindOutGrid[tGrid]);
                }
                else
                {
                    List<Grid> lstTemp =new List<Grid>();
                    lstTemp.AddRange(dicCaledAll[tGrid]);
                    //循环寻找当前找出的Grid
                    for (int i = 0; i < lstGrid.Count; i++)
                    {
                        Grid temp = lstGrid[i];
                        if (ContainsGrid(findOutGrid,temp))
                        {
                            Grid temp1 = FindGrid(unFindOutGrid, temp);
                            Grid temp2 = FindGrid(findOutGrid, temp);
                            unFindOutGrid[temp1] = unFindOutGrid[temp1] + findOutGrid[temp2] + 1;
                            lstTemp.Remove(temp);
                        }
                    }

                    if (lstTemp.Count == 0)
                    {
                        result[tGrid.i, tGrid.j] = unFindOutGrid[tGrid];
                        findOutGrid.Add(tGrid, unFindOutGrid[tGrid]);

                        dicCaledAll.Remove(tGrid);
                        // unFindOutGrid.Remove(tGrid);
                        FormCalView.SetColor(tGrid.i, tGrid.j, unFindOutGrid[tGrid]);
                    }

                    else
                    {
                        dicCaledAll[tGrid] = lstTemp;

                        queueGrid.Enqueue(tGrid);
                    }
                }
            }
            return result;
        }

        private static bool ContainsGrid(Dictionary<Grid, int> source,Grid key)
        {
            if (source.Count==0)
            {
                return false;
            }
            foreach (var item in source)
            {
                if (item.Key.i==key.i&&item.Key.j==key.j)
                {
                    return true;
                }
            }
            return false;
        }

        private static Grid FindGrid(Dictionary<Grid, int> source, Grid key)
        {
            if (source.Count == 0)
            {
                return null;
            }
            foreach (var item in source)
            {
                if (item.Key.i == key.i && item.Key.j == key.j)
                {
                    return item.Key;
                }
            }
            return null;
        }

        /// <summary>
        /// 计算特定位置像元水流方向(根据高程差)
        /// 算法说明：计算某个单元格附近八个（不足的不考虑）的最小高程（最大高程差）
        /// 结果说明：若有一个最小的，流向指向最小的；若有多个最小的继续分别计算几个最小值附近的最小值
        /// 一直递归找到最小值的那一项
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="col">列</param>
        /// <returns></returns>
        private static int CalFlowDirection(int rowCount, int colCount, int row, int col, double[,] src)
        {
            double? S, N, E, SE, NE, NW, W, SW;
            double M;
            GetAround(rowCount, colCount, row, col, src, out S, out N, out E, out SE, out NE, out NW, out W, out SW, out M);

            ////取最大的坡降
            Dictionary<Grid, int> dicMin = new Dictionary<Grid, int>();
            if (M == S)
            {
                dicMin.Add(new Grid(row + 1, col), 4);
            }
            if (M == SE)
            {
                dicMin.Add(new Grid(row + 1, col + 1), 2);
            }
            if (M == N)
            {
                dicMin.Add(new Grid(row - 1, col), 64);
            }
            if (M == E)
            {
                dicMin.Add(new Grid(row, col + 1), 1);
            }
            if (M == NE)
            {
                dicMin.Add(new Grid(row - 1, col + 1), 128);
            }
            if (M == NW)
            {
                dicMin.Add(new Grid(row - 1, col - 1), 32);
            }
            if (M == W)
            {
                dicMin.Add(new Grid(row, col - 1), 16);
            }
            if (M == SW)
            {
                dicMin.Add(new Grid(row + 1, col - 1), 8);
            }
            if (dicMin.Count == 1)
            {
                return dicMin.Values.FirstOrDefault();
            }
            else
            {
                double m = double.MinValue;
                Grid m_Grid = null;
                foreach (var item in dicMin)
                {
                    GetAround(rowCount, colCount, item.Key.i, item.Key.j, src, out S, out N, out E, out SE, out NE, out NW, out W, out SW, out M);
                    if (m < M)
                    {
                        m = M;
                        m_Grid = item.Key;
                    }
                }
                return dicMin[m_Grid];
            }
        }

        /// <summary>
        /// 计算单元格周围8格数据
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="colCount"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="src"></param>
        /// <param name="S"></param>
        /// <param name="N"></param>
        /// <param name="E"></param>
        /// <param name="SE"></param>
        /// <param name="NE"></param>
        /// <param name="NW"></param>
        /// <param name="W"></param>
        /// <param name="SW"></param>
        /// <param name="M"></param>
        private static void GetAround(int rowCount, int colCount, int row, int col, double[,] src, out double? S, out double? N, out double? E, out double? SE, out double? NE, out double? NW, out double? W, out double? SW, out double M)
        {
            double Sqrt2 = Math.Sqrt(2);
            S = -1;
            N = -1;
            E = -1;
            SE = -1;
            NE = -1;
            NW = -1;
            W = -1;
            SW = -1;
            if ((row != (rowCount - 1)))
                S = (src[row, col] - src[row + 1, col]);
            if (row != (rowCount - 1) && col != (colCount - 1))
                SE = (src[row, col] - src[row + 1, col + 1]) / Sqrt2;
            if ((row != 0))
                N = (src[row, col] - src[row - 1, col]);
            if (col != (colCount - 1))
                E = (src[row, col] - src[row, col + 1]);
            if (row != 0 && col != (colCount - 1))
                NE = (src[row, col] - src[row - 1, col + 1]) / Sqrt2;
            if ((row != 0 && col != 0))
                NW = (src[row, col] - src[row - 1, col - 1]) / Sqrt2;
            if (col != 0)
                W = (src[row, col] - src[row, col - 1]);
            if (row != (rowCount - 1) && col != 0)
                SW = (src[row, col] - src[row + 1, col - 1]) / Sqrt2;

            //判断其中最小值
            M = new List<double?>() { S, SE, N, E, NE, NW, W, SW }.Where(t => t != null && t >= 0).Min().HasValue ? new List<double?>() { S, SE, N, E, NE, NW, W, SW }.Where(t => t != null && t >= 0).Min().Value : new List<double?>() { S, SE, N, E, NE, NW, W, SW }.Where(t => t != null).Max().Value;
        }

        /// <summary>
        /// 获取当前单元格指定方向的单元格流向它
        /// </summary>
        /// <param name="src"></param>
        /// <param name="index"></param>
        /// <param name="direct"></param>
        /// <returns></returns>
        private static List<Grid> GetFlowAround(double[,] src, Grid index)
        {
            List<Grid> result = new List<Grid>();
            double x;
            //判断边界
            if (index.i - 1 >= 0)
            {
                x = src[index.i - 1, index.j];
                if (x == 1)
                {
                    result.Add(new Grid(index.i - 1, index.j));
                }
            }

            //判断边界
            if (index.i - 1 >= 0 && index.j - 1 >= 0)
            {
                x = src[index.i - 1, index.j - 1];
                if (x == 2)
                {
                    result.Add(new Grid(index.i - 1, index.j - 1));
                }
            }

            //判断边界
            if (index.j - 1 >= 0)
            {
                x = src[index.i, index.j - 1];
                if (x == 4)
                {
                    result.Add(new Grid(index.i, index.j - 1));
                }
            }

            //判断边界
            if (index.i + 1 < src.GetLength(0) && index.j - 1 >= 0)
            {
                x = src[index.i + 1, index.j - 1];
                if (x == 8)
                {
                    result.Add(new Grid(index.i + 1, index.j - 1));
                }
            }

            //判断边界
            if (index.i + 1 < src.GetLength(0))
            {
                x = src[index.i + 1, index.j];
                if (x == 16)
                {
                    result.Add(new Grid(index.i + 1, index.j));
                }
            }

            //判断边界
            if (index.i + 1 < src.GetLength(0) && index.j + 1 < src.GetLength(1))
            {
                x = src[index.i + 1, index.j + 1];
                if (x == 32)
                {
                    result.Add(new Grid(index.i + 1, index.j + 1));
                }
            }

            //判断边界
            if (index.j + 1 < src.GetLength(1))
            {
                x = src[index.i, index.j + 1];
                if (x == 64)
                {
                    result.Add(new Grid(index.i, index.j + 1));
                }
            }

            //判断边界
            if (index.i - 1 >= 0 && index.j + 1 < src.GetLength(1))
            {
                x = src[index.i - 1, index.j + 1];
                if (x == 128)
                {
                    result.Add(new Grid(index.i - 1, index.j + 1));
                }
            }

            return result;
        }
    }

    public class Grid
    {
        public int i;
        public int j;
        public Grid(int i, int j)
        {
            this.i = i;
            this.j = j;
        }
        public Grid()
        {
            i = j = 0;
        }

        public override string ToString()
        {
            return string.Format("i={0},j={1}", i, j);
        }
    }

    public enum Direct
    {
        left = 1,//左
        leftTop = 2,//左上
        top = 4,//上
        rightTop = 8,//右上
        right = 16,//右
        rigthBottom = 32,//右下
        bottom = 64,//下
        leftBottom = 128//左下
    }
}
