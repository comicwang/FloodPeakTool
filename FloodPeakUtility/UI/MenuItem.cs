using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FloodPeakUtility.UI
{
    /// <summary>
    /// 扩展MenuItem
    /// 1.HoverImage
    /// 2.自动Dock
    /// </summary>
    [ToolboxItem(true)]
    [DesignTimeVisible(true)]
    public class MenuItem : PictureBox
    {
        public MenuItem()
        {
            base.Cursor = Cursors.Hand;
            base.MouseEnter += MenuItem_MouseEnter;
            base.MouseLeave += MenuItem_MouseLeave;           
        }

        private void MenuItem_MouseLeave(object sender, EventArgs e)
        {
            if (DefaultImage != null && HoverImage != null)
            {
                base.Image = DefaultImage;
            }
        }

        private void MenuItem_MouseEnter(object sender, EventArgs e)
        {
            if (DefaultImage != null && HoverImage != null)
            {
                base.Image = HoverImage;
            }
        }

        /// <summary>
        /// 默认图片
        /// </summary>
        public Image DefaultImage
        {
            get;
            set;
        }

        /// <summary>
        /// Hover图片
        /// </summary>
        public Image HoverImage
        {
            get;
            set;
        }

        private AligmentEnum _aligment = AligmentEnum.Near;

        /// <summary>
        /// Dock在菜单上的位置
        /// </summary>
        public AligmentEnum Aligment
        {
            get
            {
                return _aligment;
            }
            set
            {
                _aligment = value;
                switch (Aligment)
                {
                    case AligmentEnum.Far:
                        base.Dock = DockStyle.Right;
                        break;
                    case AligmentEnum.Near:
                        base.Dock = DockStyle.Left;
                        break;
                    default:
                        base.Dock = DockStyle.Left;
                        break;
                }
            }
        }

        public enum AligmentEnum
        {
            Far,
            Near
        }


    }
}
