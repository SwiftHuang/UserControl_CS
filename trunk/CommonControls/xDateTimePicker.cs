using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace hwj.UserControls.CommonControls
{
    public class xDateTimePicker : DateTimePicker, ICommonControls
    {
        public enum DateTimeTypes
        {
            None,
            CN,
            HK,
        }
        [DefaultValue(DateTimeTypes.None)]
        public DateTimeTypes DateTimeType { get; set; }
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
            switch (DateTimeType)
            {
                case DateTimeTypes.None:
                    break;
                case DateTimeTypes.CN:
                    this.Format = DateTimePickerFormat.Custom;
                    this.CustomFormat = "yyyy-MM-dd";
                    break;
                case DateTimeTypes.HK:
                    this.Format = DateTimePickerFormat.Custom;
                    this.CustomFormat = "dd/MM/yyyy";
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
