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

namespace EgeriaCapital.Manager
{
    public class TradeRecommendationManager
    {

        public TradeRecommendationManager() {}

        public TradeRecommendation GetTradeRecommendation(String sym, IReadOnlyList<YahooFinanceApi.Candle> candles, BollingerBandSetting setting)
        {
            var outputBollinger = BollingerBand.GetTradeRecommendation(sym, setting, candles);

/*
            var linearRegressionOutput = LinearRegression.GetTradeRecommendation(candles, setting.Period);

            TradeRecommendation recommendation = new TradeRecommendation()
            {
                Symbol = sym,
                MostRecentTradingSession = outputBollinger.MostRecentTradingSession,
                PurchaseRecommendation = .2m * linearRegressionOutput.PurchaseRecommendation + .8m * outputBollinger.PurchaseRecommendation,
                SellRecommendation = .2m * linearRegressionOutput.SellRecommendation +  .8m * outputBollinger.SellRecommendation
            }; */

            return outputBollinger;
        }


    }
}
