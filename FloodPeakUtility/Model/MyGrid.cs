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

        private static List<MyGrid> CaledGrid = null;

        //行
        private int row = 0;

        //列
        private int col = 0;

        //行数
        private static int rowCount = 0;

        //列数
        private static int colCount = 0;

        //当前网格高程值
        public double ALT = 0;

        /// <summary>
        /// 获取多流向的可能流向网格
        /// </summary>
        public List<MyGrid> MaybeDirections
        {
            get
            {
                double? S, N, E, SE, NE, NW, W, SW;
               
                double Sqrt2 = Math.Sqrt(2);
                S = null;
                N = null;
                E = null;
                SE = null;
                NE = null;
                NW = null;
                W = null;
                SW = null;
                if ((Row != (rowCount - 1)))
                    S = (Src[Row, Col] - Src[Row + 1, Col]);
                if (Row != (rowCount - 1) && Col != (colCount - 1))
                    SE = (Src[Row, Col] - Src[Row + 1, Col + 1]) / Sqrt2;
                if ((Row != 0))
                    N = (Src[Row, Col] - Src[Row - 1, Col]);
                if (Col != (colCount - 1))
                    E = (Src[Row, Col] - Src[Row, Col + 1]);
                if (Row != 0 && Col != (colCount - 1))
                    NE = (Src[Row, Col] - Src[Row - 1, Col + 1]) / Sqrt2;
                if ((Row != 0 && Col != 0))
                    NW = (Src[Row, Col] - Src[Row - 1, Col - 1]) / Sqrt2;
                if (Col != 0)
                    W = (Src[Row, Col] - Src[Row, Col - 1]);
                if (Row != (rowCount - 1) && Col != 0)
                    SW = (Src[Row, Col] - Src[Row + 1, Col - 1]) / Sqrt2;

                List<double?> D8 = new List<double?>() { S, SE, N, E, NE, NW, W, SW };
                //判断其中最大降值
                double M = D8.Where(t => t.HasValue&&t>=0).Max().Value;

                List<MyGrid> lstDirections = new List<MyGrid>();

                if (M == S)
                {
                    lstDirections.Add(new MyGrid(Row + 1, Col));
                }
                if (M == SE)
                {
                    lstDirections.Add(new MyGrid(Row + 1, Col + 1));
                }
                if (M == N)
                {
                    lstDirections.Add(new MyGrid(Row - 1, Col));
                }
                if (M == E)
                {
                    lstDirections.Add(new MyGrid(Row, Col + 1));
                }
                if (M == NE)
                {
                    lstDirections.Add(new MyGrid(Row - 1, Col + 1));
                }
                if (M == NW)
                {
                    lstDirections.Add(new MyGrid(Row - 1, Col - 1));
                }
                if (M == W)
                {                  
                    lstDirections.Add(new MyGrid(Row, Col - 1));
                }
                if (M == SW)
                {
                    lstDirections.Add(new MyGrid(Row + 1, Col - 1));
                }
                return lstDirections;
            }
        }

        //周围最大流向差值
        public double MaxDirection = 0;

        //周围最低流向差值
        public double MinDirection = 0;

        /// <summary>
        /// 获取网格的最大有效降值
        /// </summary>
        public double MaxReducedValue
        {
            get
            {
                double? S, N, E, SE, NE, NW, W, SW;

                double Sqrt2 = Math.Sqrt(2);
                S = null;
                N = null;
                E = null;
                SE = null;
                NE = null;
                NW = null;
                W = null;
                SW = null;
                if ((Row != (rowCount - 1)))
                    S = (Src[Row, Col] - Src[Row + 1, Col]);
                if (Row != (rowCount - 1) && Col != (colCount - 1))
                    SE = (Src[Row, Col] - Src[Row + 1, Col + 1]) / Sqrt2;
                if ((Row != 0))
                    N = (Src[Row, Col] - Src[Row - 1, Col]);
                if (Col != (colCount - 1))
                    E = (Src[Row, Col] - Src[Row, Col + 1]);
                if (Row != 0 && Col != (colCount - 1))
                    NE = (Src[Row, Col] - Src[Row - 1, Col + 1]) / Sqrt2;
                if ((Row != 0 && Col != 0))
                    NW = (Src[Row, Col] - Src[Row - 1, Col - 1]) / Sqrt2;
                if (Col != 0)
                    W = (Src[Row, Col] - Src[Row, Col - 1]);
                if (Row != (rowCount - 1) && Col != 0)
                    SW = (Src[Row, Col] - Src[Row + 1, Col - 1]) / Sqrt2;

                List<double?> D8 = new List<double?>() { S, SE, N, E, NE, NW, W, SW };

                return D8.Where(t => t.HasValue && t.Value >= 0).Max().Value;
            }
        }

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
        /// 获取单元格是否为洼地-true非洼地
        /// </summary>
        public bool IsFill
        {
            get
            {
                double? S, N, E, SE, NE, NW, W, SW;
 
                double Sqrt2 = Math.Sqrt(2);
                S = null;
                N = null;
                E = null;
                SE = null;
                NE = null;
                NW = null;
                W = null;
                SW = null;
                if ((Row != (rowCount - 1)))
                    S = (Src[Row, Col] - Src[Row + 1, Col]);
                if (Row != (rowCount - 1) && Col != (colCount - 1))
                    SE = (Src[Row, Col] - Src[Row + 1, Col + 1]) / Sqrt2;
                if ((Row != 0))
                    N = (Src[Row, Col] - Src[Row - 1, Col]);
                if (Col != (colCount - 1))
                    E = (Src[Row, Col] - Src[Row, Col + 1]);
                if (Row != 0 && Col != (colCount - 1))
                    NE = (Src[Row, Col] - Src[Row - 1, Col + 1]) / Sqrt2;
                if ((Row != 0 && Col != 0))
                    NW = (Src[Row, Col] - Src[Row - 1, Col - 1]) / Sqrt2;
                if (Col != 0)
                    W = (Src[Row, Col] - Src[Row, Col - 1]);
                if (Row != (rowCount - 1) && Col != 0)
                    SW = (Src[Row, Col] - Src[Row + 1, Col - 1]) / Sqrt2;

                List<double?> D8 = new List<double?>() { S, SE, N, E, NE, NW, W, SW };

                //判断是否为洼地
                return D8.Where(t => t.HasValue && t.Value >= 0).Count() > 0;
            }
        }

        /// <summary>
        /// 获取洼地需要填充的值
        /// </summary>
        public double FilledALT
        {
            get
            {
                double result = 0;
                double? S, N, E, SE, NE, NW, W, SW;

                double M = double.MinValue;
                double Sqrt2 = Math.Sqrt(2);
                S = null;
                N = null;
                E = null;
                SE = null;
                NE = null;
                NW = null;
                W = null;
                SW = null;
                if ((Row != (rowCount - 1)))
                {
                    S = (Src[Row, Col] - Src[Row + 1, Col]);
                    if (S > M)
                    {
                        M = S.Value;
                        result = Src[Row + 1, Col];
                    }
                }
                if (Row != (rowCount - 1) && Col != (colCount - 1))
                {
                    SE = (Src[Row, Col] - Src[Row + 1, Col + 1]) / Sqrt2;
                    if (SE > M)
                    {
                        M = SE.Value;
                        result = Src[Row + 1, Col + 1];
                    }
                }
                if ((Row != 0))
                {
                    N = (Src[Row, Col] - Src[Row - 1, Col]);
                    if (N > M)
                    {
                        M = N.Value;
                        result = Src[Row - 1, Col];
                    }
                }
                if (Col != (colCount - 1))
                {
                    E = (Src[Row, Col] - Src[Row, Col + 1]);
                    if (E > M)
                    {
                        M = E.Value;
                        result = Src[Row, Col + 1];
                    }
                }
                if (Row != 0 && Col != (colCount - 1))
                {
                    NE = (Src[Row, Col] - Src[Row - 1, Col + 1]) / Sqrt2;
                    if (NE > M)
                    {
                        M = NE.Value;
                        result = Src[Row - 1, Col + 1];
                    }
                }
                if ((Row != 0 && Col != 0))
                {
                    NW = (Src[Row, Col] - Src[Row - 1, Col - 1]) / Sqrt2;
                    if (NW > M)
                    {
                        M = NW.Value;
                        result = Src[Row - 1, Col - 1];
                    }
                }
                if (Col != 0)
                {
                    W = (Src[Row, Col] - Src[Row, Col - 1]);
                    if (W > M)
                    {
                        M = W.Value;
                        result = Src[Row, Col - 1];
                    }
                }
                if (Row != (rowCount - 1) && Col != 0)
                {
                    SW = (Src[Row, Col] - Src[Row + 1, Col - 1]) / Sqrt2;
                    if (SW > M)
                    {
                        M = SW.Value;
                        result = Src[Row + 1, Col - 1];
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// 获取网格是否为平地多流向
        /// </summary>
        public bool IsFlat
        {
            get
            {
                double? S, N, E, SE, NE, NW, W, SW;

                double Sqrt2 = Math.Sqrt(2);
                S = null;
                N = null;
                E = null;
                SE = null;
                NE = null;
                NW = null;
                W = null;
                SW = null;
                if ((Row != (rowCount - 1)))
                    S = (Src[Row, Col] - Src[Row + 1, Col]);
                if (Row != (rowCount - 1) && Col != (colCount - 1))
                    SE = (Src[Row, Col] - Src[Row + 1, Col + 1]) / Sqrt2;
                if ((Row != 0))
                    N = (Src[Row, Col] - Src[Row - 1, Col]);
                if (Col != (colCount - 1))
                    E = (Src[Row, Col] - Src[Row, Col + 1]);
                if (Row != 0 && Col != (colCount - 1))
                    NE = (Src[Row, Col] - Src[Row - 1, Col + 1]) / Sqrt2;
                if ((Row != 0 && Col != 0))
                    NW = (Src[Row, Col] - Src[Row - 1, Col - 1]) / Sqrt2;
                if (Col != 0)
                    W = (Src[Row, Col] - Src[Row, Col - 1]);
                if (Row != (rowCount - 1) && Col != 0)
                    SW = (Src[Row, Col] - Src[Row + 1, Col - 1]) / Sqrt2;

                List<double?> D8 = new List<double?>() { S, SE, N, E, NE, NW, W, SW };

                double maxValue = D8.Where(t => t.HasValue && t >= 0).Max().Value;

                return D8.Where(t => t.HasValue && t.Value == maxValue).Count() > 1;
            }
        }

        /// <summary>
        /// 获取网格的流向值
        /// </summary>
        public int Direction
        {
            get
            {
                if (IsFlat == false)
                {
                    double? S, N, E, SE, NE, NW, W, SW;

                    double Sqrt2 = Math.Sqrt(2);
                    S = null;
                    N = null;
                    E = null;
                    SE = null;
                    NE = null;
                    NW = null;
                    W = null;
                    SW = null;
                    if ((Row != (rowCount - 1)))
                        S = (Src[Row, Col] - Src[Row + 1, Col]);
                    if (Row != (rowCount - 1) && Col != (colCount - 1))
                        SE = (Src[Row, Col] - Src[Row + 1, Col + 1]) / Sqrt2;
                    if ((Row != 0))
                        N = (Src[Row, Col] - Src[Row - 1, Col]);
                    if (Col != (colCount - 1))
                        E = (Src[Row, Col] - Src[Row, Col + 1]);
                    if (Row != 0 && Col != (colCount - 1))
                        NE = (Src[Row, Col] - Src[Row - 1, Col + 1]) / Sqrt2;
                    if ((Row != 0 && Col != 0))
                        NW = (Src[Row, Col] - Src[Row - 1, Col - 1]) / Sqrt2;
                    if (Col != 0)
                        W = (Src[Row, Col] - Src[Row, Col - 1]);
                    if (Row != (rowCount - 1) && Col != 0)
                        SW = (Src[Row, Col] - Src[Row + 1, Col - 1]) / Sqrt2;

                    double M = new List<double?>() { S, SE, N, E, NE, NW, W, SW }.Max().Value;

                    if (M == S)
                    {
                        return 4;
                    }
                    if (M == SE)
                    {
                        return 2;
                    }
                    if (M == N)
                    {
                        return 64;
                    }
                    if (M == E)
                    {
                        return 1;
                    }
                    if (M == NE)
                    {
                        return 128;
                    }
                    if (M == NW)
                    {
                        return 32;
                    }
                    if (M == W)
                    {
                        return 16;
                    }
                    if (M == SW)
                    {
                        return 8;
                    }
                    return 0;
                }
                else
                {
                    CaledGrid = new List<MyGrid>();
                    MyGrid resultGrid = CalMultiDirection(MaybeDirections);
                    return CompareDirection(resultGrid);
                }
            }
        }

        /// <summary>
        /// 获取行
        /// </summary>
        public int Row { get => row; }

        /// <summary>
        /// 获取列
        /// </summary>
        public int Col { get => col; }

        /// <summary>
        /// 重新计算值
        /// </summary>
        public void Caculate()
        {
            ALT = Src[Row, Col];
           
        }

        /// <summary>
        /// 计算多流向指向的网格（迭代算法）
        /// </summary>
        /// <param name="lstGrid"></param>
        /// <returns></returns>

        private MyGrid CalMultiDirection(List<MyGrid> lstGrid)
        {
            CaledGrid.AddRange(lstGrid);
            double maxReduce = lstGrid.Select(t => t.MaxReducedValue).Max();

            //计算周围降值最大的单元格
            List<MyGrid> result = lstGrid.Where(t => t.MaxReducedValue == maxReduce).ToList();

            //只有一个，取到了流向网格
            if (result.Count == 1)
            {
                return result.FirstOrDefault();
            }
            //有多条，取出下级多条网格列表，迭代计算
            else
            {
                List<MyGrid> lstNextLvlGrid = new List<MyGrid>();
                foreach (var grid in result)
                {
                    List<MyGrid> lstTempGrid = grid.MaybeDirections;
                    foreach (var item in lstTempGrid)
                    {
                        if (!ContainsGrid(lstNextLvlGrid, item) && !ContainsGrid(CaledGrid, item))
                        {
                            lstNextLvlGrid.Add(item);
                        }
                    }
                }
                if (lstNextLvlGrid.Count == 0)
                {
                    return result.FirstOrDefault();  //去掉无限递归的数据
                }
                MyGrid resultGrid = CalMultiDirection(lstNextLvlGrid);
                ////获取结果Grid所属的根Grid
                return GetNextLvlGrid(result, resultGrid);
            }
        }

        /// <summary>
        /// 是否包含列
        /// </summary>
        /// <param name="lst"></param>
        /// <param name="grid"></param>
        /// <returns></returns>
        private bool ContainsGrid(List<MyGrid> lst, MyGrid grid)
        {
            return lst.Where(t => t.Row == grid.Row && t.Col == grid.Col).Any();
        }

        /// <summary>
        /// 获取下级所属的网格
        /// </summary>
        /// <param name="lst"></param>
        /// <param name="grid"></param>
        /// <returns></returns>
        private MyGrid GetNextLvlGrid(List<MyGrid> lst, MyGrid grid)
        {
            foreach (var item in lst)
            {
                if ((Math.Abs(item.Row - grid.Row) == 0 || Math.Abs(item.Row - grid.Row) == 1) &&
                    (Math.Abs(item.Col - grid.Col) == 0 || Math.Abs(item.Col - grid.Col) == 1))
                {
                    return item;
                }
            }
            return null;
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

        /// <summary>
        /// 比较2个网格，判断网格相对位置和流向值
        /// </summary>
        /// <param name="comparedGrid"></param>
        /// <returns></returns>
        public int CompareDirection(MyGrid comparedGrid)
        {
            if (Row + 1 == comparedGrid.Row && Col == comparedGrid.Col)
            {
                return 4;
            }
            else if (Row + 1 == comparedGrid.Row && Col + 1 == comparedGrid.Col)
            {
                return 2;
            }
            else if (Row - 1 == comparedGrid.Row && Col == comparedGrid.Col)
            {
                return 64;
            }
            else if (Row == comparedGrid.Row && Col + 1 == comparedGrid.Col)
            {
                return 1;
            }
            else if (Row - 1 == comparedGrid.Row && Col + 1 == comparedGrid.Col)
            {
                return 128;
            }
            else if (Row - 1 == comparedGrid.Row && Col - 1 == comparedGrid.Col)
            {
                return 32;
            }
            else if (Row == comparedGrid.Row && Col - 1 == comparedGrid.Col)
            {
                return 16;
            }
            else if (Row + 1 == comparedGrid.Row && Col - 1 == comparedGrid.Col)
            {
                return 8;
            }
            return 0;
        }

        public override string ToString()
        {
            return $"x{Row},y{Col}:{ALT}";
        }
    }
}
