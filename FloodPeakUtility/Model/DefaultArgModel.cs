using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace FloodPeakUtility
{
    [XmlRoot("DefaultArgModel")]
    public class DefaultArgModel
    {
         [XmlElement("Qm")]
        public string Qm { get; set; }
         [XmlElement("p1")]
        public string p1 { get; set; }
         [XmlElement("esp1")]
        public string esp1 { get; set; }
         [XmlElement("esp2")]
        public string esp2 { get; set; }
         [XmlElement("tc")]
        public string tc { get; set; }
    }
}
