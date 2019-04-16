using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FloodPeakUtility.UI
{
    public partial class FormMsg : Form
    {
        static FormMsg formMsg = new FormMsg();
        public FormMsg()
        {
            InitializeComponent();
        }

        public static void ShowMsg(string msg)
        {
            formMsg.label1.Text = msg;
            formMsg.ShowDialog();
        }

        public static bool Visiable
        {
            get
            {
                return formMsg.Visible;
            }
        }

        public static void HideMsg()
        {
            formMsg.Hide();
        }
    }
}
