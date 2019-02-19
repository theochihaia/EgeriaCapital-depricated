using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EgeriaCapital.Models;
using System.ComponentModel;
using YahooFinanceApi;
using EgeriaCapital.Common;

namespace EgeriaCapital.Manager
{
    public class TradeRecommendationManager
    {

        public TradeRecommendationManager()
        {

        }

        public TradeRecommendation GetTradeRecommendation(String sym, IReadOnlyList<YahooFinanceApi.Candle> candles, BollingerBandSetting setting)
        {
            var output = CalculateBollingerBand(candles, setting);
            output.Symbol = sym;
            return output;
        }

        // TODO: Fix type inconsistencies with double
        // TODO: Create inherited class with GetBuyRecommendation, GetSellRecommendation, GetTradeRecommendation
        private TradeRecommendation CalculateBollingerBand(IReadOnlyList<YahooFinanceApi.Candle> candles, BollingerBandSetting setting)
        {
            TradeRecommendation recommendation = new TradeRecommendation();

            var list = candles.OrderByDescending(c => c.DateTime).Take(setting.Period);

            // determine bands
            decimal stdDevHigh = (decimal) CalculateStandardDeviation(list.Select(c => c.High));
            decimal avgHigh = CalculateAverage(list.Select(c => c.High));
            decimal stdDevLow = (decimal)CalculateStandardDeviation(list.Select(c => c.Low));
            decimal avgLow = CalculateAverage(list.Select(c => c.Low));

            // Populate Output
            recommendation.MostRecentTradingSession = list.First();
            recommendation.PurchaseRecommendation = (avgLow - stdDevLow * setting.LowerStdDevLimit);
            recommendation.SellRecommendation = (avgHigh + stdDevHigh * setting.UpperStdDevLimit);

            if (list.First().Close <= (decimal) recommendation.PurchaseRecommendation)
            {
                recommendation.TradeAction = Enums.TradeAction.Buy;
                recommendation.TradeActionDescription = recommendation.TradeAction.GetDescription();
            }
            else if(list.First().Close >= (decimal)recommendation.SellRecommendation)
            {
                recommendation.TradeAction = Enums.TradeAction.Sell;
                recommendation.TradeActionDescription = recommendation.TradeAction.GetDescription();
            }
            else
            {
                recommendation.TradeAction = Enums.TradeAction.Hold;
                recommendation.TradeActionDescription = recommendation.TradeAction.GetDescription();
            }

            recommendation.PercentReturnPotential = (recommendation.SellRecommendation - recommendation.PurchaseRecommendation)/recommendation.PurchaseRecommendation * 100;

            return recommendation;
        }



        #region Helper Methods
        private static decimal CalculateAverage(IEnumerable<decimal> numbers)
        {
            return numbers.Sum(c => c) / (decimal) numbers.Count();
        }

        private static double CalculateRootMeanSquared(IEnumerable<decimal> numbers)
        {
            var avg = CalculateAverage(numbers);

            var rootMeanSquared = numbers.Sum(n =>
                Math.Pow( (double) (n - avg), 2)
                / (double) numbers.Count()
             );

            return rootMeanSquared;
        }

        private static double CalculateStandardDeviation(IEnumerable<decimal> numbers)
        {
            double rootMeanSquared = CalculateRootMeanSquared(numbers);
            return Math.Sqrt(rootMeanSquared);
        }

        #endregion

    }
}
