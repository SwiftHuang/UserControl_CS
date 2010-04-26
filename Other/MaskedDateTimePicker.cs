using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace hwj.UserControls.Other
{
    public partial class MaskedDateTimePicker : UserControl
    {
        #region Property
        public string Format { get; set; }
        private DateTime _value = DateTime.MinValue;
        public DateTime Value
        {
            get { return _value; }
            set
            {
                _value = value;
                mTxtValue.Text = value.ToString(Format);
            }
        }
        #endregion

        public MaskedDateTimePicker()
        {
            InitializeComponent();
            mTxtValue.ValidatingType = typeof(System.DateTime);
            mTxtValue.TypeValidationCompleted += new TypeValidationEventHandler(mTxtValue_TypeValidationCompleted);
        }

        void mTxtValue_TypeValidationCompleted(object sender, TypeValidationEventArgs e)
        {
            if (e.IsValidInput)
            {
                Value = (DateTime)e.ReturnValue;
            }
            else
                Value = DateTime.MinValue;
        }

        protected override void OnCreateControl()
        {
            mTxtValue.Mask =  Regex.Replace(Format, @"[a-zA-Z]", "0");
            Value = DateTime.Now;
            base.OnCreateControl();
        }
    }
}
