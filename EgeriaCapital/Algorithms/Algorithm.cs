using EgeriaCapital.Algorithms.Settings;
using EgeriaCapital.Enums;
using EgeriaCapital.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EgeriaCapital.Algorithms
{
    abstract public class Algorithm
    {
        //abstract public TradeRecommendation GetTradeRecommendation(Setting setting, IReadOnlyList<YahooFinanceApi.Candle> candles);

        abstract public TradeAlgorithm GetTradeAlgorithm();

    }
}
