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

        private List<char> unallowList = new List<char>();
        [DesignOnly(false)]
        public List<char> UnallowList
        {
            get
            {
                if (unallowList == null)
                    return new List<char>();
                else
                    return unallowList;
            }
            set
            {
                if (value == null)
                    return;
                else
                    unallowList = value;
            }
        }

        public PasswordTextBox()
        {
            InitializeComponent();
        }
        public PasswordTextBox(IContainer container)
        {
            container.Add(this);
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
            if (UnallowList.Contains(e.KeyChar))
            {
                hwj.UserControls.Common.ShowToolTipInfo(this, "Not allow input \"" + ((e.KeyChar == ' ') ? "SPACE" : e.KeyChar.ToString()) + "\" into password");
                e.Handled = true;
                return;
            }
            base.OnKeyPress(e);
        }
    }
}
