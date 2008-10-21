using System;
using System.Collections.Generic;
using System.Text;

namespace Vinson.UserControls
{
    [Serializable]
    public class AutoCompleteValue
    {
        private string _key;
        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }

        private string _priValue;
        public string PrimaryValue
        {
            get { return _priValue; }
            set { _priValue = value; }
        }

        private string _secValue;
        public string SecondValue
        {
            get { return _secValue; }
            set { _secValue = value; }
        }
    }

    public class AutoCompleteCommon
    {
        private static string _SearchValue = string.Empty;
        public static string SearchValue
        {
            get { return _SearchValue; }
            set { _SearchValue = value; }
        }
        public static bool SearchPrimaryValue(AutoCompleteValue s)
        {
            if (s.PrimaryValue.IndexOf(SearchValue) == -1)
                return false;
            else
                return true;
        }
    }
}
