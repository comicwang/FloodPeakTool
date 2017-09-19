using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace FloodPeakUtility.Model
{
    [XmlRoot("RainstormLoss")]
    public class BYSSResult
    {
        [XmlElement("Area")]
        public double AreaR { get; set; }
        [XmlElement("LoosR")]
        public double LossR { get; set; }
         [XmlElement("SubN")]
        public double SubN { get; set; }
    }
}
