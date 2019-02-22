using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EgeriaCapital.Models;
using System.ComponentModel;
using YahooFinanceApi;
using EgeriaCapital.Common.Extensions;
using EgeriaCapital.Common.Utils;
using EgeriaCapital.Algorithms.Settings;
using EgeriaCapital.Algorithms;
using EgeriaCapital.Enums;

namespace EgeriaCapital.Manager
{
    public class TradeRecommendationManager
    {
        // WARNING: Make sure weights add up to 1
        private Dictionary<TradeAlgorithm, decimal> algorithmWeights = new Dictionary<TradeAlgorithm, decimal>()
        {
            { TradeAlgorithm.BollingerBand, .9m },
            { TradeAlgorithm.LinearRegression, .1m },
        };

        public TradeRecommendationManager() {}

        public TradeRecommendation GetTradeRecommendation(String sym, IReadOnlyList<YahooFinanceApi.Candle> candles, BollingerBandSetting setting)
        {
            List<TradeRecommendation> recommendations = new List<TradeRecommendation>();
            
            // Execute Algorithms
            foreach(TradeAlgorithm alg in algorithmWeights.Keys.ToList())
            {
                switch(alg)
                {
                    case TradeAlgorithm.BollingerBand:
                        recommendations.Add(BollingerBand.GetTradeRecommendation(sym, setting, candles));
                        break;
                    case TradeAlgorithm.LinearRegression:
                        recommendations.Add(LinearRegression.GetTradeRecommendation(candles, setting.Period));
                        break;
                }
            }

            // Calculate Purchase and Sell
            decimal purchasePrice= recommendations.Sum(r => r.PurchaseRecommendation * algorithmWeights.GetValueOrDefault(r.Algorithm));
            decimal sellPrice = recommendations.Sum(r => r.SellRecommendation * algorithmWeights.GetValueOrDefault(r.Algorithm));

            // TODO: Make new object ot replace candles from yahoo finance
                //require proper decimal formatting and sorting
            TradeRecommendation outputRecommendation = new TradeRecommendation()
            {
                Symbol = sym,
                MostRecentTradingSession = candles.OrderByDescending(c => c.DateTime).FirstOrDefault(),
                PurchaseRecommendation = purchasePrice,
                SellRecommendation = sellPrice
            };

            return outputRecommendation;
        }


    }
}
