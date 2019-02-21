using EgeriaCapital.Enums;
using EgeriaCapital.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EgeriaCapital.Algorithms
{
    public class LinearRegression : Algorithm
    {
        private static TradeAlgorithm algorithm = TradeAlgorithm.LinearRegression;

        public LinearRegression()
        {

        }

        // This should return the slope and then use it for bollinger band calculation
        public static TradeRecommendation GetTradeRecommendation(IReadOnlyList<YahooFinanceApi.Candle> candles, int period)
        {
            var xRange = toDoubleArray(Enumerable.Range(1, period).ToList());

            var list = candles.OrderByDescending(c => c.DateTime).Take(period);

            var yRangeHigh = toDoubleArray(list.Select(c => c.High).ToList());
            var yRangeLow = toDoubleArray(list.Select(c => c.Low).ToList());

            var outputHigh = MathNet.Numerics.LinearRegression.SimpleRegression.Fit(xRange, yRangeHigh);
            var outputLow = MathNet.Numerics.LinearRegression.SimpleRegression.Fit(xRange, yRangeLow);

            int preditictionPeriod = period + 5;
            TradeRecommendation recommendation = new TradeRecommendation()
            {
                Algorithm = algorithm,
                MostRecentTradingSession = candles.OrderBy(c => c.DateTime).First(),
                SellRecommendation = (decimal)(outputHigh.Item1 + (outputHigh.Item2 * preditictionPeriod)),
                PurchaseRecommendation = (decimal)(outputLow.Item1 + (outputLow.Item2 * preditictionPeriod)),
                
            };

            return recommendation;
        }


        public static Tuple<double,double> GetLinRegLow(IReadOnlyList<YahooFinanceApi.Candle> candles, int period)
        {

            var xRange = toDoubleArray(Enumerable.Range(1, period).ToList());

            var list = candles.OrderByDescending(c => c.DateTime).Take(period);

            var yRangeLow = toDoubleArray(list.Select(c => c.Low).ToList());

            return MathNet.Numerics.LinearRegression.SimpleRegression.Fit(xRange, yRangeLow);

        }

        private static double[] toDoubleArray(List<int> list)
        {
            return list.Select<int, double>(i => i).ToArray();
        }

        private static double[] toDoubleArray(List<decimal> list)
        {
            return list.Select(item => Convert.ToDouble(item)).ToArray();
        }

        public override TradeAlgorithm GetTradeAlgorithm()
        {
            return algorithm;
        }

    }
}
