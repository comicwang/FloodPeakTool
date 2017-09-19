using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace FloodPeakUtility
{
    /// <summary>
    /// Author:王冲
    /// Date:2017-09-12
    /// xml文件操作的帮助类
    /// </summary>
    public class XmlHelp
    {
        /// <summary>
        /// 将xml文件序列化为实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlPath"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string xmlPath) where T : class
        {
            if (File.Exists(xmlPath))
            {
                using (StreamReader reader = new StreamReader(xmlPath))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(T));
                    return xs.Deserialize(reader) as T;
                }
            }
            return default(T);
        }

        /// <summary>
        /// 将实体存储到xml文件中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="xmlPath"></param>
        public static void Serialize<T>(T obj, string xmlPath) where T : class
        {
            using (StreamWriter writer = new StreamWriter(xmlPath))
            {
                XmlSerializer xs = new XmlSerializer(typeof(T));
                xs.Serialize(writer, obj);
            }
        }
    }
}
