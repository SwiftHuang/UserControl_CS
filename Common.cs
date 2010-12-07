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
        private static ToolTip toolTip = new ToolTip();

        public static void HideToolTopInfo(Control control)
        {
            if (toolTip.Active)
                toolTip.Hide(control);
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
                //toolTip.IsBalloon = true;
                if (isError)
                {
                    toolTip.ToolTipIcon = ToolTipIcon.Error;
                    toolTip.ToolTipTitle = Properties.Resources.ErrorInfo;
                }
                else
                {
                    toolTip.ToolTipIcon = ToolTipIcon.Info;
                    toolTip.ToolTipTitle = Properties.Resources.Information;
                }
                toolTip.Show(text, control, 0, control.Height, 1500);
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
