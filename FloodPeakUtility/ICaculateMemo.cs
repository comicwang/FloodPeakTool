using FloodPeakUtility.UI;
using iTelluro.GlobeEngine.MapControl3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FloodPeakUtility
{
    public interface ICaculateMemo
    {

        string CaculateId { get; }
        /// <summary>
        /// 模块名称
        /// </summary>
        string CaculateName { get; }

        /// <summary>
        /// 模块描述
        /// </summary>
        string Discription { get; }

        /// <summary>
        /// 加载模块
        /// </summary>
        /// <param name="Parent"></param>
        void LoadPlugin(GlobeView Globe, PnlLeftControl Parent);

    }
}
