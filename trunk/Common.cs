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
        #region ToolTip Function
        private static ToolTip toolTip = new ToolTip();
        public static void ShowToolTipInfo(ToolTip toolTip1, Control control, string text)
        {
            //toolTip.IsBalloon = true;
            toolTip.ToolTipIcon = ToolTipIcon.Info;
            toolTip.ToolTipTitle = Properties.Resources.Information;
            toolTip.Show(text, control, 0, control.Height, 5000);
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
    }
}
