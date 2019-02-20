using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EgeriaCapital.Enums;
using YahooFinanceApi;

namespace EgeriaCapital.Models
{
    public class TradeRecommendation
    {
        public string Symbol { get; set; }

        [Display(Name = "Action")]
        public TradeAction TradeAction { get; set; }

        [Display(Name = "Action")]
        public String TradeActionDescription { get; set; }

        [Display(Name = "Buy")]
        public decimal PurchaseRecommendation { get; set; }

        [Display(Name = "Sell")]
        public decimal SellRecommendation { get; set; }

        [Display(Name = "Potential Gain")]
        public decimal PercentReturnPotential { get; set; }

        [Display(Name = "Sell Ratio")]
        public decimal SellRating {
            get
            {
                decimal closePrice = this.MostRecentTradingSession.Close;
                decimal score = (this.MostRecentTradingSession.Close - this.SellRecommendation)/ this.MostRecentTradingSession.Close;


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
