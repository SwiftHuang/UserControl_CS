using System;
using System.Collections.Generic;
using System.Text;

namespace hwj.UserControls.Function.Verify
{
    public interface IRequiredHandle
    {
        bool HasRequired { get; }
        Function.Verify.RequiredHandle RequiredHandle { get; set; }
    }
}
