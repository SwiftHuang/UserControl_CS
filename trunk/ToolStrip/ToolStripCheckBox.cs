using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms.Design;

namespace hwj.UserControls.ToolStrip
{
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.StatusStrip)]
    public class ToolStripCheckedBox : ToolStripControlHost
    {
        public event EventHandler CheckedChanged;
        public ToolStripCheckedBox()
            : base(new CheckBox())
        {
            this.CheckBox.CheckedChanged+=new EventHandler(CheckBox_CheckedChanged);
        }

        void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckedChanged != null)
                CheckedChanged(sender, e);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public CheckBox CheckBox
        {
            get { return (CheckBox)this.Control; }
        }

    }


}
