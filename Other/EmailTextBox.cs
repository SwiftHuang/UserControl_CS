using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using hwj.UserControls.Interface;
using hwj.UserControls.CommonControls;

namespace hwj.UserControls.Other
{
    public class EmailTextBox : xTextBox
    {
        /// <summary>
        /// 
        /// </summary>
        public enum EmailspliterEnum : byte
        {
            /// <summary>
            ///真没有 
            /// </summary>
            None = 0,
            /// <summary>
            ///逗号 
            /// </summary>
            Comma = 0x2C,
            /// <summary>
            /// 分号
            /// </summary>
            Semicolon = 0x3B,
            /// <summary>
            /// 换行
            /// </summary>
            Enter = 0x0D,
            /// <summary>
            /// 自定义
            /// </summary>
            Custom,
        }
        private string emailSplitChar
        {
            get
            {
                if (EmailSpliter == EmailspliterEnum.None)
                    return string.Empty;
                else if (EmailSpliter == EmailspliterEnum.Enter)
                    return "\r\n";
                else if (EmailSpliter != EmailspliterEnum.Custom)
                    return Convert.ToChar(EmailSpliter).ToString();
                else
                    return CustomEmailSplitString;
            }
        }

        [DefaultValue(EmailspliterEnum.None), Description("设置输入多个Email地址时的分隔符,None为单个Email地址")]
        public EmailspliterEnum EmailSpliter { get; set; }

        public string CustomEmailSplitString { get; set; }
        [Browsable(false)]
        public ContentType ContentType { get; set; }

        public EmailTextBox()
        {
            Properties.Resources.Culture = Thread.CurrentThread.CurrentUICulture;
            ShowContentError = true;

            TextIsChanged = false;
            OldBackColor = this.BackColor;
            IsRequired = false;
            EnterEqualTab = true;
            SetValueToControl = null;
            ValueChangedEnabled = true;
            ContentType = ContentType.None;
        }

        #region Override Function
        protected override void OnValidating(CancelEventArgs e)
        {
            if (DesignMode)
                return;

            bool isInvaildText = false;
            List<string> errList = new List<string>();

            if (!string.IsNullOrEmpty(Text) && !CommonLibrary.Object.EmailHelper.isValidEmail(this.Text, emailSplitChar, out errList))
            {
                if (ShowContentError)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(string.Format(Properties.Resources.InvalidEmail, string.Empty));
                    foreach (string str in errList)
                    {
                        sb.AppendLine("-" + str);
                    }
                    Common.ShowToolTipInfo(this, sb.ToString());
                }
                //this.Text = string.Empty;
                isInvaildText = true;
            }

            if (isInvaildText && IsRequired)
            {
                e.Cancel = true;
                TextIsChanged = true;
            }
            base.SetRequiredStatus();
            if (SetValueToControl != null)
                SetValueToControl.Text = this.Text;
            base.OnValidating(e);
        }
        protected override void OnEnter(EventArgs e)
        {
            if (string.IsNullOrEmpty(Text) && EmailSpliter != EmailspliterEnum.None)
            {
                Common.ShowToolTipInfo(this, string.Format(Properties.Resources.EamilTips, emailSplitChar == "\r\n" ? "Enter" : emailSplitChar));
            }
            if (DesignMode)
                return;
            base.OnEnter(e);
            TextIsChanged = false;
        }
        #endregion

        #region Public Function
        public bool CheckData()
        {
            string err = string.Empty;
            return CheckData(out err);
        }
        public bool CheckData(out string errmsg)
        {
            errmsg = string.Empty;
            if (DesignMode)
                return true;
            bool isVaildText = true;
            List<string> errList = new List<string>();
            if (!string.IsNullOrEmpty(Text) && !CommonLibrary.Object.EmailHelper.isValidEmail(this.Text, Convert.ToChar(EmailSpliter).ToString(), out errList))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(string.Format(Properties.Resources.InvalidEmail, string.Empty));
                foreach (string str in errList)
                {
                    sb.AppendLine("-" + str);
                }
                errmsg = sb.ToString();
                isVaildText = false;
            }

            return isVaildText;

        }
        #endregion
    }
}
