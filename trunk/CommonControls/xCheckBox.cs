﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using hwj.UserControls.Interface;

namespace hwj.UserControls.CommonControls
{
    public class xCheckBox : CheckBox, IEnterEqualTab
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

        public xCheckBox()
        {
            EnterEqualTab = true;
            ValueChangedHandle = Common.ValueChanged;
        }

        #region Override Function
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (EnterEqualTab && e.KeyCode == Keys.Enter)
                SendKeys.Send("{Tab}");
            base.OnKeyDown(e);
        }
        protected override void OnCheckedChanged(EventArgs e)
        {
            if (this.Focused && ValueChangedHandle != null)
                ValueChangedHandle.IsChanged = true;
            base.OnCheckedChanged(e);
        }
        #endregion
    }
}
