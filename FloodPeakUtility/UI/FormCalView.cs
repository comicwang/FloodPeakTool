/********************************************************************************
    ** auth： 王冲
    ** date： 2017/10/30 13:35:29
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
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace FloodPeakUtility.UI
{
    public partial class FormCalView : Form
    {
        [DllImport("gdi32.dll")]//取指定点颜色   
        private static extern int GetPixel(IntPtr hdc, Point p);

        [DllImport("user32.dll")]//取设备场景   
        private static extern IntPtr GetDC(IntPtr hwnd);//返回设备场景句柄   

        private static FormCalView _form = null;
        private static ColorRamps _colorRamps = null;
        private static string _path = Path.Combine(Application.StartupPath, ConfigNames.Colors);
        public FormCalView()
        {
            InitializeComponent();
            _colorRamps = XmlHelper.Deserialize<ColorRamps>(_path);
        }

        public void SetSize(int x,int y)
        {
            panel1.Width = x;
            panel1.Height = y;
        }

        public void DrawValue(int x, int y, int value)
        {
            Graphics grp = panel1.CreateGraphics();
            grp.DrawString(value.ToString(), new Font("宋体", 5), Brushes.Black, new RectangleF(x, y, 1, 1));
            grp.Dispose();
        }

        public void DrawColor(int x, int y, Color color)
        {
            Graphics grp = panel1.CreateGraphics();
            Pen pen = new Pen(color);
            grp.DrawRectangle(pen, new Rectangle(x, y, 1, 1));
            pen.Dispose();
            grp.Dispose();
        }

        public void DrawLegend(Dictionary<string, Color> dicLegend)
        {
            Graphics grp = pnlColor.CreateGraphics();
            grp.Clear(pnlColor.BackColor);
            Brush strBrush = Brushes.Black;
            if (dicLegend != null && dicLegend.Count > 0)
            {
                int i = 1;
                int top = 40;
                int left = 10;
                foreach (KeyValuePair<string, Color> item in dicLegend)
                {

                    SolidBrush brush = new SolidBrush(item.Value);
                    grp.FillRectangle(brush, new Rectangle(left, top * i, 30, 30));
                    grp.DrawString(item.Key, new Font("微软雅黑", 12), strBrush, new RectangleF(60, top * i, 150, 30));
                    i++;
                    brush.Dispose();
                }
            }
            strBrush.Dispose();
            grp.Dispose();
        }

        public static void InitializeForm()
        {
            _form = new FormCalView();
        }

        public static void SetAllSize(int x,int y)
        {
            try
            {
                if (_form == null || _form.IsDisposed)
                {
                    _form = new FormCalView();
                }
                if (_form.InvokeRequired)
                {
                    _form.Invoke(new Action(() =>
                    {
                        _form.Show();
                        _form.WindowState = FormWindowState.Normal;
                        _form.Activate();
                        _form.SetSize(x, y);
                      
                    }));
                }
                else
                {
                    _form.Show();
                    _form.WindowState = FormWindowState.Normal;
                    _form.Activate();
                    _form.SetSize(x, y);
                  
                }
            }
            catch { }
        }

        public static void SetValue(int x, int y, int value)
        {
            try
            {
                if (_form == null || _form.IsDisposed)
                {
                    return;
                }
                if (_form.InvokeRequired)
                {
                    _form.Invoke(new Action(() =>
                    {
                        _form.DrawValue(x,y,value);
                    }));
                }
                else
                {
                    _form.DrawValue(x, y, value);
                }
            }
            catch { }
        }

        public static void SetColor(int x, int y, int value)
        {
            try
            {
                if (_form == null || _form.IsDisposed)
                {
                    return;
                }
                if (_form.InvokeRequired)
                {
                    _form.Invoke(new Action(() =>
                    {
                        _form.DrawColor(x, y, _colorRamps.GetColor(value));
                    }));
                }
                else
                {
                    _form.DrawColor(x, y, _colorRamps.GetColor(value));
                }
            }
            catch { }
        }

        public static void SetLegend(Dictionary<string, Color> dicLegend)
        {
            try
            {
                if (_form == null || _form.IsDisposed)
                {
                    return;
                }
                if (_form.InvokeRequired)
                {
                    _form.Invoke(new Action(() =>
                    {
                        _form.DrawLegend(dicLegend);
                    }));
                }
                else
                {
                    _form.DrawLegend(dicLegend);
                }
            }
            catch { }
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            Panel pnl = sender as Panel;
            Point point = pnl.PointToClient(Control.MousePosition);

            IntPtr hdc = GetDC(new IntPtr(0));//取到设备场景(0就是全屏的设备场景)   
            int c = GetPixel(hdc, Control.MousePosition);//取指定点颜色   
            int b = (c & 0xFF0000) / 65536;//转换B   
            label1.Text = string.Format("x:{0},y:{1},v:{2}", point.X, point.Y, b);
        }
    }
}
