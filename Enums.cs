using System;
using System.Collections.Generic;
using System.Text;

namespace hwj.UserControls
{
    public class Enums
    {
        public enum DateFormat
        {
            None,
            InputDate,
            Date,
            DateTime,
        }
        public static string GetFormat(Enums.DateFormat dateType)
        {
            switch (dateType)
            {
                case Enums.DateFormat.None:
                    return string.Empty;
                case Enums.DateFormat.Date:
                    return Common.Format_Date;
                case Enums.DateFormat.DateTime:
                    return Common.Format_DateTime;
                case DateFormat.InputDate:
                    return Common.Format_InputDate;
                default:
                    break;
            }
            return string.Empty;
        }
        public enum CellContentType
        {
            None,
            Date,
            Numberic,
        }
    }
    //public static class EnumsExtensions
    //{
    //    public static string GetFormat(this Enums.DateFormat dateType)
    //    {
    //        switch (dateType)
    //        {
    //            case Enums.DateFormat.None:
    //                return string.Empty;
    //            case Enums.DateFormat.Date:
    //                return Common.Format_Date;
    //            case Enums.DateFormat.DateTime:
    //                return Common.Format_DateTime;
    //            default:
    //                break;
    //        }
    //        return string.Empty;
    //    }
    //}
}
