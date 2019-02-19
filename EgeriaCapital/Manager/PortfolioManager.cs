using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EgeriaCapital.Database.DataAccess;
using EgeriaCapital.Enums;
using EgeriaCapital.Models;
using YahooFinanceApi;

namespace EgeriaCapital.Manager
{
    public class PortfolioManager
    {
        PortfolioDAO _portfolioDao;
        StockManger _stockManager = new StockManger();
        TradeRecommendationManager _tradeRecommendationManager = new TradeRecommendationManager();


        // TODO: Add Config
        String[] symbols = new String[]{
                "vod","fslr","snap","ge","cqqq","bpy","phk","pgp","gld","hyem","oigyx","voo","tur","iipr"
            };

        BollingerBandSetting bollingerSetting = new BollingerBandSetting()
        {
            Period = 40,
            UpperStdDevLimit = 1.5m,
            LowerStdDevLimit = 1.5m
        };

        public PortfolioManager()
        {
            _portfolioDao = new PortfolioDAO();

            /*         
            TextReader tr = new StreamReader(@"data/StockSymbols.txt");
            string stockSymbolsToParse = tr.ReadLine();
            symbols = stockSymbolsToParse.Split(',');
            */

        }

        public List<Portfolio> GetPortfolios(Portfolio.GetRequest request)
        {
            List<Portfolio> portfolios = _portfolioDao.GetPortfolio(request.PortfolioGuids);
            return portfolios;
        }

        public Guid CreatePortfolio(Portfolio.CreateRequest request)
        {
            Guid portfolioGuid = _portfolioDao.CreatePortfolio(request);

            return Guid.Empty;
        }


        public void UpdatePortfolio(Guid portfolioGuid, Portfolio.UpdateRequest request)
        {
            _portfolioDao.UpdatePortfolio(portfolioGuid, request);
        }

        public List<TradeRecommendation> GetTradeRecommendation(Guid portfolioGuid, TradeRecommendation.GetRequest request)
        {
            // should call trade recommendation manager
            throw new NotImplementedException();
        }

        public async Task<TradeRecommendationView> GetDefaultDashboard()
        {
            List<Task<TradeRecommendation>> tasks = new List<Task<TradeRecommendation>>();

            foreach (String sym in symbols)
            {
                // Get stock data
                tasks.Add(ProcessTradeRecommendation(sym, bollingerSetting));
            }

            var results = await Task.WhenAll(tasks);

            // TODO: Handle null Trade Recommendations

            var tradeRecommendation =  new TradeRecommendationView()
            {
                Recommendations = results.ToList(),
                BollingerBandSetting = bollingerSetting
            };

            tradeRecommendation.Recommendations = tradeRecommendation.Recommendations.OrderByDescending(o => o?.SellRating).ToList();

            return tradeRecommendation;
        }

        private Task<TradeRecommendation> ProcessTradeRecommendation(String sym, BollingerBandSetting setting)
        {
            try
            {
                return Task.Run(() =>
                {
                    var candles = _stockManager.GetStockDetails(sym);

                    return _tradeRecommendationManager.GetTradeRecommendation(sym, candles.Result, setting);
                });

            }
            catch(Exception e)
            {
                // TODO: Implement Logger
                Console.WriteLine($@"Exception Getting trade recommendation for: {sym}
                    Exception Message: {e.Message}");
            }

            return null;
        }
    }
}
