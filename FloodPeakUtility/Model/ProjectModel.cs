using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace FloodPeakUtility.Model
{

    [XmlRoot("ProjectModel")]
    public class ProjectModel
    {
        [XmlElement("ProjectName")]
        public string ProjectName { get; set; }
        [XmlElement("ProjectPath")]
        public string ProjectPath { get; set; }

        [XmlArray("NodeModels")]
        public NodeModel[] Nodes { get; set; }
        
    }

    public class NodeModel
    {
        [XmlElement("NodeName")]
        public string NodeName { get; set; }
        [XmlElement("NodeId")]
        public string NodeId { get; set; }
        [XmlElement("PNode")]
        public string PNode { get; set; }
        [XmlElement("CanRemove")]
        public bool CanRemove { get; set; }
        [XmlElement("ShowCheck")]
        public bool ShowCheck { get; set; }
        [XmlElement("ImageIndex")]
        public int ImageIndex { get; set; }
        [XmlElement("Path")]
        public string Path { get; set; }
    }
}
