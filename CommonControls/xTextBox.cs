using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using hwj.UserControls.Interface;

namespace hwj.UserControls.CommonControls
{
    public enum ContentType
    {
        None,
        Email,
        /// <summary>
        /// 数字(含正负小数点)
        /// </summary>
        Numberic,
        /// <summary>
        /// 整数
        /// </summary>
        Integer,
    }

    public class xTextBox : TextBox, IEnterEqualTab
    {
        #region Property
        [DefaultValue(true)]
        public bool EnterEqualTab { get; set; }

        /// <summary>
        /// 获取或设置为必填控件
        /// </summary>
        [DefaultValue(false), Description("获取或设置为必填控件")]
        public bool IsRequired { get; set; }

        [Browsable(false)]
        public System.Drawing.Color OldBackColor { get; set; }

        /// <summary>
        /// 设置引发hwj.UserControls.ValueChanged事件的对象
        /// </summary>
        [DefaultValue(null), Description("设置引发hwj.UserControls.ValueChanged事件的对象"), Browsable(false)]
        protected internal Function.Verify.ValueChangedHandle ValueChangedHandle { get; set; }
        protected Function.Verify.RequiredHandle RequiredHandle { get; set; }

        /// <summary>
        /// "是否显示验证错误信息(只有ContentType不为None时有效)"
        /// </summary>
        [DefaultValue(true), Description("是否显示验证错误信息(只有ContentType不为None时有效)")]
        public bool ShowContentError { get; set; }

        [DefaultValue(ContentType.None)]
        public ContentType ContentType { get; set; }

        [DefaultValue("")]
        public string Format { get; set; }

        /// <summary>
        /// 内容是否改变(在Validating/Validated有效)
        /// </summary>
        [Browsable(false)]
        public bool TextIsChanged { get; set; }

        [Description("当值改变时,同时赋值给指定的控件")]
        public xTextBox SetValueToControl { get; set; }
        #endregion

        public xTextBox()
        {
            Properties.Resources.Culture = Thread.CurrentThread.CurrentUICulture;
            ShowContentError = true;

            TextIsChanged = false;
            OldBackColor = this.BackColor;
            IsRequired = false;
            EnterEqualTab = true;
            SetValueToControl = null;
        }

        #region Override Function
        protected override void OnCreateControl()
        {
            if (ContentType == ContentType.Numberic)
            {
                if (Text == string.Empty)
                    Text = "0.00";
                TextAlign = HorizontalAlignment.Right;
            }
            base.OnCreateControl();
            RequiredHandle = Common.Required;
            ValueChangedHandle = Common.ValueChanged;

            SetRequiredStatus();
        }
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            //3=Ctrl+C;22=Ctrl+V;26=Ctrl+Z
            if (e.KeyChar == 3 || e.KeyChar == 22 || e.KeyChar == 26)
            {
            }
            else
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
                else if (ContentType == ContentType.Integer)
                {
                    if (!(Char.IsNumber(e.KeyChar) || e.KeyChar == '\b'))
                        e.Handled = true;
                }
            }
            base.OnKeyPress(e);
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (EnterEqualTab && e.KeyCode == Keys.Enter)
                if (e.KeyData != (Keys.Control | Keys.Enter))
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
                        if (IsRequired)
                            e.Cancel = true;
                    }
                    break;
                case ContentType.Numberic:
                    decimal v = 0;
                    if (!decimal.TryParse(this.Text, out v))
                    {
                        if (ShowContentError)
                            Common.ShowToolTipInfo(this, string.Format(Properties.Resources.InvalidNumberic, this.Text));
                        if (IsRequired)
                            e.Cancel = true;
                    }
                    if (!string.IsNullOrEmpty(Format))
                        this.Text = v.ToString(Format);
                    break;
                default:
                    break;
            }
            SetRequiredStatus();
            if (SetValueToControl != null)
                SetValueToControl.Text = this.Text;
            base.OnValidating(e);
        }
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            TextIsChanged = false;
        }
        protected override void OnTextChanged(EventArgs e)
        {
            if (DesignMode)
                return;
            TextIsChanged = true;
            if (this.Focused && ValueChangedHandle != null)
                ValueChangedHandle.IsChanged = true;
            base.OnTextChanged(e);
            SetRequiredStatus();
        }
        protected override void OnValidated(EventArgs e)
        {
            base.OnValidated(e);
            TextIsChanged = false;
        }
        #endregion

        #region Private Function
        public void SetRequiredStatus()
        {
            if (DesignMode) return;
            if (IsRequired && string.IsNullOrEmpty(this.Text))
            {
                if (RequiredHandle != null)
                {
                    if (this.Parent is Suggest.SuggestBox)
                        RequiredHandle.Add(this.Parent);
                    else
                        RequiredHandle.Add(this);
                }
                this.BackColor = Common.RequiredBackColor;
            }
            else
            {
                if (RequiredHandle != null)
                {
                    if (this.Parent is Suggest.SuggestBox)
                        RequiredHandle.Remove(this.Parent);
                    else
                        RequiredHandle.Remove(this);
                }
                this.BackColor = this.OldBackColor;
            }
        }
        private bool IsNegatives()
        {
            if (ContentType == ContentType.Numberic && this.Text.IndexOf('-') != -1)
                return true;
            else
                return false;
        }
        #endregion

    }

}
