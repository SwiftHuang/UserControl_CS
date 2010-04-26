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
        private ToolStripDropDown tsDropDown = null;
        private ToolStripControlHost tsCtrlHost = null;
        private MonthCalendar monthCalendar = new MonthCalendar();

        #region Property
        public string Format { get; set; }
        private bool _showCheckBox = false;
        public bool ShowCheckBox
        {
            get { return _showCheckBox; }
            set
            {
                _showCheckBox = value;
                if (value)
                {
                    chkBox.Visible = true;
                    Point p = new Point(20, 3);
                    mTxtValue.Location = p;
                }
                else
                {
                    chkBox.Visible = false;
                    Point p = new Point(3, 3);
                    mTxtValue.Location = p;
                }
            }
        }
        private DateTime _value = DateTime.MinValue;
        public DateTime Value
        {
            get { return _value; }
            set
            {
                _value = value;
                string tmp = value.ToString(Format);
                if (tmp.Length > mTxtValue.Text.Length)
                    mTxtValue.Mask = Regex.Replace(Format, @"[a-zA-Z0-9]", "0");
                mTxtValue.Text = value.ToString(Format);
            }
        }
        #endregion

        public MaskedDateTimePicker()
        {
            InitializeComponent();
            ShowCheckBox = false;
            if (!DesignMode)
            {
                monthCalendar.DateSelected += new DateRangeEventHandler(monthCalendar_DateSelected);

                mTxtValue.ValidatingType = typeof(System.DateTime);
                mTxtValue.TypeValidationCompleted += new TypeValidationEventHandler(mTxtValue_TypeValidationCompleted);

                tsCtrlHost = new ToolStripControlHost(monthCalendar);
                tsCtrlHost.Padding = new Padding(0);
                tsCtrlHost.Margin = new Padding(0);
                tsCtrlHost.Dock = DockStyle.Fill;

                tsDropDown = new ToolStripDropDown();
                tsDropDown.Padding = new Padding(0);
                tsDropDown.DropShadowEnabled = true;
                tsDropDown.Items.Add(tsCtrlHost);
                tsDropDown.AutoClose = true;
            }
        }

        void monthCalendar_DateSelected(object sender, DateRangeEventArgs e)
        {
            Value = monthCalendar.SelectionStart;
            CloseList();
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
            if (string.IsNullOrEmpty(Format))
                Format = mTxtValue.Culture.DateTimeFormat.ShortDatePattern;

            mTxtValue.Mask = Regex.Replace(Format, @"[a-zA-Z0-9]", "0");
            Value = DateTime.Now;
            base.OnCreateControl();
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            ShowList(sender, e);
        }

        void ParentForm_Move(object sender, EventArgs e)
        {
            CloseList();
        }

        private void ShowList(object sender, EventArgs e)
        {

            if (tsDropDown != null)
            {
                if (this.ParentForm != null)
                    this.ParentForm.Move += new EventHandler(ParentForm_Move);
                tsDropDown.Show(this, -2, this.Height - 2);
                Focus();
            }

        }
        private void CloseList()
        {
            if (tsDropDown == null)
                return;

            tsDropDown.Close();
        }
    }
}
