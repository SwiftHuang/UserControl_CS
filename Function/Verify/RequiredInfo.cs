using System.Windows.Forms;

namespace hwj.UserControls.Function.Verify
{
    internal class RequiredInfo
    {
        public string Name { get; set; }
        public string TypeName { get; set; }
        public Control ControlObject { get; set; }
        public RequiredInfo(Control control)
        {
            Name = control.Name;
            TypeName = control.GetType().ToString();
            ControlObject = control;
        }
    }
}
