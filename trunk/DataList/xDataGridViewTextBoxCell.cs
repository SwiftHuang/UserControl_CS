using System;
using System.ComponentModel;
using System.Windows.Forms;
using hwj.UserControls.CommonControls;

namespace hwj.UserControls.DataList
{
    public class xDataGridViewTextBoxCell : DataGridViewTextBoxCell
    {
        [DefaultValue(false)]
        public bool IsSuggestBoxCell { get; set; }

        public xDataGridViewTextBoxCell()
        {
            IsSuggestBoxCell = false;
        }
    }
}
