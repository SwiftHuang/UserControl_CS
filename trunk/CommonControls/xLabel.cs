using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;

namespace hwj.UserControls.CommonControls
{
    public partial class xLabel : Label
    {
        #region Property
        private Color OldBackColor;
        private Cursor OldCursor;

        [Description("获取或设置复制模式开启时，鼠标停留时显示的背景色")]
        public Color ClipboardBackColor { get; set; }

        [DefaultValue(false), Description("获取或设置控件是否支持双击复制")]
        public bool ClipboardEnabled { get; set; }
        #endregion

        public xLabel()
        {
            ClipboardBackColor = Color.FromArgb(175, 167, 222);
        }

        protected override void OnCreateControl()
        {
            if (ClipboardEnabled)
            {
                OldBackColor = this.BackColor;
                OldCursor = this.Cursor;
            }
            base.OnCreateControl();
        }
        protected override void OnMouseEnter(EventArgs e)
        {
            if (ClipboardEnabled)
            {
                this.Cursor = Cursors.Hand;
                this.BackColor = ClipboardBackColor;
            }
            base.OnMouseEnter(e);
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            if (ClipboardEnabled)
            {
                this.Cursor = OldCursor;
                this.BackColor = OldBackColor;
            }

            base.OnMouseLeave(e);
        }
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            if (ClipboardEnabled)
            {
                if (!string.IsNullOrEmpty(this.Text))
                    Clipboard.SetDataObject(this.Text);
            }

            base.OnMouseDoubleClick(e);
        }
    }
}
