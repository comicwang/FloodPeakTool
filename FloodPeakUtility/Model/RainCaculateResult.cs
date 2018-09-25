using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FloodPeakUtility.Model
{
    /// <summary>
    /// 雨量计算结果
    /// </summary>
    public class RainCaculateResult
    {
        /// <summary>
        /// 计算最大值结果
        /// </summary>
        public double? MaxValue { get; set; }

        /// <summary>
        /// 计算结果的日期
        /// </summary>
        public DateTime MaxValueDate { get; set; }

        /// <summary>
        /// 最大值的控制码
        /// </summary>
        public string MaxValueQc { get; set; }

        /// <summary>
        /// 最大值的时间范围
        /// </summary>
        public double During { get; set; }

        /// <summary>
        /// 是否是天统计
        /// </summary>
        public bool Day { get; set; } = false;

        /// <summary>
        /// 是否是月统计最大值
        /// </summary>
        public bool MonthMax { get; set; } = false;

        /// <summary>
        /// 事件编号
        /// </summary>
        public string EventNum { get; set; }

    }
}
