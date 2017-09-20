using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace FloodPeakUtility.Model
{
    [XmlRoot("RainstormSub")]
    public class BYSJResult
    {
        [XmlElement("Sd")]
        public double Sd { get; set; }
        [XmlElement("nd")]
        public double nd { get; set; }
         [XmlElement("d")]
        public double d { get; set; }
    }
}
