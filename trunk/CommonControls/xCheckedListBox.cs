using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using hwj.UserControls.Interface;
using System.ComponentModel;

namespace hwj.UserControls.CommonControls
{
    public class xCheckedListBox : CheckedListBox, IValueChanged
    {
        #region Property
        /// <summary>
        /// 设置引发hwj.UserControls.ValueChanged事件的对象
        /// </summary>
        [DefaultValue(null), Description("设置引发hwj.UserControls.ValueChanged事件的对象"), Browsable(false)]
        protected Function.Verify.ValueChangedHandle ValueChangedHandle { get; set; }
        #endregion

        public xCheckedListBox()
        {
            ValueChangedHandle = Common.ValueChanged;
            ValueChangedEnabled = true;
        }

        protected override void OnItemCheck(ItemCheckEventArgs ice)
        {
            if (ice.CurrentValue != ice.NewValue)
            {
                if (ValueChangedEnabled && this.Focused && ValueChangedHandle != null)
                {
                    ValueChangedHandle.IsChanged = true;
                }
            }
            base.OnItemCheck(ice);
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
