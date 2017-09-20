using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FloodPeakUtility
{
    public class ConfigNames
    {
        /// <summary>
        /// 暴雨损失文件名称
        /// </summary>
        public static readonly string RainStormLoss = "RainStormLoss";

        /// <summary>
        /// 暴雨衰减文件名称
        /// </summary>
        public static readonly string RainStormSub = "RainStormSub";

        /// <summary>
        /// 河槽汇流文件名称
        /// </summary>
        public static readonly string RiverConfluence = "RiverConfluence";

        /// <summary>
        /// 坡面汇流文件名称
        /// </summary>
        public static readonly string SlopeConfluence = "SlopeConfluence";
    }
}
