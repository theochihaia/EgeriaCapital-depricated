using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EgeriaCapital.Common.Extensions;
using EgeriaCapital.Enums;
using YahooFinanceApi;

namespace EgeriaCapital.Models
{
    public class TradeRecommendation
    {
        public string Symbol { get; set; }

        [Display(Name = "Buy")]
        public decimal PurchaseRecommendation { get; set; }

        [Display(Name = "Sell")]
        public decimal SellRecommendation { get; set; }

        [Display(Name = "Action")]
        public TradeAction TradeAction
        {
            get
            {
                if (MostRecentTradingSession.Close <= (decimal) PurchaseRecommendation)
                    return Enums.TradeAction.Buy;
                else if (MostRecentTradingSession.Close >= (decimal) SellRecommendation)
                    return Enums.TradeAction.Sell;
                else
                    return Enums.TradeAction.Hold;
            }
        }

        [Display(Name = "Action")]
        public String TradeActionDescription
        {
            get
            {
                return this.TradeAction.GetDescription();
            }
        }

        [Display(Name = "Potential Gain")]
        public decimal PercentReturnPotential
        {
            get
            {
                decimal score = (this.SellRecommendation - this.PurchaseRecommendation) / this.PurchaseRecommendation;
                return score * 100;
            }
        }

        [Display(Name = "Sell Ratio")]
        public decimal SellRatio {
            get
            {
                decimal closePrice = this.MostRecentTradingSession.Close;
                decimal score = (this.MostRecentTradingSession.Close - this.SellRecommendation)/ this.MostRecentTradingSession.Close;
                return score;
            }
        }

        [Display(Name = "Buy Ratio")]
        public decimal BuyRatio
        {
            get
            {
                decimal closePrice = this.MostRecentTradingSession.Close;
                decimal score = (this.MostRecentTradingSession.Close - this.PurchaseRecommendation) / this.MostRecentTradingSession.Close;
                return score;
            }
        }


        public Candle MostRecentTradingSession { get; set; }


        // TODO: Use this for algorithm settings
        public class GetRequest{
            public Dictionary<TradeAlgorithm, TradeAlgorithmConfiguration> algorithms { get; set; }

        }


    }
}
