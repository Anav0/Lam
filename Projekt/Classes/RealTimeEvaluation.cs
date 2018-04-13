using System;
using System.Collections.Generic;

namespace Projekt.Classes
{
    public class RealTimeEvaluation
    {
        /// <summary>
        ///     Liczba zapytań jaka ma być przepracowana aby dokonać oceny
        ///     czy sesja jest H czy R
        /// </summary>
        private double k { get; } = 5000;

        /// <summary>
        ///     Wartość k jako procentowa wartość zapytań do przepracowania
        /// </summary>
        private double procentOfk { get; set; }

        /// <summary>
        ///     Bazowe wymagane prawdopodobieństwo tzw. baseline
        /// </summary>
        private double delta { get; set; }

        /// <summary>
        ///     Ocenia czy sesja jest sesją robocika
        /// </summary>
        public string JudgeSession(int currReq, int totalReqNumber, double prawH, double prawR)
        {
            double d;

            if (currReq >= k)
            {
                d = Math.Log10(prawR / prawH);

                if (!(Math.Abs(d) < delta))
                    return "";
                if (d >= 0)
                    return "R";
                if (d < 0) return "H";
            }
            else
            {
                return "";
            }

            return "";
        }

        
    }
}