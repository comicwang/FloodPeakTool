using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace FloodPeakUtility
{
    [XmlRoot("MainResult")]
    public class MainResult
    {
        [XmlElement("Qm")]
        public double Qm { get; set; }
        [XmlElement("P1")]
        public double p1 { get; set; }
        [XmlElement("tQ")]
        public double tQ { get; set; }
        [XmlElement("t")]
        public double t { get; set; }
        [XmlElement("a1tc")]
        public double a1tc { get; set; }
        [XmlElement("d1")]
        public double d1 { get; set; }
        [XmlElement("d2")]
        public double d2 { get; set; }
    }

}
