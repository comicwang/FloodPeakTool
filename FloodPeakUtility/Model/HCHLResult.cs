using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace FloodPeakUtility.Model
{
    [XmlRoot("RiverConfluence")]
    public class HCHLResult
    {
        [XmlElement("L1")]
        public double L1 { get; set; }
        [XmlElement("l1")]
        public double l1 { get; set; }
        [XmlElement("A1")]
        public double A1 { get; set; }
    }
}
