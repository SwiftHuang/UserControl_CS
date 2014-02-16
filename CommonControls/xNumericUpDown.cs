using hwj.UserControls.Interface;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace hwj.UserControls.CommonControls
{
    public class xNumericUpDown : NumericUpDown, IEnterEqualTab, IValueChanged
    {
        #region Property

        private bool isFirstFocus = false;

        [DefaultValue(true)]
        public bool EnterEqualTab { get; set; }

        /// <summary>
        /// 设置引发hwj.UserControls.ValueChanged事件的对象
        /// </summary>
        [DefaultValue(null), Description("设置引发hwj.UserControls.ValueChanged事件的对象"), Browsable(false)]
        protected Function.Verify.ValueChangedHandle ValueChangedHandle { get; set; }

        [Description("当值改变时,同时赋值给指定的控件")]
        public xNumericUpDown SetValueToControl { get; set; }

        [DefaultValue(false), Description("当获取焦点时,自动全选")]
        public bool AutoSelectAll { get; set; }

        #endregion Property

        public xNumericUpDown()
        {
            EnterEqualTab = true;
            ValueChangedHandle = Common.ValueChanged;
            ValueChangedEnabled = true;
            SetValueToControl = null;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (EnterEqualTab && e.KeyCode == Keys.Enter)
                SendKeys.Send("{Tab}");
            base.OnKeyDown(e);
        }

        protected override void OnValueChanged(EventArgs e)
        {
            if (ValueChangedEnabled && this.Focused && ValueChangedHandle != null)
                ValueChangedHandle.IsChanged = true;
            if (SetValueToControl != null)
            {
                SetValueToControl.Value = this.Value;
            }
            base.OnValueChanged(e);
        }

        protected override void OnClick(EventArgs e)
        {
            if (AutoSelectAll && isFirstFocus && this.Value != null)
            {
                this.Select(0, this.Value.ToString().Length);
            }
            isFirstFocus = false;
            base.OnClick(e);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            if (AutoSelectAll && this.Value != null)
            {
                this.Select(0, this.Value.ToString().Length);
            }
            isFirstFocus = true;
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            isFirstFocus = false;
        }

        #region IValueChanged Members

        /// <summary>
        /// 获取或设置ValueChanged事件的IsChanged属性
        /// </summary>
        [DefaultValue(true), Description("获取或设置ValueChanged事件的IsChanged属性")]
        public bool ValueChangedEnabled { get; set; }

        #endregion IValueChanged Members
    }
}