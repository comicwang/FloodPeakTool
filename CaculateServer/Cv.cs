using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CaculateServer
{
    [XmlRoot("Cv")]
    public class Cv
    {
        [XmlElement("Cv")]
        public double Cv { get; set; }
        [XmlElement("Cs")]
        public double Cs { get; set; }
        [XmlElement("X")]
        public double X { get; set; }
         [XmlElement("Nihe")]
        public string Nihe { get; set; }
    }
}
