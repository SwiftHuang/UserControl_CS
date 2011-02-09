using System;
using System.Collections.Generic;
using System.Text;
using hwj.UserControls.Function.Verify;

namespace hwj.UserControls.Interface
{
    interface IValueChanged
    {
        //ValueChangedHandle ValueChangedHandle { get; }
        /// <summary>
        /// 获取或设置触发ValueChanged事件
        /// </summary>
        bool ValueChangedEnabled { get; set; }
    }
}
