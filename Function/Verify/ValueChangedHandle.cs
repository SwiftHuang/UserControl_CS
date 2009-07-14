﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hwj.UserControls.Function.Verify
{
    public class ValueChangedHandle
    {
        public event EventHandler xValueChanged;

        #region Property
        internal bool Enabled { get; set; }
        private bool _IsChanged = true;
        /// <summary>
        /// 获取是否有控件的值发生改变
        /// </summary>
        public bool IsChanged
        {
            get { return _IsChanged; }
            set
            {
                if (Enabled && _IsChanged == false && value && xValueChanged != null)
                    xValueChanged(null, null);
                if (Enabled)
                    _IsChanged = value;
                else
                    _IsChanged = false;
            }
        }
        #endregion

        public ValueChangedHandle()
        {
            Initialize();
        }

        /// <summary>
        /// 初始化控件值的改变状态
        /// </summary>
        public void Initialize()
        {
            Enabled = true;
            IsChanged = false;
        }
        /// <summary>
        /// 销毁控件值的改变状态(不需要运用ValueChanged判断的时候，请销毁该状态来提高控件性能)
        /// </summary>
        public void Dispose()
        {
            Enabled = false;
            _IsChanged = true;
        }
    }
}
