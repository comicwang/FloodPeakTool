using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace FloodPeakUtility
{
    public class ColorRamp
    {
        [XmlElement("value")]
        public double value { get; set; }
        [XmlElement("A")]
        public byte A { get; set; }
        [XmlElement("R")]
        public byte R { get; set; }
        [XmlElement("G")]
        public byte G { get; set; }
        [XmlElement("B")]
        public byte B { get; set; }
    }

     [XmlRoot("Colors")]
    public class ColorRamps
    {
         [XmlArray("ColorRamps")]
         public ColorRamp[] MyRamps { get; set; }

         /// <summary>
         /// 根据值获取颜色
         /// </summary>
         /// <param name="value"></param>
         /// <returns></returns>
         public Color GetColor(double value)
         {
            if (this.MyRamps == null)
                return Color.White;
            ColorRamp result = MyRamps.Where(t => t.value == value).FirstOrDefault();
            if (result == null)
                return Color.Black;
            return Color.FromArgb(result.A, result.R, result.G, result.B);
            //return Color.FromArgb(10, 255, 0, (int)value);
         }


    }
}
