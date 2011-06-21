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

    public class xTextBox : TextBox, IEnterEqualTab, IValueChanged
    {

        #region Property

        private bool isFirstFocus = false;
        [DefaultValue(true)]
        public bool EnterEqualTab { get; set; }

        private bool _IsRequired = false;
        /// <summary>
        /// 获取或设置为必填控件
        /// </summary>
        [DefaultValue(false), Description("获取或设置为必填控件")]
        public bool IsRequired
        {
            get { return _IsRequired; }
            set
            {
                _IsRequired = value;
                SetRequiredStatus();
            }
        }

        [Browsable(false)]
        public System.Drawing.Color OldBackColor { get; set; }

        /// <summary>
        /// 设置引发hwj.UserControls.ValueChanged事件的对象
        /// </summary>
        [DefaultValue(null), Description("设置引发hwj.UserControls.ValueChanged事件的对象"), Browsable(false)]
        protected internal Function.Verify.ValueChangedHandle ValueChangedHandle { get; set; }
        Function.Verify.RequiredHandle _RequiredHandle = null;
        public Function.Verify.RequiredHandle RequiredHandle
        {
            get { return _RequiredHandle; }
            set
            {
                _RequiredHandle = value;
                SetRequiredStatus();
            }
        }

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

        [DefaultValue(false), Description("当获取焦点时,自动全选")]
        public bool AutoSelectAll { get; set; }

        /// <summary>
        /// 获取或设置禁用的密码字符。
        /// </summary>
        [Description("获取或设置禁用的密码字符。"), DesignOnly(true), Browsable(false)]
        public List<char> InvalidPasswordChar { get; set; }
        #endregion

        public xTextBox()
        {
            Properties.Resources.Culture = Thread.CurrentThread.CurrentUICulture;
            ShowContentError = true;

            this.Disposed += new EventHandler(xTextBox_Disposed);
            TextIsChanged = false;
            OldBackColor = this.BackColor;
            IsRequired = false;
            EnterEqualTab = true;
            SetValueToControl = null;
            ValueChangedEnabled = true;
        }

        void xTextBox_Disposed(object sender, EventArgs e)
        {
            Common.HideToolTip();
        }

        #region Override Function
        protected override void OnCreateControl()
        {
            if (DesignMode)
                return;
            if (ContentType == ContentType.Numberic || ContentType == ContentType.Integer)
            {
                if (string.IsNullOrEmpty(Text) && !string.IsNullOrEmpty(Format))
                {
                    if (ContentType == ContentType.Numberic)
                        Text = decimal.Parse("0").ToString(Format);
                    else if (ContentType == ContentType.Integer)
                        Text = int.Parse("0").ToString(Format);
                }
                TextAlign = HorizontalAlignment.Right;
            }
            base.OnCreateControl();
            if (RequiredHandle == null)
                RequiredHandle = Common.Required;
            ValueChangedHandle = Common.ValueChanged;

            SetRequiredStatus();
        }
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (DesignMode)
                return;
            //3=Ctrl+C;22=Ctrl+V;26=Ctrl+Z
            if (!ReadOnly)
            {
                if (e.KeyChar == 3 || e.KeyChar == 22 || e.KeyChar == 26)
                {
                }
                else if (this.SelectedText != this.Text)
                {
                    if (ContentType == ContentType.Numberic)
                    {

                        if (e.KeyChar == '.')
                        {
                            int index = this.Text.IndexOf(e.KeyChar);
                            if (index != -1)
                            {
                                string tmp = string.Empty;
                                if (this.SelectionStart < index)
                                {
                                    tmp = this.Text.Remove(this.Text.IndexOf(e.KeyChar), 1);
                                    tmp = tmp.Insert(this.SelectionStart, ".");
                                    tmp = tmp.Replace(",", "");
                                }
                                else if (this.SelectionStart > index)
                                {
                                    tmp = this.Text.Insert(this.SelectionStart, ".");
                                    tmp = tmp.Remove(this.Text.IndexOf(e.KeyChar), 1);
                                    tmp = tmp.Replace(",", "");
                                }
                                else if (this.SelectionStart == index)
                                {
                                    e.Handled = true;
                                    return;
                                }
                                this.Text = tmp;
                                SelectionStart = this.Text.IndexOf(".") + 1;
                                e.Handled = true;
                            }
                            return;
                        }
                        else if (e.KeyChar == '-')
                        {
                            if (SelectionLength == this.Text.Length)
                                this.Text = string.Empty;

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
                if (PasswordChar != Char.MinValue && InvalidPasswordChar != null && InvalidPasswordChar.Contains(e.KeyChar))
                {
                    hwj.UserControls.Common.ShowToolTipError(this, string.Format(Properties.Resources.InvalidPassword, ((e.KeyChar == ' ') ? Properties.Resources.Space : e.KeyChar.ToString())));
                    e.Handled = true;
                    return;
                }
            }
            base.OnKeyPress(e);
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (DesignMode)
                return;
            if (EnterEqualTab && e.KeyCode == Keys.Enter)
                if (e.KeyData != (Keys.Control | Keys.Enter))
                    SendKeys.Send("{Tab}");
            if (IsNegatives() && e.KeyCode == Keys.Left && SelectionStart == 1)
                e.Handled = true;
            base.OnKeyDown(e);
        }
        protected override void OnClick(EventArgs e)
        {
            if (DesignMode)
                return;
            if (IsNegatives() && SelectionStart == 0)
                SelectionStart = 1;
            if (AutoSelectAll && isFirstFocus)
                this.SelectAll();
            isFirstFocus = false;
            base.OnClick(e);
        }
        protected override void OnValidating(CancelEventArgs e)
        {
            if (DesignMode)
                return;

            string error = string.Empty;
            string value = string.Empty;
            bool isInvaildText = HasInvalidData(out value, out error);
            this.Text = value;

            if (isInvaildText && ShowContentError)
            {
                Common.ShowToolTipInfo(this, error);
            }

            if (isInvaildText && ContentType == ContentType.Email)
            {
                isInvaildText = false;
            }

            if (isInvaildText && IsRequired)
            {
                e.Cancel = true;
                TextIsChanged = true;
            }

            SetRequiredStatus();
            if (SetValueToControl != null)
            {
                SetValueToControl.Text = this.Text;
            }

            base.OnValidating(e);
        }
        protected override void OnEnter(EventArgs e)
        {
            if (DesignMode)
                return;
            base.OnEnter(e);
            TextIsChanged = false;
        }
        protected override void OnTextChanged(EventArgs e)
        {
            if (DesignMode)
                return;
            TextIsChanged = true;
            if (ValueChangedEnabled && this.Focused && ValueChangedHandle != null)
                ValueChangedHandle.IsChanged = true;
            base.OnTextChanged(e);
            SetRequiredStatus();
        }
        protected override void OnValidated(EventArgs e)
        {
            if (DesignMode)
                return;
            base.OnValidated(e);
            TextIsChanged = false;
        }
        protected override void OnEnabledChanged(EventArgs e)
        {
            if (DesignMode)
                return;
            base.OnEnabledChanged(e);
            SetRequiredStatus();
        }
        protected override void OnReadOnlyChanged(EventArgs e)
        {
            if (DesignMode)
                return;
            base.OnReadOnlyChanged(e);
            SetRequiredStatus();
        }
        protected override void OnGotFocus(EventArgs e)
        {
            isFirstFocus = true;
            base.OnGotFocus(e);
        }
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            isFirstFocus = false;
        }
        #endregion

        #region Public Function
        /// <summary>
        /// 是否存在无效的数据
        /// </summary>
        /// <returns></returns>
        public bool HasInvalidData()
        {
            string error = string.Empty;
            string value = this.Text;
            return HasInvalidData(out value, out error);
        }
        /// <summary>
        /// 是否存在无效的数据
        /// </summary>
        /// <param name="error">返回错误信息</param>
        /// <returns></returns>
        public bool HasInvalidData(out string error)
        {
            error = string.Empty;
            string value = this.Text;
            return HasInvalidData(out value, out error);
        }

        public void SetRequiredStatus()
        {
            if (DesignMode) return;

            bool tmpReadOnly = ReadOnly;
            Suggest.SuggestBox sg = null;

            if (RequiredHandle != null)
            {
                sg = this.Parent as Suggest.SuggestBox;
                if (sg != null && sg.DropDownStyle == hwj.UserControls.Suggest.SuggestBox.SuggextBoxStyle.DropDownList)
                {
                    tmpReadOnly = false;
                }
            }

            if (IsRequired && string.IsNullOrEmpty(this.Text) && Enabled && !tmpReadOnly)
            {
                if (RequiredHandle != null)
                {
                    sg = this.Parent as Suggest.SuggestBox;
                    if (sg != null)
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
                    sg = this.Parent as Suggest.SuggestBox;
                    if (sg != null)
                        RequiredHandle.Remove(this.Parent);
                    else
                        RequiredHandle.Remove(this);
                }
                this.BackColor = this.OldBackColor;
            }
        }
        #endregion

        #region Private Function
        /// <summary>
        /// 是否存在无效的数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        private bool HasInvalidData(out string value, out string error)
        {
            error = string.Empty;
            value = this.Text;

            switch (ContentType)
            {
                case ContentType.None:
                    break;
                case ContentType.Email:
                    if (!CommonLibrary.Object.EmailHelper.isValidEmail(this.Text))
                    {
                        error = string.Format(Properties.Resources.InvalidEmail, this.Text);
                        return true;
                    }
                    break;
                case ContentType.Numberic:
                    decimal v = 0;
                    if (!decimal.TryParse(this.Text, out v))
                    {
                        error = string.Format(Properties.Resources.InvalidNumberic, this.Text);
                        return true;
                    }
                    if (!string.IsNullOrEmpty(Format))
                    {
                        value = v.ToString(Format);
                    }
                    break;
                case ContentType.Integer:
                    int i = 0;
                    if (!int.TryParse(this.Text, out i))
                    {
                        error = string.Format(Properties.Resources.InvalidNumberic, this.Text);
                        return true;
                    }
                    if (!string.IsNullOrEmpty(Format))
                    {
                        value = i.ToString(Format);
                    }
                    else
                    {
                        value = i.ToString();
                    }
                    break;
                default:
                    break;
            }

            return false;

        }
        private bool IsNegatives()
        {
            if (ContentType == ContentType.Numberic && this.Text.IndexOf('-') != -1)
                return true;
            else
                return false;
        }
        #endregion

        #region IValueChanged Members

        /// <summary>
        /// 获取或设置ValueChanged事件的IsChanged属性
        /// </summary>
        [DefaultValue(true), Description("获取或设置ValueChanged事件的IsChanged属性")]
        public bool ValueChangedEnabled { get; set; }

        #endregion
    }

}
