using System;
using System.Collections.Generic;
using System.Text;

namespace hwj.UserControls.Suggest
{
    [Serializable]
    public class SuggestValue
    {
        public string Key { get; set; }
        public string PrimaryValue { get; set; }
        public string SecondValue { get; set; }
    }
    [Serializable]
    public class SuggestList : List<SuggestValue>
    { }
}
