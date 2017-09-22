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
        [XmlElement("F")]
        public double F { get; set; }
        [XmlElement("R")]
        public double R { get; set; }
        [XmlElement("N")]
        public double N { get; set; }
        [XmlElement("r1")]
        public double r1 { get; set; }
    }
}
