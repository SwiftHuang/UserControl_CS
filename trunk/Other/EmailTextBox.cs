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
            Single = 0,
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

        #region Property
        /// <summary>
        /// 设置输入多个Email地址时的分隔符,None为单个Email地址
        /// </summary>
        [DefaultValue(EmailspliterEnum.Single), Description("设置输入多个Email地址时的分隔符,None为单个Email地址")]
        public EmailspliterEnum EmailSpliter { get; set; }
        /// <summary>
        /// 获取或设置自定义分隔字符
        /// </summary>
        [Description("获取或设置自定义分隔字符")]
        public string CustomEmailSplitString { get; set; }

        [Browsable(false)]
        public ContentType ContentType { get; set; }
        [Browsable(false)]
        public string Format { get; set; }
        #endregion

        public EmailTextBox()
            : base()
        {
            ContentType = ContentType.None;
        }

        #region Override Function
        protected override void OnValidating(CancelEventArgs e)
        {
            if (DesignMode)
                return;

            List<string> errList = new List<string>();

            if (ShowContentError && HasInvalidData(out errList))
            {
                if (EmailSpliter == EmailspliterEnum.Single)
                {
                    Common.ShowToolTipInfo(this, Properties.Resources.InvalidEmail);
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string str in errList)
                    {
                        sb.AppendLine(" - " + str);
                    }
                    Common.ShowToolTipInfo(this, string.Format(Properties.Resources.InvalidEmails, sb.ToString()));
                }
            }

            base.OnValidating(e);
        }
        protected override void OnEnter(EventArgs e)
        {
            if (DesignMode)
                return;

            if (string.IsNullOrEmpty(Text) && EmailSpliter != EmailspliterEnum.Single)
            {
                Common.ShowToolTipInfo(this, string.Format(Properties.Resources.EamilTips, EmailSpliter == EmailspliterEnum.Enter ? Properties.Resources.Enter : GetEmailSplitString()));
            }

            base.OnEnter(e);
        }
        #endregion

        #region Public Function
        public bool HasInvalidData()
        {
            List<string> invalidList = new List<string>();
            return HasInvalidData(out invalidList);
        }
        public bool HasInvalidData(out List<string> invalidList)
        {
            invalidList = new List<string>();

            return !hwj.CommonLibrary.Object.EmailHelper.isValidEmail(this.Text, GetEmailSplitString(), out invalidList);
        }
        #endregion

        #region Private Function
        private string GetEmailSplitString()
        {
            if (EmailSpliter == EmailspliterEnum.Single)
                return string.Empty;
            else if (EmailSpliter == EmailspliterEnum.Enter)
                return "\r\n";
            else if (EmailSpliter != EmailspliterEnum.Custom)
                return Convert.ToChar(EmailSpliter).ToString();
            else
                return CustomEmailSplitString;
        }
        #endregion
    }
}
