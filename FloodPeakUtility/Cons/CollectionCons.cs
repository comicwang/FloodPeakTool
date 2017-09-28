using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FloodPeakUtility
{
    /// <summary>
    /// 固定的字典集合项
    /// </summary>
    public static class CollectionCons
    {
        static CollectionCons()
        {
            DicStrToHour.Add("MAX_10_MIN", 0.17);
            DicStrToHour.Add("MAX_30_MIN", 0.5);
            DicStrToHour.Add("[MAX_1_HOUR]", 1);
            DicStrToHour.Add("[MAX_3_HOUR]", 3);
            DicStrToHour.Add("[MAX_6_HOUR]", 6);
            DicStrToHour.Add("[MAX_12_HOUR]", 12);
            DicStrToHour.Add("[MAX_24_HOUR]", 24);
            DicStrToHour.Add("[MAX_48_HOUR]", 48);
            DicStrToHour.Add("[MAX_72_HOUR]", 72);
            DicStrToHour.Add("MAX_1_DAY", 24);
            DicStrToHour.Add("MAX_3_DAY", 72);
            DicStrToHour.Add("MAX_5_DAY", 120);
            DicStrToHour.Add("MAX_7_DAY", 168);
            DicStrToHour.Add("MAX_15_DAY", 360);
            DicStrToHour.Add("MAX_30_DAY", 720);
        }

        /// <summary>
        /// 需要统计的概率集合
        /// </summary>
        public static List<double> StaticsPercents = new List<double>()
        {
            1,2,5,10,20
        };

        public static Dictionary<string, double> DicStrToHour = new Dictionary<string, double>();
      
    }
}
