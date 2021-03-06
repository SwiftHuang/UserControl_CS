﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using hwj.UserControls.Interface;

namespace hwj.UserControls.CommonControls
{
    public class xDateTimePicker : DateTimePicker, IEnterEqualTab, IValueChanged
    {
        #region Property
        [DefaultValue(Enums.DateFormat.None)]
        public Enums.DateFormat DateFormat { get; set; }
        [DefaultValue(true)]
        public bool EnterEqualTab { get; set; }
        /// <summary>
        /// 设置引发hwj.UserControls.ValueChanged事件的对象
        /// </summary>
        [DefaultValue(null), Description("设置引发hwj.UserControls.ValueChanged事件的对象"), Browsable(false)]
        protected Function.Verify.ValueChangedHandle ValueChangedHandle { get; set; }

        [Description("当值改变时,同时赋值给指定的控件")]
        public xDateTimePicker SetValueToControl { get; set; }

        //private DateTimePickerFormat tmpFormat = DateTimePickerFormat.Custom;
        //private string tmpCustomFormat = string.Empty;
        //public string InputFormat { get; set; }
        #endregion

        public xDateTimePicker()
        {
            EnterEqualTab = true;
            ValueChangedHandle = Common.ValueChanged;
            SetValueToControl = null;
            ValueChangedEnabled = true;
        }

        #region Override Function
        protected override void OnCreateControl()
        {
            switch (DateFormat)
            {
                case Enums.DateFormat.None:
                    break;
                case Enums.DateFormat.Date:
                    this.Format = DateTimePickerFormat.Custom;
                    this.CustomFormat = Common.Format_Date;
                    break;
                case Enums.DateFormat.DateTime:
                    this.Format = DateTimePickerFormat.Custom;
                    this.CustomFormat = Common.Format_DateTime;
                    break;
                default:
                    break;
            }
            base.OnCreateControl();
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (EnterEqualTab && e.KeyCode == Keys.Enter)
                SendKeys.Send("{Tab}");
            base.OnKeyDown(e);
        }
        protected override void OnValueChanged(EventArgs eventargs)
        {
            if (ValueChangedEnabled && this.Focused && ValueChangedHandle != null)
                ValueChangedHandle.IsChanged = true;
            if (SetValueToControl != null)
            {
                SetValueToControl.ShowCheckBox = this.ShowCheckBox;
                SetValueToControl.Checked = this.Checked;
                SetValueToControl.Text = this.Text;
                SetValueToControl.Value = this.Value;
            }
            base.OnValueChanged(eventargs);
        }
        protected override void OnCloseUp(EventArgs eventargs)
        {
            if (ValueChangedEnabled && ValueChangedHandle != null)
                ValueChangedHandle.IsChanged = true;
            base.OnCloseUp(eventargs);
        }
        //protected override void OnGotFocus(EventArgs e)
        //{
        //    base.OnGotFocus(e);
        //    if (!string.IsNullOrEmpty(InputFormat))
        //    {
        //        tmpFormat = this.Format;
        //        tmpCustomFormat = this.CustomFormat;
        //        this.Format = DateTimePickerFormat.Custom;
        //        this.CustomFormat = InputFormat;
        //    }
        //}
        //protected override void OnLeave(EventArgs e)
        //{
        //    base.OnLeave(e);
        //    if (!string.IsNullOrEmpty(InputFormat))
        //    {
        //        this.Format = tmpFormat;
        //        this.CustomFormat = tmpCustomFormat;
        //        this.Value = hwj.CommonLibrary.Object.DateHelper.ToDate(this.Text, InputFormat);
        //    }
        //}

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
