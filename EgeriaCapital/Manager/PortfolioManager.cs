using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EgeriaCapital.Algorithms.Settings;
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
                "vod","fslr","snap","ge","cqqq","bpy","phk","pty","pgp","gld","hyem","oigyx","voo","tur","iipr","avb","eqr","vti","ilf","wfc","aapl","fb","amzn","m","cmg","goog","tsla","tmus","dis","f","chk"
            };


        BollingerBandSetting bollingerSetting = new BollingerBandSetting()
        {
            Period = 20,
            UpperStdDevLimit = 2.0m,
            LowerStdDevLimit = 2.0m
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
            if (false)
            {
                symbols = new String[]{
                "snap","ge","bpy","phk","pty"
                };
            }


            List<Task<TradeRecommendation>> tasks = new List<Task<TradeRecommendation>>();

            foreach (String sym in symbols)
            {
                // Get stock data
                var output = ProcessTradeRecommendation(sym, bollingerSetting);
                tasks.Add(output);
            }

            var results = await Task.WhenAll(tasks);

            // TODO: Handle null Trade Recommendations

            var tradeRecommendation =  new TradeRecommendationView()
            {
                Recommendations = results.ToList(),
                BollingerBandSetting = bollingerSetting
            };

            tradeRecommendation.Recommendations = tradeRecommendation.Recommendations
                .Where(r => r != null)
                .OrderByDescending(r => r?.BuyRatio)
                .ToList();

            return tradeRecommendation;
        }

        private Task<TradeRecommendation> ProcessTradeRecommendation(String sym, BollingerBandSetting setting)
        {
            return Task.Run(() =>
            {
                var candles = _stockManager.GetStockDetails(sym);
                try
                {
                    return _tradeRecommendationManager.GetTradeRecommendation(sym, candles.Result, setting);
                }
                catch (Exception e)
                {
                    // TODO: Implement Logger
                    Console.WriteLine($@"Exception Getting trade recommendation for: {sym}
                    Exception Message: {e.Message}");
                    return null;
                }

            });
        }
    }
}
