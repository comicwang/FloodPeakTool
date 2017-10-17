using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace FloodPeakUtility
{
   [XmlRoot("OutputLocation")]
    public class OutputLocation
    {
        [XmlElement("X")]
        public int X { get; set; }
        [XmlElement("Y")]
        public int Y { get; set; }
    }
}
