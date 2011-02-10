using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace hwj.UserControls.Other
{
    public partial class PasswordTextBox : hwj.UserControls.CommonControls.xTextBox
    {
        private const int WM_GETTEXT = 0x000d;
        private const int WM_COPY = 0x0301;
        private const int WM_PASTE = 0x0302;
        private const int WM_CONTEXTMENU = 0x007B;
        private const int WM_RBUTTONDOWN = 0x0204;

        [Description("获取或设置允许复制密码"), DefaultValue(false)]
        public bool AllowCopy { get; set; }

        private List<char> _invalidChar = new List<char>();
        [DesignOnly(false)]
        public List<char> InvalidChar
        {
            get
            {
                if (_invalidChar == null)
                    return new List<char>();
                else
                    return _invalidChar;
            }
            set
            {
                if (value == null)
                    return;
                else
                    _invalidChar = value;
            }
        }

        public PasswordTextBox()
        {
            InitializeComponent();
        }

        protected override void WndProc(ref Message m)
        {
            if (!AllowCopy && (m.Msg == WM_RBUTTONDOWN || m.Msg == WM_COPY || m.Msg == WM_PASTE))//|| m.Msg == WM_GETTEXT
                return;
            base.WndProc(ref m);
        }
        protected override void OnKeyPress(System.Windows.Forms.KeyPressEventArgs e)
        {
            if (InvalidChar.Contains(e.KeyChar))
            {
                //hwj.UserControls.Common.ShowToolTipInfo(this, "Not allow input \"" + ((e.KeyChar == ' ') ? "SPACE" : e.KeyChar.ToString()) + "\" into password");
                hwj.UserControls.Common.ShowToolTipInfo(this, string.Format(Properties.Resources.InvalidPassword, ((e.KeyChar == ' ') ? Properties.Resources.Space : e.KeyChar.ToString())));
                e.Handled = true;
                return;
            }
            base.OnKeyPress(e);
        }
    }
}
