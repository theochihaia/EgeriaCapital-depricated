﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EgeriaCapital.Enums;
using EgeriaCapital.Manager;
using EgeriaCapital.Models;

namespace EgeriaCapital.Controllers
{

    [Route("api/portfolio")]
    public class PortfolioController : Controller
    {
        // GET api/values
        [HttpGet("getportfolios")]
        public List<Portfolio> GetPortfolio([FromBody]Portfolio.GetRequest request)
        {
            PortfolioManager portfolioManager = new PortfolioManager();
            List<Portfolio> portfolios = portfolioManager.GetPortfolios(request);

            return portfolios;
        }

        [HttpGet("create")]
        public Guid CreatePortfolio([FromBody]Portfolio.CreateRequest request)
        {
            PortfolioManager portfolioManager = new PortfolioManager();
            Guid portfolioGuid = portfolioManager.CreatePortfolio(request);

            return portfolioGuid;
        }

        [HttpPut("{portfolioGuid}/update")]
        public void UpdatePortfolio(Guid portfolioGuid, [FromBody]Portfolio.UpdateRequest request){

            PortfolioManager portfolioManager = new PortfolioManager();
            portfolioManager.UpdatePortfolio(portfolioGuid, request);
        }

        [HttpGet("{portfolioGuid}/traderecommendations")]
        public List<TradeRecommendation> GetTradeRecommendation(Guid portfolioGuid, [FromBody]TradeRecommendation.GetRequest request)
        {
            PortfolioManager portfolioManager = new PortfolioManager();
            List<TradeRecommendation> recommendations = portfolioManager.GetTradeRecommendation(portfolioGuid, request);
            return recommendations;
        }

        [HttpGet("dashboard")]
        public TradeRecommendationView GetDefaultDashboard()
        {
            PortfolioManager portfolioManager = new PortfolioManager();
            TradeRecommendationView recommendations = portfolioManager.GetDefaultDashboard().Result;
            return recommendations;
        }

        // TODO: [DELETE PORTFOLIO]

    }
}
