using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace EgeriaCapital.Enums
{
    public enum TradeAlgorithm
    {
        [Description("Bollinger Band")]
        BollingerBand,

        [Description("Linear Regression")]
        LinearRegression,

        [Description("Williams % R")]
        Williams_R,

        [Description("Stocastic RSI")]
        Stocastic_RSI
    }
}
