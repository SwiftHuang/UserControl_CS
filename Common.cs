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
        public static string Format_DateTime = "yyyy-MM-dd hh:mm:ss";
        public static string Format_Numberic = "###,##0.00";

        #region ToolTip Function
        private static ToolTip toolTip = new ToolTip();
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
        #endregion

        #region Verify Controls
        public static Color RequiredBackColor = Color.FromArgb(255, 223, 223);
        public static bool InvalidVerify(Control control)
        {
            return InvalidVerify(control, RequiredBackColor, Color.White);
        }
        public static bool InvalidVerify(Control control, Color requiredColor, Color normalColor)
        {
            bool pass = true;
            foreach (Control c in control.Controls)
            {
                if (isRequiredControl(c, requiredColor, normalColor))
                    pass = false;
            }
            return !pass;
        }
        private static bool isRequiredControl(Control control, Color requiredColor, Color normalColor)
        {
            if (control.GetType().Name == typeof(xTextBox).Name)
            {
                xTextBox t = control as xTextBox;
                if (t != null && t.IsRequired && t.Text == string.Empty)
                {
                    t.BackColor = requiredColor;
                    return true;
                }
                else
                    t.BackColor = normalColor;
            }
            else if (control.GetType().Name == typeof(Suggest.SuggestBox).Name)
            {
                Suggest.SuggestBox s = control as Suggest.SuggestBox;
                if (s != null && s.IsRequired && s.Text == string.Empty)
                {
                    s.BackColor = requiredColor;
                    return true;
                }
                else
                    s.BackColor = normalColor;
            }
            return false;
        }

        #endregion

        //#region Value is changed
        //public static bool ControlsValueIsChanged(Control control)
        //{
        //    foreach (Control c in control.Controls)
        //    {
        //        if (IsControlValueChanged(c))
        //            return true;
        //    }
        //    return false;
        //}
        //private static bool IsControlValueChanged(Control control)
        //{
        //    if (control.GetType().Name == typeof(xTextBox).Name)
        //    {
        //        xTextBox t = control as xTextBox;
        //        if (t != null && t.ValueIsChanged)
        //            return true;
        //    }
        //    if (control.GetType().Name == typeof(xComboBox).Name)
        //    {
        //        xComboBox c = control as xComboBox;
        //        if (c != null && c.ValueIsChanged)
        //            return true;
        //    }
        //    if (control.GetType().Name == typeof(xDateTimePicker).Name)
        //    {
        //        xDateTimePicker d = control as xDateTimePicker;
        //        if (d != null && d.ValueIsChanged)
        //            return true;
        //    }
        //    if (control.GetType().Name == typeof(xDateTimePicker).Name)
        //    {
        //        xDateTimePicker dg = control as xDateTimePicker;
        //        if (dg != null && dg.ValueIsChanged)
        //            return true;
        //    }
        //    return false;
        //}
        //#endregion
    }
}
