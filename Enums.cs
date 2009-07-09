using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hwj.UserControls
{
    public class Enums
    {
        public enum DateFormat
        {
            None,
            Date,
            DateTime,
        }

        public enum CellContentType
        {
            None,
            Date,
            Numberic,
        }
    }
    public static class EnumsExtensions
    {
        public static string GetFormat(this Enums.DateFormat dateType)
        {
            switch (dateType)
            {
                case Enums.DateFormat.None:
                    return string.Empty;
                case Enums.DateFormat.Date:
                    return Common.Format_Date;
                case Enums.DateFormat.DateTime:
                    return Common.Format_DateTime;
                default:
                    break;
            }
            return string.Empty;
        }
    }
}
