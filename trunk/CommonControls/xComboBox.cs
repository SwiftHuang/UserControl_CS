using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using hwj.UserControls.Interface;

namespace hwj.UserControls.CommonControls
{
    public class xComboBox : ComboBox, IEnterEqualTab, IValueChanged
    {
        #region Property
        [DefaultValue(true)]
        public bool EnterEqualTab { get; set; }
        /// <summary>
        /// 设置引发hwj.UserControls.ValueChanged事件的对象
        /// </summary>
        [DefaultValue(null), Description("设置引发hwj.UserControls.ValueChanged事件的对象"), Browsable(false)]
        protected Function.Verify.ValueChangedHandle ValueChangedHandle { get; set; }
        protected Function.Verify.RequiredHandle RequiredHandle { get; set; }

        private bool _IsRequired = false;
        /// <summary>
        /// 获取或设置为必填控件
        /// </summary>
        [DefaultValue(false), Description("获取或设置为必填控件")]
        public bool IsRequired
        {
            get { return _IsRequired; }
            set
            {
                _IsRequired = value;
                SetRequiredStatus();
            }
        }

        [Browsable(false)]
        public System.Drawing.Color OldBackColor { get; set; }
        #endregion

        public xComboBox()
        {
            EnterEqualTab = true;
            ValueChangedHandle = Common.ValueChanged;
            OldBackColor = this.BackColor;
            IsRequired = false;
            OldBackColor = this.BackColor;
            ValueChangedEnabled = true;
        }

        #region Override Function
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            RequiredHandle = Common.Required;
            SetRequiredStatus();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (EnterEqualTab && e.KeyCode == Keys.Enter)
                SendKeys.Send("{Tab}");
            base.OnKeyDown(e);
        }
        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            if (ValueChangedEnabled && this.Focused && ValueChangedHandle != null)
                ValueChangedHandle.IsChanged = true;
            base.OnSelectedIndexChanged(e);
        }
        protected override void OnTextChanged(EventArgs e)
        {
            if (ValueChangedEnabled && this.Focused && ValueChangedHandle != null)
                ValueChangedHandle.IsChanged = true;
            base.OnTextChanged(e);
            SetRequiredStatus();
        }
        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            SetRequiredStatus();
        }
        #endregion

        public void SetRequiredStatus()
        {
            if (DesignMode) return;

            if (IsRequired && string.IsNullOrEmpty(this.Text) && Enabled)
            {
                if (RequiredHandle != null)
                {
                    RequiredHandle.Add(this);
                }
                this.BackColor = Common.RequiredBackColor;
            }
            else
            {
                if (RequiredHandle != null)
                {
                    RequiredHandle.Remove(this);
                }
                this.BackColor = this.OldBackColor;
            }
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
