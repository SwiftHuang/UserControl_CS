﻿using System;
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

        [DefaultValue(false), Description("当获取焦点时,自动全选")]
        public bool AutoSelectAll { get; set; }
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
            RequiredHandle = Common.Required;
            ValueChangedHandle = Common.ValueChanged;

            SetRequiredStatus();
        }
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (DesignMode)
                return;
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
            bool isInvaildText = false;
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
                        isInvaildText = true;
                    }
                    break;
                case ContentType.Numberic:
                    decimal v = 0;
                    if (!decimal.TryParse(this.Text, out v))
                    {
                        if (ShowContentError)
                            Common.ShowToolTipInfo(this, string.Format(Properties.Resources.InvalidNumberic, this.Text));
                        isInvaildText = true;
                    }
                    if (!string.IsNullOrEmpty(Format))
                        this.Text = v.ToString(Format);
                    break;
                case ContentType.Integer:
                    int i = 0;
                    if (!int.TryParse(this.Text, out i))
                    {
                        if (ShowContentError)
                            Common.ShowToolTipInfo(this, string.Format(Properties.Resources.InvalidNumberic, this.Text));
                        isInvaildText = true;
                    }
                    if (!string.IsNullOrEmpty(Format))
                        this.Text = i.ToString(Format);
                    break;
                default:
                    break;
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
            if (this.Focused && ValueChangedHandle != null)
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
        private bool IsNegatives()
        {
            if (ContentType == ContentType.Numberic && this.Text.IndexOf('-') != -1)
                return true;
            else
                return false;
        }
    }

}
