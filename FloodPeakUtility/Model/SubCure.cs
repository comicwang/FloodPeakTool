using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace FloodPeakUtility
{
    [XmlRoot("SubCure")]
    public class SubCure
    {
        [XmlElement("Sd")]
        public double Sd { get; set; }
        [XmlElement("nd")]
        public double nd { get; set; }
        [XmlElement("d")]
        public double d { get; set; }
        [XmlElement("n1")]
        public double n1 { get; set; }
        [XmlElement("n2")]
        public double n2 { get; set; }
         [XmlElement("j1")]
        public double j1 { get; set; }
         [XmlElement("j2")]
        public double j2 { get; set; }
    }
}
