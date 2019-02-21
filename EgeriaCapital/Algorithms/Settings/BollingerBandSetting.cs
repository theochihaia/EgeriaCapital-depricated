using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EgeriaCapital.Algorithms.Settings
{
    public class BollingerBandSetting : Setting
    {
        public int Period { get; set; }
        public decimal UpperStdDevLimit { get; set; }
        public decimal LowerStdDevLimit { get; set; }
    }
}
