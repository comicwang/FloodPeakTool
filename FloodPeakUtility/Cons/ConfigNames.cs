using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FloodPeakUtility
{
    /// <summary>
    /// 配置文件名称
    /// </summary>
    public class ConfigNames
    {
        /// <summary>
        /// 暴雨损失文件名称
        /// </summary>
        public static readonly string RainStormLoss = "RainStormLoss.xml";

        /// <summary>
        /// 暴雨衰减文件名称
        /// </summary>
        public static readonly string RainStormSub = "RainStormSub.xml";

        /// <summary>
        /// 河槽汇流文件名称
        /// </summary>
        public static readonly string RiverConfluence = "RiverConfluence.xml";

        /// <summary>
        /// 坡面汇流文件名称
        /// </summary>
        public static readonly string SlopeConfluence = "SlopeConfluence.xml";

        /// <summary>
        /// 水文曲线文件名称
        /// </summary>
        public static readonly string SvCure = "SvCure.xml";

        /// <summary>
        /// 暴雨衰减参数文件名称
        /// </summary>
        public static readonly string SubCure = "SubCure.xml";

        /// <summary>
        /// 暴雨衰减参数文件名称(d=0)
        /// </summary>
        public static readonly string SubCure0 = "SubCure0.xml";

        /// <summary>
        /// 洪峰流量基础参数文件名称
        /// </summary>
        public static readonly string DefaultArgModel = "DefaultArgModel.xml";

        /// <summary>
        /// 洪峰流量文件名称
        /// </summary>
        public static readonly string FloodPeak = "FloodPeak.xml";

        /// <summary>
        /// 临时文件名称
        /// </summary>
        public static readonly string TempName = "TempName.xml";

        /// <summary>
        ///输出窗口位置文件
        /// </summary>
        public static readonly string OutputLocation = "OutputLocation.xml";

    }

    /// <summary>
    /// 后台计算方法名称
    /// </summary>
    public class MethodName
    {
        /// <summary>
        /// 水文曲线
        /// </summary>
        public static readonly string SWCure = "SWCure";

        /// <summary>
        /// 拟合曲线
        /// </summary>
        public static readonly string NiHeCure = "NiHeCure";

        /// <summary>
        /// 曲线反查
        /// </summary>
        public static readonly string ResearchCure = "ResearchCure";

        /// <summary>
        /// 暴雨衰减
        /// </summary>
        public static readonly string RainStormSub = "RainStormSub";

        /// <summary>
        /// 暴雨衰减(d=0)
        /// </summary>
        public static readonly string RainStormSub0 = "RainStormSub0";

        /// <summary>
        /// 洪峰流量
        /// </summary>
        public static readonly string FloodPeak = "FloodPeak";
    }
}
