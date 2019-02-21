using EgeriaCapital.Common.Utils;
using EgeriaCapital.Common.Extensions;
using EgeriaCapital.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using EgeriaCapital.Algorithms.Settings;

namespace EgeriaCapital.Algorithms
{
    public class BollingerBand : Algorithm
    {
        public BollingerBand()
        {
        }

        // TODO: Fix type inconsistencies with double
        // TODO: Create inherited class with GetBuyRecommendation, GetSellRecommendation, GetTradeRecommendation
        public static TradeRecommendation GetTradeRecommendation(BollingerBandSetting setting, IReadOnlyList<YahooFinanceApi.Candle> candles)
        {
            TradeRecommendation recommendation = new TradeRecommendation();

            var list = candles.OrderByDescending(c => c.DateTime).Take(setting.Period);

            // determine bands
            decimal stdDevHigh = (decimal)MathUtil.CalculateStandardDeviation(list.Select(c => c.High));
            decimal avgHigh = MathUtil.CalculateAverage(list.Select(c => c.High));
            decimal stdDevLow = (decimal)MathUtil.CalculateStandardDeviation(list.Select(c => c.Low));
            decimal avgLow = MathUtil.CalculateAverage(list.Select(c => c.Low));

            // Populate Output
            recommendation.MostRecentTradingSession = list.First();
            recommendation.PurchaseRecommendation = (avgLow - stdDevLow * setting.LowerStdDevLimit);
            recommendation.SellRecommendation = (avgHigh + stdDevHigh * setting.UpperStdDevLimit);
            

            return recommendation;
        }
    }
}
