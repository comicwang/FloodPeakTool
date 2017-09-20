using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FloodPeakToolUI.UI
{
    public partial class FormCaculateArgView : Form
    {
        public FormCaculateArgView()
        {
            InitializeComponent();
        }

        public FormCaculateArgView(string projectForlder)
        {

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            //保存参数
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
    }
}
