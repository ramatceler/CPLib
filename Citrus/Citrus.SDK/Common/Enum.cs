using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citrus.SDK.Common
{
    public enum EnvironmentType
    {
        [Description("https://sandboxadmin.citruspay.com/")]
        Sandbox,
        [Description("https://admin.citruspay.com/")] 
        Production,
        [Description("https://oops.citruspay.com/")] 
        Oops
    };
}
