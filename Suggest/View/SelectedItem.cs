using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace hwj.UserControls.Suggest.View
{
    [Serializable]
    public class SelectedItem
    {
        public string Key { get; set; }
        public string FirstColumnValue { get; set; }
        public string SecondColumnValue { get; set; }
        public DataRowView Item { get; set; }
        public int SelectedIndex { get; set; }

        public SelectedItem()
        {

        }
    }
}
