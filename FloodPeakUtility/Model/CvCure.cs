using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace FloodPeakUtility
{
    [XmlRoot("CvCure")]
    public class CvCure
    {
        [XmlElement("Cv")]
        public double Cv { get; set; }
        [XmlElement("Cs")]
        public double Cs { get; set; }
        [XmlElement("X")]
        public double X { get; set; }
        [XmlElement("Nihe")]
        public string Nihe { get; set; }
        [XmlIgnore]
        public string State { get; set; }
        [XmlIgnore]
        public string Time { get; set; }
    }
}
