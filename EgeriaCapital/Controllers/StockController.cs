using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EgeriaCapital.Manager;
using EgeriaCapital.Models;

namespace EgeriaCapital.Controllers
{
    [Route("api/stocks")]
    public class StockController : Controller
    {
        // GET api/values
        [HttpGet("details/{symbol}")]
        public Task<IReadOnlyList<YahooFinanceApi.Candle>> GetStockDetails(string symbol)
        {
            StockManger portfolioManager = new StockManger();
            var candleList = portfolioManager.GetStockDetails(symbol);

            return candleList;
        }



    }
}
