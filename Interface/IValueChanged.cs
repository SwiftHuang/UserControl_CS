using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hwj.UserControls.Interface
{
    interface IValueChanged
    {
        /// <summary>
        /// 设置引发hwj.UserControls.ValueChanged事件的对象
        /// </summary>
        Function.Verify.ValueChangedHandle ValueChangedHandle { get; set; }
    }
}
