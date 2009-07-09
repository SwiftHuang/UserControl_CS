using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace hwj.UserControls.CommonControls
{
    public class xComboBox : ComboBox, ICommonControls
    {
        #region ICommonControls 成员
        public bool IsRequired { get; set; }
        public bool EnterEqualTab { get; set; }
        #endregion

        public xComboBox()
        {
            IsRequired = false;
            EnterEqualTab = true;
        }

        protected override void OnCreateControl()
        {
            if (IsRequired)
                this.BackColor = Common.RequiredBackColor;
            else
                this.BackColor = System.Drawing.SystemColors.Window;
            base.OnCreateControl();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (EnterEqualTab && e.KeyCode == Keys.Enter)
                SendKeys.Send("{Tab}");
            base.OnKeyDown(e);
        }
    }
}
