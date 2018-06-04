/********************************************************************************
    ** auth： 王冲
    ** date： 2017/10/30 15:50:52
    ** desc： 尚未编写描述
    ** Ver.:  V1.0.0
*********************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FloodPeakUtility.UI
{
    public partial class FormColorSetting : Form
    {
        private static ColorRamps _colors = null;
        private static string _path = Path.Combine(Application.StartupPath, ConfigNames.Colors);
        private static int _index = 10000;

        public FormColorSetting()
        {
            InitializeComponent();
        }

        private void CreateColorRamp(double value, Color color)
        {
            ColorChoose ctl = new ColorChoose(value, color);
            ctl.Height = 21;
            ctl.Width = 354;
            ctl.TabIndex = _index;
            ctl.Dock = DockStyle.Top;
            this.panel2.Controls.Add(ctl);
            _index--;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            CreateColorRamp(1, Color.White);
        }

        private void FormColorSetting_Load(object sender, EventArgs e)
        {
            _colors = XmlHelper.Deserialize<ColorRamps>(_path);
            if (_colors != null && _colors.MyRamps != null && _colors.MyRamps.Length > 0)
            {
                Array.ForEach(_colors.MyRamps, t =>
                    {
                        if (t.value == 0)
                        {
                            colorChoose1.SelectColor = Color.FromArgb(t.A, t.R, t.G, t.B);
                            colorChoose1.ColorValue = t.value;
                        }
                        else
                            CreateColorRamp(t.value, Color.FromArgb(t.A, t.R, t.G, t.B));
                    });
            }
            else
            {
                _colors = new ColorRamps();
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            List<ColorRamp> temp = new List<ColorRamp>();
            foreach (Control ctl in panel2.Controls)
            {
                ColorChoose color = ctl as ColorChoose;
                temp.Add(new ColorRamp() { value = color.ColorValue, A = color.SelectColor.A, R = color.SelectColor.R, G = color.SelectColor.G, B = color.SelectColor.B });
            }
            _colors.MyRamps = temp.ToArray();

            XmlHelper.Serialize<ColorRamps>(_colors, _path);

            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
