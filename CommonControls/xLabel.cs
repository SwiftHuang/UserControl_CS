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
        private Cursor OldCursor;
        private Font OldFont;

        //[Description("获取或设置复制模式开启时，鼠标进入时显示的背景色")]
        //public Color ClipboardBackColor { get; set; }
        [Description("获取或设置复制模式开启时鼠标进入时的字体样式(不填默认为将字体变为粗体)")]
        public Font ClipboardFont { get; set; }
        [DefaultValue(false), Description("获取或设置控件是否支持双击复制")]
        public bool ClipboardEnabled { get; set; }
        #endregion

        public xLabel()
        {
        }

        protected override void OnCreateControl()
        {
            if (ClipboardEnabled)
            {
                OldFont = this.Font;
                OldCursor = this.Cursor;
                if (!DesignMode && ClipboardFont == null)
                {
                    ClipboardFont = this.Font;
                    ClipboardFont = new Font(this.Font, FontStyle.Bold);
                }
            }
            base.OnCreateControl();
        }
        protected override void OnMouseEnter(EventArgs e)
        {
            if (ClipboardEnabled)
            {
                this.Cursor = Cursors.Hand;
                if (ClipboardFont != null)
                    this.Font = ClipboardFont;
            }
            base.OnMouseEnter(e);
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            if (ClipboardEnabled)
            {
                this.Cursor = OldCursor;
                if (OldFont != null)
                    this.Font = OldFont;
            }

            base.OnMouseLeave(e);
        }
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            if (ClipboardEnabled)
            {
                if (!string.IsNullOrEmpty(this.Text))
                {
                    Clipboard.SetDataObject(this.Text);
                    this.Font = OldFont;
                }
            }

            base.OnMouseDoubleClick(e);
        }
    }
}
