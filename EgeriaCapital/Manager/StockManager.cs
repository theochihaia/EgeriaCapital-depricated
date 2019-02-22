using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EgeriaCapital.Enums;
using EgeriaCapital.Models;
using YahooFinanceApi;

namespace EgeriaCapital.Manager
{
    public class StockManger
    {

        public async Task<IReadOnlyList<YahooFinanceApi.Candle>> GetStockDetails(string symbol, int period)
        {

            IReadOnlyList<YahooFinanceApi.Candle> candles = await YahooFinanceApi.Yahoo.GetHistoricalAsync(symbol, DateTime.UtcNow.Date.AddDays(-1.5 * period), DateTime.UtcNow);

            return candles;
        }

        public async Task<IReadOnlyList<YahooFinanceApi.Candle>> GetStockDetails(string symbol, DateTime latestDate)
        {

            IReadOnlyList<YahooFinanceApi.Candle> candles = await YahooFinanceApi.Yahoo.GetHistoricalAsync(symbol, latestDate, DateTime.UtcNow);

            return candles;
        }

        public async Task<IReadOnlyList<YahooFinanceApi.Candle>> GetStockDetailsAll(string symbol)
        {

            IReadOnlyList<YahooFinanceApi.Candle> candles = await YahooFinanceApi.Yahoo.GetHistoricalAsync(symbol, DateTime.UtcNow.Date.AddDays(-800), DateTime.UtcNow);

            return candles;
        }

    }
}
