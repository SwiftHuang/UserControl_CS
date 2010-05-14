﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using hwj.UserControls.Interface;

namespace hwj.UserControls.Other
{
    public partial class MaskedDateTimePicker : UserControl, IEnterEqualTab
    {
        private bool isFirstFocus = false;
        private ToolStripDropDown tsDropDown = null;
        private ToolStripControlHost tsCtrlHost = null;
        private MonthCalendar monthCalendar = new MonthCalendar();
        private DateTime LastDateTime = new DateTime();
        private Color OldBackColor;
        public event EventHandler ValueChanged;

        #region Property
        public bool Checked
        {
            get
            {
                if (chkBox != null)
                    return chkBox.Checked;
                else
                    return false;
            }
            set
            {
                if (chkBox != null)
                    chkBox.Checked = value;
            }
        }
        public string Format { get; set; }

        private bool _showCheckBox = false;
        [Description("获取或设置一个值，该值指示在选定日期的左侧是否显示一个复选框。"), DefaultValue(false)]
        public bool ShowCheckBox
        {
            get { return _showCheckBox; }
            set
            {
                _showCheckBox = value;
                if (this.Created || this.DesignMode)
                {
                    chkBox.Visible = value;
                    if (value)
                    {
                        mTxtValue.Enabled = chkBox.Checked;
                        Point p = new Point(19, 3);
                        mTxtValue.Location = p;
                        mTxtValue.Width = this.Width - 40;
                        chkBox.CheckedChanged += new EventHandler(chkBox_CheckedChanged);
                    }
                    else
                    {
                        mTxtValue.Enabled = true;
                        Point p = new Point(3, 3);
                        mTxtValue.Location = p;
                        mTxtValue.Width = this.Width + 40;
                        chkBox.CheckedChanged -= new EventHandler(chkBox_CheckedChanged);
                    }
                }
            }
        }

        private DateTime _value = DateTime.Now;
        public DateTime Value
        {
            get
            {
                if (DesignMode)
                    return DateTime.Now;

                if (ShowCheckBox)
                {
                    if (chkBox.Checked)
                        return _value;
                    else
                        return DateTime.MinValue;
                }
                else
                    return _value;
            }
            set
            {
                LastDateTime = _value;
                _value = value;
                SetValue();

                if (this.Created && LastDateTime != value)
                {
                    SetOtherControl();
                    SetRequiredStatus();
                    if (ValueChanged != null)
                        ValueChanged(this, null);
                }
            }
        }
        [Browsable(false)]
        public string Text
        {
            get { return mTxtValue.Text; }
            set { mTxtValue.Text = value; }
        }

        [Description("当值改变时,同时赋值给指定的控件")]
        public MaskedDateTimePicker SetValueToControl { get; set; }

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
        [DefaultValue(null), Description("设置引发hwj.UserControls.ValueChanged事件的对象"), Browsable(false)]
        protected Function.Verify.ValueChangedHandle ValueChangedHandle { get; set; }

        protected Function.Verify.RequiredHandle RequiredHandle { get; set; }

        [DefaultValue(true)]
        public bool EnterEqualTab { get; set; }

        [DefaultValue(Enums.DateFormat.None)]
        public Enums.DateFormat DateFormat { get; set; }

        #endregion

        public MaskedDateTimePicker()
        {
            InitializeComponent();
            ShowCheckBox = false;
            SetValueToControl = null;
            EnterEqualTab = true;
            OldBackColor = this.mTxtValue.BackColor;

            if (!DesignMode)
            {
                monthCalendar.DateSelected += new DateRangeEventHandler(monthCalendar_DateSelected);
                monthCalendar.VisibleChanged += new EventHandler(monthCalendar_VisibleChanged);

                mTxtValue.Click += new EventHandler(mTxtValue_Click);
                mTxtValue.ValidatingType = typeof(System.DateTime);
                mTxtValue.TypeValidationCompleted += new TypeValidationEventHandler(mTxtValue_TypeValidationCompleted);
                mTxtValue.EnabledChanged += new EventHandler(mTxtValue_EnabledChanged);
                mTxtValue.KeyDown += new KeyEventHandler(mTxtValue_KeyDown);
                mTxtValue.TextChanged += new EventHandler(mTxtValue_TextChanged);
                mTxtValue.LostFocus += new EventHandler(mTxtValue_LostFocus);
                mTxtValue.GotFocus += new EventHandler(mTxtValue_GotFocus);

                tsCtrlHost = new ToolStripControlHost(monthCalendar);
                tsCtrlHost.Padding = new Padding(0);
                tsCtrlHost.Margin = new Padding(0);
                tsCtrlHost.Dock = DockStyle.Fill;

                tsDropDown = new ToolStripDropDown();
                tsDropDown.Padding = new Padding(0);
                tsDropDown.DropShadowEnabled = true;
                tsDropDown.Items.Add(tsCtrlHost);
                tsDropDown.AutoClose = true;

            }
            switch (DateFormat)
            {
                case Enums.DateFormat.None:
                    break;
                case Enums.DateFormat.Date:
                    Format = Common.Format_Date;
                    break;
                case Enums.DateFormat.DateTime:
                    Format = Common.Format_DateTime;
                    break;
                default:
                    break;
            }
            if (string.IsNullOrEmpty(Format))
                Format = Common.Format_Date;

            mTxtValue.Mask = Regex.Replace(Format, @"[a-zA-Z0-9]", "0");
            if (_value == DateTime.MinValue)
                _value = DateTime.Now;
            SetValue();
            LastDateTime = Value;

        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            ShowCheckBox = ShowCheckBox;
            ValueChangedHandle = Common.ValueChanged;
            RequiredHandle = Common.Required;
            SetRequiredStatus();
        }
        protected override void OnBackColorChanged(EventArgs e)
        {
            mTxtValue.BackColor = this.BackColor;
            base.OnBackColorChanged(e);
        }

        #region Private Event Function
        private void pBox_Click(object sender, EventArgs e)
        {
            if (ShowCheckBox)
                chkBox.Checked = true;
            ShowList(sender, e);
        }
        void monthCalendar_DateSelected(object sender, DateRangeEventArgs e)
        {
            Value = monthCalendar.SelectionStart;
            SetValueChangedHandle();
            CloseList();
        }
        void monthCalendar_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Value != DateTime.MinValue)
            {
                monthCalendar.SelectionStart = this.Value;
                monthCalendar.SelectionEnd = this.Value;
            }
        }

        void mTxtValue_TypeValidationCompleted(object sender, TypeValidationEventArgs e)
        {
            if (e.IsValidInput)
            {
                LastDateTime = Value;
                Value = (DateTime)e.ReturnValue;
                SetOtherControl();
            }
            else
            {
                mTxtValue.ResetText();
                _value = DateTime.MinValue;
                Common.ShowToolTipInfo(this, Properties.Resources.InvalidDate);
            }
            SetValueChangedHandle();
            SetRequiredStatus();
        }
        void mTxtValue_EnabledChanged(object sender, EventArgs e)
        {
            if (!mTxtValue.Enabled)
            {
                mTxtValue.BackColor = SystemColors.Window;
            }
        }
        void mTxtValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (EnterEqualTab && e.KeyCode == Keys.Enter)
                SendKeys.Send("{Tab}");
            base.OnKeyDown(e);
        }
        void mTxtValue_TextChanged(object sender, EventArgs e)
        {
            if (this.Focused && ValueChangedHandle != null)
                ValueChangedHandle.IsChanged = true;
        }
        void mTxtValue_Click(object sender, EventArgs e)
        {
            if (isFirstFocus)
                mTxtValue.SelectAll();
            isFirstFocus = false;
        }
        void mTxtValue_LostFocus(object sender, EventArgs e)
        {
            isFirstFocus = true;
        }
        void mTxtValue_GotFocus(object sender, EventArgs e)
        {
            isFirstFocus = true;
        }

        void ParentForm_Move(object sender, EventArgs e)
        {
            CloseList();
        }
        void chkBox_CheckedChanged(object sender, EventArgs e)
        {
            mTxtValue.Enabled = chkBox.Checked;
        }
        #endregion

        #region Private Function
        private void ShowList(object sender, EventArgs e)
        {

            if (tsDropDown != null)
            {
                if (this.ParentForm != null)
                    this.ParentForm.Move += new EventHandler(ParentForm_Move);
                tsDropDown.Show(this, -2, this.Height - 2);
                Focus();
            }

        }
        private void CloseList()
        {
            if (tsDropDown == null)
                return;

            tsDropDown.Close();
        }
        private void SetOtherControl()
        {
            if (SetValueToControl != null)
            {
                if (SetValueToControl.ShowCheckBox)
                {
                    SetValueToControl.ShowCheckBox = this.ShowCheckBox;
                    SetValueToControl.Checked = this.Checked;
                }
                SetValueToControl.Value = _value;
            }
        }
        private void SetRequiredStatus()
        {
            if (DesignMode) return;
            if (RequiredHandle == null) return;

            if (IsRequired && this.Value == DateTime.MinValue)
            {
                RequiredHandle.Add(this);
                BackColor = Common.RequiredBackColor;
            }
            else
            {
                RequiredHandle.Remove(this);
                BackColor = this.OldBackColor;
            }
        }
        private void SetValueChangedHandle()
        {
            if (ValueChangedHandle != null)
                ValueChangedHandle.IsChanged = true;
        }
        private void SetValue()
        {
            string tmp = _value.ToString(Format);
            if (tmp.Length > mTxtValue.Text.Length)
                mTxtValue.Mask = Regex.Replace(tmp, @"[a-zA-Z0-9]", "0");
            mTxtValue.Text = _value.ToString(Format);
        }
        #endregion

    }
}
