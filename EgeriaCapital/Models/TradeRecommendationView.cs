using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EgeriaCapital.Models
{
    public class TradeRecommendationView
    {
        public List<TradeRecommendation> Recommendations { get; set; }
        public BollingerBandSetting BollingerBandSetting { get; set; }
    }
}
