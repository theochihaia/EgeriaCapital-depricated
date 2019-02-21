using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EgeriaCapital.Common.Utils
{
    public class MathUtil
    {
        public static decimal CalculateAverage(IEnumerable<decimal> numbers)
        {
            return numbers.Sum(c => c) / (decimal)numbers.Count();
        }

        public static double CalculateRootMeanSquared(IEnumerable<decimal> numbers)
        {
            var avg = CalculateAverage(numbers);

            var rootMeanSquared = numbers.Sum(n =>
                Math.Pow((double)(n - avg), 2)
                / (double)numbers.Count()
             );

            return rootMeanSquared;
        }

        public static double CalculateStandardDeviation(IEnumerable<decimal> numbers)
        {
            double rootMeanSquared = CalculateRootMeanSquared(numbers);
            return Math.Sqrt(rootMeanSquared);
        }

    }
}
