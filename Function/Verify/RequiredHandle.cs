using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;

namespace hwj.UserControls.Function.Verify
{
    public class RequiredHandle
    {
        #region Property
        /// <summary>
        /// 获取或设置必填的控件名称
        /// </summary>
        public List<RequiredInfo> RequiredControls { get; private set; }
        /// <summary>
        /// 获取是否存在没完整填写的控件
        /// </summary>
        public bool HasRequired
        {
            get
            {
                if (RequiredControls != null && RequiredControls.Count > 0)
                {
                    SetBackColor(RequiredControls);
                    return true;
                }
                else
                    return false;
            }

        }
        #endregion

        public RequiredHandle()
        {
            RequiredControls = new List<RequiredInfo>();
            if (Common.Required != null)
                Common.Required = null;
            Common.Required = this;
        }

        public void SetCheckObject()
        {
            Common.Required = this;
        }
        public void ClearCheckObject()
        {
            Common.Required = null;
        }
        /// <summary>
        /// 添加必填控件
        /// </summary>
        /// <param name="control"></param>
        public void Add(Control control)
        {
            if (RequiredControls != null)
            {
                Predicate<RequiredInfo> FindValues = delegate(RequiredInfo value)
                {
                    return value.Name == control.Name ? true : false;
                };
                if (!RequiredControls.Exists(FindValues))
                    RequiredControls.Add(new RequiredInfo(control));
            }
        }
        /// <summary>
        /// 移除必填控件
        /// </summary>
        /// <param name="control"></param>
        public void Remove(Control control)
        {
            if (RequiredControls != null && RequiredControls.Count > 0)
            {
                Predicate<RequiredInfo> FindValues = delegate(RequiredInfo value)
                {
                    return value.Name == control.Name ? true : false;
                };
                RequiredControls.RemoveAll(FindValues);
            }
        }

        private void SetBackColor(List<RequiredInfo> list)
        {
            foreach (RequiredInfo r in list)
            {
                r.ControlObject.BackColor = Common.RequiredBackColor;
            }
        }
    }
}
