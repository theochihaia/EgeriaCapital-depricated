using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace EgeriaCapital.Enums
{
    public enum TradeAction
    {
        [Description("Buy")]
        Buy = 0,

        [Description("Sell")]
        Sell = 1,

        [Description("Hold")]
        Hold = 2
    }
}
