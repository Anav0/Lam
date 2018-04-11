using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt.Classes
{
    public class RealTime
    {
        /// <summary>
        /// Liczba zapytań jaka ma być przepracowana aby dokonać oceny
        /// czy sesja jest H czy R
        /// </summary>
        double k { get; set; } = 5000;

        /// <summary>
        /// Wartość k jako procentowa wartość zapytań do przepracowania
        /// </summary>
        double procentOfk { get; set; }

        /// <summary>
        /// Bazowe wymagane prawdopodobieństwo tzw. baseline
        /// </summary>
        double delta { get; set; }

        /// <summary>
        /// Ocenia czy sesja jest sesją robocika
        /// </summary>
        public string JudgeSession(int currReq, int totalReqNumber, double prawH, double prawR)
        {
            double d;

            if(currReq >= k)
            {
                d = Math.Log10(prawR / prawH);

                if(!(Math.Abs(d) < delta))
                {
                    return "";
                }
                else if(d >= 0)
                {
                    return "R";
                }
                else if(d < 0)
                {
                    return "H";
                }
            }
            else { return ""; }

            return "";
        }

        public List<Pair<double[], string>> OptimizeDTMC(List<Pair<double[], string>> List)
        {
            var newList = new List<Pair<double[], string>>();
            var index = 0;

            //Jesli lista ma dwie pozycje i są one różne to zwróć listę bez optymalizacji
            if (List.Count == 2 && List[0].item2 != List[1].item2)
            {
                return List;
            }
            //Jeśli jest więcej elementów niż 2 to posortuj je.
            if (List.Count > 1)
            {
                List.Sort((x, y) => x.item2.CompareTo(y.item2));
            }

            for (int i = 0; i < List.Count; i++)
            {
                for (int j = 0; j < List.Count; j++)
                {
                    //Jesli elementy są tego samego typu i są na różnych pozycjach to:
                    if (List[i].item2 == List[j].item2 && i != j)
                    {
                        index = 0;
                        //Dla każdego elementu prawdopodobieństwa
                        foreach (double value in List[i].item1)
                        {
                            //W skrócie z dwóch sesji R R lub H H zostawiamy tą w której wyszło wyższe prawdopodobieństwo
                            if (value > List[j].item1[index])
                            {
                                List[j].item1[index] = value;
                            }
                            index++;
                        }
                        List.RemoveAt(i);
                    }
                }
            }
            return List;
        }
    }
}
