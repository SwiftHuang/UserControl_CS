using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using hwj.UserControls.CommonControls;

namespace hwj.UserControls
{
    public class Common
    {
        public static string Format_Date = "yyyy-MM-dd";
        public static string Format_InputDate = "yyyy-MM-dd";
        public static string Format_DateTime = "yyyy-MM-dd hh:mm:ss";

        public static string Format_Numberic = "N";

        protected internal static Function.Verify.ValueChangedHandle ValueChanged { get; set; }
        protected internal static Function.Verify.RequiredHandle Required { get; set; }

        #region ToolTip Function
        private static ToolTip _toolTip;
        private static ToolTip toolTip
        {
            get
            {
                if (_toolTip == null)
                    _toolTip = new ToolTip();
                return _toolTip;
            }
        }

        public static void HideToolTip()
        {
            if (_toolTip != null)
            {
                _toolTip.Dispose();
                _toolTip = null;
            }
        }
        public static void ShowToolTipInfo(Control control, string text)
        {
            ShowToolTip(toolTip, control, text, false);
        }
        public static void ShowToolTipError(Control control, string text)
        {
            ShowToolTip(toolTip, control, text, true);
        }
        public static void ShowToolTip(ToolTip toolTip1, Control control, string text, bool isError)
        {
            if (toolTip1 != null || control != null)
            {
                SetTitle(isError, ref toolTip1);
                toolTip1.Show(text, control, 0, control.Height, 3000);
            }
        }
        public static void SetToolTip(Control control, string text)
        {
            SetToolTip(toolTip, control, text, false);
        }
        public static void SetToolTip(Control control, string text, bool isError)
        {
            SetToolTip(toolTip, control, text, isError);
        }
        public static void SetToolTip(ToolTip toolTip1, Control control, string text, bool isError)
        {
            if (toolTip1 != null || control != null)
            {
                //toolTip.IsBalloon = true;
                toolTip1.AutoPopDelay = 10000;
                SetTitle(isError, ref toolTip1);
                toolTip1.SetToolTip(control, text);
            }
        }
        private static void SetTitle(bool isError, ref ToolTip toolTip1)
        {
            if (isError)
            {
                toolTip1.ToolTipIcon = ToolTipIcon.Error;
                toolTip1.ToolTipTitle = Properties.Resources.ErrorInfo;
            }
            else
            {
                toolTip1.ToolTipIcon = ToolTipIcon.Info;
                toolTip1.ToolTipTitle = Properties.Resources.Information;
            }
        }
        #endregion

        #region Verify Controls
        public static Color RequiredBackColor = Color.FromArgb(255, 223, 223);
        //public static bool InvalidVerify(Control control)
        //{
        //    return InvalidVerify(control, RequiredBackColor, Color.White);
        //}
        //public static bool InvalidVerify(Control control, Color requiredColor, Color normalColor)
        //{
        //    bool pass = true;
        //    foreach (Control c in control.Controls)
        //    {
        //        if (isRequiredControl(c, requiredColor, normalColor))
        //            pass = false;
        //    }
        //    return !pass;
        //}
        //private static bool isRequiredControl(Control control, Color requiredColor, Color normalColor)
        //{
        //    if (control.GetType().Name == typeof(xTextBox).Name)
        //    {
        //        xTextBox t = control as xTextBox;
        //        if (t != null && t.IsRequired && t.Text == string.Empty)
        //        {
        //            t.BackColor = requiredColor;
        //            return true;
        //        }
        //        else
        //            t.BackColor = normalColor;
        //    }
        //    else if (control.GetType().Name == typeof(Suggest.SuggestBox).Name)
        //    {
        //        Suggest.SuggestBox s = control as Suggest.SuggestBox;
        //        if (s != null && s.IsRequired && s.Text == string.Empty)
        //        {
        //            s.BackColor = requiredColor;
        //            return true;
        //        }
        //        else
        //            s.BackColor = normalColor;
        //    }
        //    return false;
        //}
        #endregion

        public static Color DataGridViewCellReadonlyBackColor = Color.FromArgb(245, 245, 245);

    }
}
