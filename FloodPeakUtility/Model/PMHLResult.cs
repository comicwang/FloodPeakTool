using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace FloodPeakUtility.Model
{
    [XmlRoot("SlopeConfluence")]
    public class PMHLResult
    {
        [XmlElement("L2")]
        public double L2 { get; set; }
        [XmlElement("l2")]
        public double l2 { get; set; }
        [XmlElement("A2")]
        public double A2 { get; set; }
    }
}
