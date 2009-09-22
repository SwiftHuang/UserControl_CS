using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using hwj.UserControls.Interface;

namespace hwj.UserControls.CommonControls
{
    public class xDateTimePicker : DateTimePicker, IEnterEqualTab
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
        #endregion

        public xDateTimePicker()
        {
            EnterEqualTab = true;
            ValueChangedHandle = Common.ValueChanged;
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
            if (this.Focused && ValueChangedHandle != null)
                ValueChangedHandle.IsChanged = true;
            base.OnValueChanged(eventargs);
        }
        protected override void OnCloseUp(EventArgs eventargs)
        {
            if (ValueChangedHandle != null)
                ValueChangedHandle.IsChanged = true;
            base.OnCloseUp(eventargs);
        }
        #endregion
    }
}
