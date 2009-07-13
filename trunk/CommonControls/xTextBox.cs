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
    public class xTextBox : TextBox, ICommonControls
    {
        #region Property
        private System.Drawing.Color oldBackColor;
        [DefaultValue(true)]
        public bool EnterEqualTab { get; set; }
        /// <summary>
        /// 获取或设置为必填控件
        /// </summary>
        [DefaultValue(false), Description("获取或设置为必填控件")]
        public bool IsRequired { get; set; }
        /// <summary>
        /// "是否显示验证错误信息(只有ContentType不为None时有效)"
        /// </summary>
        [DefaultValue(true), Description("是否显示验证错误信息(只有ContentType不为None时有效)")]
        public bool ShowContentError { get; set; }
        [DefaultValue(ContentType.None)]
        public ContentType ContentType { get; set; }
        [DefaultValue("")]
        public string Format { get; set; }

        #endregion

        public xTextBox()
        {
            oldBackColor = this.BackColor;
            Properties.Resources.Culture = Thread.CurrentThread.CurrentUICulture;
            ShowContentError = true;

            IsRequired = false;
            EnterEqualTab = true;
        }
        protected override void OnCreateControl()
        {
            if (ContentType == ContentType.Numberic)
            {
                if (Text == string.Empty)
                    Text = "0.00";
                TextAlign = HorizontalAlignment.Right;
            }
            SetRequiredStatus(IsRequired);
            base.OnCreateControl();
        }
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (ContentType == ContentType.Numberic)
            {
                if (e.KeyChar == '.')
                {
                    if (this.Text.IndexOf(e.KeyChar) != -1)
                        e.Handled = true;
                    return;
                }
                else if (e.KeyChar == '-')
                {
                    if (this.Text.IndexOf(e.KeyChar) != -1)
                        this.Text = this.Text.Replace("-", "");
                    else
                    {
                        this.Text = this.Text.Insert(0, "-");
                        SelectionStart = 1;
                    }
                    e.Handled = true;
                    return;
                }
                if (!(Char.IsNumber(e.KeyChar) || e.KeyChar == '\b'))
                    e.Handled = true;
            }
            base.OnKeyPress(e);
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (EnterEqualTab && e.KeyCode == Keys.Enter)
                SendKeys.Send("{Tab}");
            if (IsNegatives() && e.KeyCode == Keys.Left && SelectionStart == 1)
                e.Handled = true;
            base.OnKeyDown(e);
        }
        protected override void OnClick(EventArgs e)
        {
            if (IsNegatives() && SelectionStart == 0)
                SelectionStart = 1;
            base.OnClick(e);
        }
        protected override void OnValidating(CancelEventArgs e)
        {
            switch (ContentType)
            {
                case ContentType.None:
                    break;
                case ContentType.Email:
                    if (!CommonLibrary.Object.EmailHelper.isValidEmail(this.Text))
                    {
                        if (ShowContentError)
                            Common.ShowToolTipInfo(this, string.Format(Properties.Resources.InvalidEmail, this.Text));
                        this.Text = string.Empty;
                        e.Cancel = true;
                    }
                    break;
                case ContentType.Numberic:
                    decimal v = 0;
                    if (!decimal.TryParse(this.Text, out v))
                    {
                        if (ShowContentError)
                            Common.ShowToolTipInfo(this, string.Format(Properties.Resources.InvalidNumberic, this.Text));
                        e.Cancel = true;
                    }
                    if (!string.IsNullOrEmpty(Format))
                        this.Text = v.ToString(Format);
                    break;
                default:
                    break;
            }
            if (IsRequired)
            {
                if (string.IsNullOrEmpty(this.Text))
                    SetRequiredStatus(true);
                else
                    SetRequiredStatus(false);
            }
            base.OnValidating(e);
        }
        protected override void OnTextChanged(EventArgs e)
        {
            VerifyInfo.ValueIsChanged = true;
            base.OnTextChanged(e);
        }
        private void SetRequiredStatus(bool isRequired)
        {
            if (isRequired)
            {
                VerifyInfo.AddRequiredControl(this);
                this.BackColor = Common.RequiredBackColor;
            }
            else
            {
                VerifyInfo.RemoveRequiredControl(this);
                this.BackColor = System.Drawing.SystemColors.Window;
            }
        }
        private bool IsNegatives()
        {
            if (ContentType == ContentType.Numberic && this.Text.IndexOf('-') != -1)
                return true;
            else
                return false;
        }
    }

}
