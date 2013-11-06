using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using hwj.UserControls.Interface;
using System.Globalization;

namespace hwj.UserControls.Other
{
    public partial class MaskedDateTimePicker : UserControl, IEnterEqualTab, IValueChanged
    {
        public enum DataTimeConditions
        {
            Always,
            WhenNull,
            WhenGreater,
            WhenLess,
        }

        private bool isFirstFocus = false;
        private ToolStripDropDown tsDropDown = null;
        private ToolStripControlHost tsCtrlHost = null;
        private MonthCalendar monthCalendar = new MonthCalendar();
        private DateTime LastDateTime = new DateTime();
        private Color OldBackColor;
        public event EventHandler ValueChanged;

        private DateTime max = DateTime.MaxValue;
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly DateTime MaxDateTime = new DateTime(0x270e, 12, 0x1f);
        private DateTime min = DateTime.MinValue;

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

        [DefaultValue(Enums.DateFormat.None)]
        public Enums.DateFormat DateFormat { get; set; }
        private string _Format = string.Empty;
        public string Format
        {
            get { return _Format; }
            set
            {
                if (DateFormat == Enums.DateFormat.None)
                    _Format = value;
                else
                    _Format = Enums.GetFormat(DateFormat);
                if (!DesignMode)
                    SetValue();
            }
        }

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
        [DesignOnly(true)]
        public new DateTime Value
        {
            get
            {
                if (DesignMode)
                    return DateTime.Now;

                if (ShowCheckBox)
                {
                    if (!chkBox.Checked)
                        return DateTime.MinValue;
                }
                return _value;
                //return Convert.ToDateTime(_value.ToString(System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern));
            }
            set
            {
                LastDateTime = _value;
                _value = value;
                SetValue();

                if (this.Created && LastDateTime != value)
                {
                    if (ShowCheckBox)
                        this.chkBox.Checked = true;
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
        [Description("当值改变时,同时赋值给指定的控件的条件")]
        public DataTimeConditions SetValueToControlCondition { get; set; }

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

        [DefaultValue(typeof(DateTime), "9998-12-31")]
        public DateTime MaxDate
        {
            get
            {
                return EffectiveMaxDate(this.max);
            }
            set
            {
                if (value != this.max)
                {
                    if (value < EffectiveMinDate(this.min))
                    {
                        throw new ArgumentOutOfRangeException("MaxDate");
                    }
                    if (value > MaximumDateTime)
                    {
                        throw new ArgumentOutOfRangeException("MaxDate");
                    }
                    this.max = value;
                    //this.SetRange();
                    if (this.Value > this.max)
                    {
                        this.Value = this.max;
                    }
                }
            }
        }
        public static DateTime MaximumDateTime
        {
            get
            {
                return MaxDateTime;
            }
        }
        [DefaultValue(typeof(DateTime), "1753-1-1")]
        public DateTime MinDate
        {
            get
            {
                return EffectiveMinDate(this.min);
            }
            set
            {
                if (value != this.min)
                {
                    if (value > EffectiveMaxDate(this.max))
                    {
                        throw new ArgumentOutOfRangeException("MinDate");
                    }
                    if (value < MinimumDateTime)
                    {
                        throw new ArgumentOutOfRangeException("MinDate");
                    }
                    this.min = value;
                    //this.SetRange();
                    if (this.Value < this.min)
                    {
                        this.Value = this.min;
                    }
                }
            }
        }

        public static DateTime MinimumDateTime
        {
            get
            {
                DateTime minSupportedDateTime = CultureInfo.CurrentCulture.Calendar.MinSupportedDateTime;
                if (minSupportedDateTime.Year < 0x6d9)
                {
                    return new DateTime(0x6d9, 1, 1);
                }
                return minSupportedDateTime;
            }
        }
        #endregion

        public MaskedDateTimePicker()
        {
            InitializeComponent();
            ShowCheckBox = false;
            SetValueToControl = null;
            EnterEqualTab = true;
            OldBackColor = this.mTxtValue.BackColor;
            ValueChangedEnabled = true;

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
                //tsDropDown.AutoClose = true;

            }
            Format = Enums.GetFormat(DateFormat);
            if (string.IsNullOrEmpty(Format))
                Format = Common.Format_Date;

            mTxtValue.Mask = Regex.Replace(Format, @"[a-zA-Z0-9]", "0");
            if (_value == DateTime.MinValue)
                _value = DateTime.Now;
            SetValue();
            LastDateTime = Value;
            this.Disposed += new EventHandler(MaskedDateTimePicker_Disposed);

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
        protected override void OnGotFocus(EventArgs e)
        {
            if (ShowCheckBox)
            {
                //this.chkBox.CanSelect = true;
                this.chkBox.Focus();
            }
            base.OnGotFocus(e);
        }

        #region Private Event Function
        private void pBox_Click(object sender, EventArgs e)
        {
            //if (ShowCheckBox)
            //    chkBox.Checked = true;
            ShowList(sender, e);
        }
        private void MaskedDateTimePicker_Leave(object sender, EventArgs e)
        {
            CloseList();
        }
        private void MaskedDateTimePicker_Disposed(object sender, EventArgs e)
        {
            Common.HideToolTip();
            if (tsDropDown != null && tsDropDown.IsHandleCreated)
            {
                tsDropDown.Dispose();
            }
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
            bool isValid = e.IsValidInput;
            if (isValid)
            {
                DateTime d = (DateTime)e.ReturnValue;
                if (d < MinDate || d > MaxDate)
                {
                    isValid = false;
                }
                else
                {
                    LastDateTime = Value;
                    Value = d;
                    SetOtherControl();
                }
            }

            if (!isValid)
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
            if (ValueChangedEnabled && this.Focused && ValueChangedHandle != null)
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
            this.mTxtValue.SelectAll();
        }

        void ParentForm_Move(object sender, EventArgs e)
        {
            CloseList();
        }
        void chkBox_CheckedChanged(object sender, EventArgs e)
        {
            mTxtValue.Enabled = chkBox.Checked;
            if (!DesignMode && ValueChangedEnabled && ValueChangedHandle != null)
                ValueChangedHandle.IsChanged = true;
        }
        #endregion

        #region Private Function
        private void ShowList(object sender, EventArgs e)
        {

            if (tsDropDown != null)
            {
                if (tsDropDown.AutoClose)
                    tsDropDown.AutoClose = false;

                if (this.ParentForm != null)
                {
                    this.ParentForm.Move -= new EventHandler(ParentForm_Move);
                    this.ParentForm.Move += new EventHandler(ParentForm_Move);
                }
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
                switch (SetValueToControlCondition)
                {
                    case DataTimeConditions.Always:
                        SetBrotherValue();
                        break;
                    case DataTimeConditions.WhenGreater:
                        if (this.Value.CompareTo(SetValueToControl.Value) > 0)
                            SetBrotherValue();
                        break;
                    case DataTimeConditions.WhenLess:
                        if (this.Value.CompareTo(SetValueToControl.Value) < 0)
                            SetBrotherValue();
                        break;
                    case DataTimeConditions.WhenNull:
                        if (SetValueToControl.Value == DateTime.MinValue)
                            SetBrotherValue();
                        break;
                }
            }
        }
        private void SetBrotherValue()
        {
            if (SetValueToControl.ShowCheckBox)
            {
                SetValueToControl.ShowCheckBox = this.ShowCheckBox;
                SetValueToControl.Checked = this.Checked;
            }
            SetValueToControl.Value = _value;
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
            if (ValueChangedEnabled && ValueChangedHandle != null)
                ValueChangedHandle.IsChanged = true;
        }
        private void SetValue()
        {
            if (!string.IsNullOrEmpty(Format))
            {
                string tmp = _value.ToString(Format);
                //if (tmp.Length > mTxtValue.Text.Length)
                mTxtValue.Mask = Regex.Replace(tmp, @"[a-zA-Z0-9]", "0");
                mTxtValue.Text = _value.ToString(Format);
            }
        }
        #endregion

        internal static DateTime EffectiveMaxDate(DateTime maxDate)
        {
            DateTime maximumDateTime = MaximumDateTime;
            if (maxDate > maximumDateTime)
            {
                return maximumDateTime;
            }
            return maxDate;
        }

        internal static DateTime EffectiveMinDate(DateTime minDate)
        {
            DateTime minimumDateTime = MinimumDateTime;
            if (minDate < minimumDateTime)
            {
                return minimumDateTime;
            }
            return minDate;
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
