using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using hwj.UserControls.Interface;

namespace hwj.UserControls.CommonControls
{
    public class xNumericUpDown : NumericUpDown, IEnterEqualTab, IValueChanged
    {
        #region Property
        [DefaultValue(true)]
        public bool EnterEqualTab { get; set; }
        /// <summary>
        /// 设置引发hwj.UserControls.ValueChanged事件的对象
        /// </summary>
        [DefaultValue(null), Description("设置引发hwj.UserControls.ValueChanged事件的对象"), Browsable(false)]
        protected Function.Verify.ValueChangedHandle ValueChangedHandle { get; set; }
        #endregion

        public xNumericUpDown()
        {
            EnterEqualTab = true;
            ValueChangedHandle = Common.ValueChanged;
            ValueChangedEnabled = true;
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
            base.OnValueChanged(e);
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
