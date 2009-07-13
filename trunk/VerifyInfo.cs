using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;

namespace hwj.UserControls
{
    public class VerifyInfo
    {
        public static List<string> RequiredControls = null;
        public static bool ValueIsChanged { get; set; }

        public static void AddRequiredControl(Control control)
        {
            if (RequiredControls != null)
                RequiredControls.Add(control.Name);
        }
        public static void RemoveRequiredControl(Control control)
        {
            if (RequiredControls != null)
                RequiredControls.Remove(control.Name);
        }
    }
}
