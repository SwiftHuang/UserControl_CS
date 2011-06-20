using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using hwj.UserControls.Interface;

namespace hwj.UserControls.Other
{
    public class EmailBox : TextBox, IEnterEqualTab, IValueChanged
    {
        /// <summary>
        /// 
        /// </summary>
        public enum EmailspliterEnum : byte
        {

            /// <summary>
            ///真没有 
            /// </summary>
            None = 0,
            /// <summary>
            ///逗号 
            /// </summary>
            Comma = 0x2C,
            /// <summary>
            /// 分号
            /// </summary>
            Semicolon = 0x3B,
            /// <summary>
            /// 换行
            /// </summary>
            Enter = 0x0D,
            /// <summary>
            /// 自定义
            /// </summary>
            Custom,
        }

        #region Property
        [DefaultValue(EmailspliterEnum.None), Description("设置输入多个Email地址时的分隔符,None为单个Email地址")]
        public EmailspliterEnum EmailSpliter { get; set; }

        [Browsable(false)]
        private string emailSplitChar
        {
            get
            {
                if (EmailSpliter == EmailspliterEnum.None)
                    return string.Empty;
                else if (EmailSpliter == EmailspliterEnum.Enter)
                    return "\r\n";
                else if (EmailSpliter != EmailspliterEnum.Custom)
                    return Convert.ToChar(EmailSpliter).ToString();
                else
                    return CustomEmailSplitString;
            }
        }

        public string CustomEmailSplitString { get; set; }


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
        /// "是否显示验证错误信息"
        /// </summary>
        [DefaultValue(true), Description("是否显示验证错误信息")]
        public bool ShowContentError { get; set; }

        /// <summary>
        /// 内容是否改变(在Validating/Validated有效)
        /// </summary>
        [Browsable(false)]
        public bool TextIsChanged { get; set; }

        [Description("当值改变时,同时赋值给指定的控件")]
        public EmailBox SetValueToControl { get; set; }

        [DefaultValue(false), Description("当获取焦点时,自动全选")]
        public bool AutoSelectAll { get; set; }
        #endregion

        public EmailBox()
        {
            Properties.Resources.Culture = Thread.CurrentThread.CurrentUICulture;
            ShowContentError = true;

            this.Disposed += new EventHandler(xEmailBox_Disposed);
            TextIsChanged = false;
            OldBackColor = this.BackColor;
            IsRequired = false;
            EnterEqualTab = true;
            SetValueToControl = null;
            ValueChangedEnabled = true;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (DesignMode)
                return;
            if (EnterEqualTab && e.KeyCode == Keys.Enter)
                if (e.KeyData != (Keys.Control | Keys.Enter))
                    SendKeys.Send("{Tab}");
            base.OnKeyDown(e);
        }
        protected override void OnClick(EventArgs e)
        {
            if (DesignMode)
                return;
            if (AutoSelectAll && isFirstFocus)
                this.SelectAll();
            isFirstFocus = false;
            base.OnClick(e);
        }
        protected override void OnValidating(CancelEventArgs e)
        {
            if (DesignMode)
                return;
            bool isInvaildText = false;
            List<string> errList = new List<string>();
            if (!string.IsNullOrEmpty(Text) && !CommonLibrary.Object.EmailHelper.isValidEmails(this.Text, emailSplitChar, out errList))
            {
                if (ShowContentError)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(string.Format(Properties.Resources.InvalidEmail, string.Empty));
                    foreach (string str in errList)
                    {
                        sb.AppendLine("-" + str);
                    }
                    Common.ShowToolTipInfo(this, sb.ToString());
                }
                //this.Text = string.Empty;
                isInvaildText = true;
            }

            if (isInvaildText && IsRequired)
            {
                e.Cancel = true;
                TextIsChanged = true;
            }
            SetRequiredStatus();
            if (SetValueToControl != null)
                SetValueToControl.Text = this.Text;
            base.OnValidating(e);
        }
        protected override void OnEnter(EventArgs e)
        {
            if (string.IsNullOrEmpty(Text) && EmailSpliter != EmailspliterEnum.None)
            {
                Common.ShowToolTipInfo(this, string.Format(Properties.Resources.EamilTips, emailSplitChar == "\r\n" ? "Enter" : emailSplitChar));
            }
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

        public bool CheckData()
        {
            string err = string.Empty;
            return CheckData(out err);
        }

        public bool CheckData(out string errmsg)
        {
            errmsg = string.Empty;
            if (DesignMode)
                return true;
            bool isVaildText = true;
            List<string> errList = new List<string>();
            if (!CommonLibrary.Object.EmailHelper.isValidEmails(this.Text, Convert.ToChar(EmailSpliter).ToString(), out errList))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(string.Format(Properties.Resources.InvalidEmail, string.Empty));
                foreach (string str in errList)
                {
                    sb.AppendLine("-" + str);
                }
                errmsg = sb.ToString();
                isVaildText = false;
            }

            return isVaildText;

        }
        void xEmailBox_Disposed(object sender, EventArgs e)
        {
            Common.HideToolTip();
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

        #region IValueChanged Members

        /// <summary>
        /// 获取或设置ValueChanged事件的IsChanged属性
        /// </summary>
        [DefaultValue(true), Description("获取或设置ValueChanged事件的IsChanged属性")]
        public bool ValueChangedEnabled { get; set; }

        #endregion
    }
}
