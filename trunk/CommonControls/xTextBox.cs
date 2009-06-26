using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading;

namespace hwj.UserControls.CommonControls
{
    public enum ContentType
    {
        None,
        Email,
        Numberic,
    }
    public class xTextBox : TextBox
    {
        public bool EnterEqualTab { get; set; }
        public bool ContentCheck { get; set; }
        public ContentType ContentType { get; set; }
        public bool IsRequired { get; set; }
        private ToolTip toolTip = new ToolTip();

        public xTextBox()
        {
            Properties.Resources.Culture = Thread.CurrentThread.CurrentUICulture;
            EnterEqualTab = true;
            ContentCheck = false;
            IsRequired = false;
        }
        protected override void OnCreateControl()
        {
            if (ContentType == ContentType.Numberic)
            {
                Text = "0.00";
                TextAlign = HorizontalAlignment.Right;
            }
            base.OnCreateControl();
        }


        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (EnterEqualTab && e.KeyCode == Keys.Enter)
                SendKeys.Send("{Tab}");
            base.OnKeyDown(e);
        }
        protected override void OnValidating(CancelEventArgs e)
        {
            if (ContentCheck)
            {
                switch (ContentType)
                {
                    case ContentType.None:
                        break;
                    case ContentType.Email:
                        if (!CommonLibrary.Utility.EmailHelper.isValidEmail(this.Text))
                        {
                            Common.ShowToolTipInfo(toolTip, this, string.Format(Properties.Resources.InvalidEmail, this.Text));
                            this.Text = string.Empty;
                            e.Cancel = true;
                        }
                        break;
                    case ContentType.Numberic:
                        if (!CommonLibrary.Utility.NumberHelper.IsNumeric(this.Text))
                        {
                            Common.ShowToolTipInfo(toolTip, this, string.Format(Properties.Resources.InvalidNumberic, this.Text));
                            this.Text = "0.00";
                            e.Cancel = true;
                        }
                        break;
                    default:
                        break;
                }
            }
            base.OnValidating(e);
        }
    }
}
