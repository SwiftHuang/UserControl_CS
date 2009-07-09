﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace hwj.UserControls.CommonControls
{
    public class xDateTimePicker : DateTimePicker, ICommonControls
    {

        [DefaultValue(Enums.DateFormat.None)]
        public Enums.DateFormat DateFormat { get; set; }
        [DefaultValue(true)]
        public bool EnterEqualTab { get; set; }
        [DefaultValue(false)]
        public bool IsRequired { get; set; }

        public xDateTimePicker()
        {
            EnterEqualTab = true;
            IsRequired = false;
        }
        protected override void OnCreateControl()
        {
            if (IsRequired)
                this.BackColor = Common.RequiredBackColor;
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
    }
}