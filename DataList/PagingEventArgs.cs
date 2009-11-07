using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace hwj.UserControls.DataList
{
    public class PagingEventArgs
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string SortName { get; set; }
        public ListSortDirection Sort { get; set; }
    }
}
