using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace FloodPeakUtility
{
    public class FileExtentionRegist
    {
        public static bool FileTypeRegistered(string extension)
        {
            RegistryKey sluKey = Registry.ClassesRoot.OpenSubKey(extension);
            if (sluKey != null)
                return true;
            return false;
        }

        public static void UnRegistFileType(string extension)
        {
            if (FileTypeRegistered(extension))
            {
                Registry.ClassesRoot.DeleteSubKey(extension);
                string relationName = extension.Substring(1, extension.Length - 1).ToUpper() + " FileType";
                Registry.ClassesRoot.DeleteSubKeyTree(relationName);
                Registry.ClassesRoot.Close();
            }
        }

        public static void RegistFileType(string extension)
        {
            UnRegistFileType(extension);
            string relationName = extension.Substring(1, extension.Length - 1).ToUpper() + " FileType";
            RegistryKey sluKey = Registry.ClassesRoot.CreateSubKey(extension);
            sluKey.SetValue("", relationName);
            sluKey.Close();

            RegistryKey relationKey = Registry.ClassesRoot.CreateSubKey(relationName);
            relationKey.SetValue("", "Your Description");

            RegistryKey iconKey = relationKey.CreateSubKey("DefaultIcon");//图标
            iconKey.SetValue("", System.Windows.Forms.Application.StartupPath + @"\icon\iTelluro.Globe.ico");

            RegistryKey shellKey = relationKey.CreateSubKey("Shell");
            RegistryKey openKey = shellKey.CreateSubKey("Open");
            RegistryKey commandKey = openKey.CreateSubKey("Command");
            commandKey.SetValue("", System.Windows.Forms.Application.ExecutablePath + " %1");//是数字"1",双击文件之后就把文件的路径传递过来了.
            relationKey.Close();
        }
    }
}
