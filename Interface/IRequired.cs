using System;
using System.Collections.Generic;
using System.Text;

namespace hwj.UserControls.Interface
{
    interface IRequired
    {
        bool IsRequired { get; set; }
        System.Drawing.Color OldBackColor { get; set; }
        Function.Verify.RequiredHandle RequiredHandle { get; }
    }
}
