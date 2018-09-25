using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FloodPeakUtility.Model
{
    /// <summary>
    /// 雨量计算条件模型
    /// </summary>
    public class RainCaculateConditon
    {
        /// <summary>
        /// 站号
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 事件号
        /// </summary>
        public string EventNum { get; set; }
    }
}
