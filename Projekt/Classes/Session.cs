using System;
using System.Collections.Generic;
using Projekt.Classes;

namespace Projekt
{
    /// <summary>
    ///     Klasa która reprezentuje jadną sesję
    /// </summary>
    public class Session
    {

        public Session()
        {
            Requests = new List<string>();
            RealType = "NN";
            PredictedType = "NN";

        }

        #region Properties

        /// <summary>
        ///     Lista żądań 
        /// </summary>
        public List<string> Requests { get; set; }

        /// <summary>
        /// Ile żądań ma sesja
        /// </summary>
        public int NumberOfRequests { get; set; }

        /// <summary>
        ///     Rzeczywisty typ sesji R lub H
        /// </summary>
        public string RealType { get; set; }

        /// <summary>
        ///     Przewidziany typ sesji R lub H
        /// </summary>
        public string PredictedType { get; set; }

        /// <summary>
        /// Procent żądań jakie mają zostać przepracowane aby dokonać oceny
        /// </summary>
        public double Kpercent { get; set; }

        /// <summary>
        /// Liczba żądań jakie mają zostaćprzepracowane aby dokonać oceny
        /// </summary>
        public int K { get; set; }

        /// <summary>
        /// Różnica między LogR i LogH wartości powinny być z przedziału 0,5 a 2
        /// </summary>
        public static double Delta { get; set; } = 0.5;

        private double Hresult { get; set; }

        private double Rresult { get; set; }

        public bool wasClassified { get; set; } = false;

        #endregion

        #region Public methods

      

        public void PerformOnlineDetection(List<SessionsGroup> ListOfDtmc)
        {
            if (Requests.Count <= 0) return;

            double result = 0;

            foreach (var Dtmc in ListOfDtmc)
            {
                result += GetLogPr(Dtmc, Requests[Requests.Count-1]);

                if (Dtmc.SessionsList[0].RealType == "R")
                {
                    if (result > Rresult || Rresult == 0)
                    {
                        Rresult = result;
                    }
                }
                else
                {
                    if (result > Hresult || Hresult == 0)
                    {
                        Hresult = result;
                    }
                }
            }

            // Przepracowano k-sesji
            if (Requests.Count >= K)
            {
                var d = Rresult / Hresult;

                if (Math.Abs(d) < Delta)
                {

                }
                else if (d >= 0)
                {
                    PredictedType = "R";
                }
                else if (d < 0)
                {
                    PredictedType = "H";

                }
            }

            if (Requests.Count == NumberOfRequests && !wasClassified)
            {
                PerformOfflineDetection(ListOfDtmc, 0);
            }

            if (PredictedType == "H" || PredictedType == "R")
            {
                wasClassified = true;
            }

        }

        #endregion

        #region Private Methods

        public void PerformOfflineDetection(List<SessionsGroup> ListOfDtmc, int startIndex)
        {
            foreach (var Dtmc in ListOfDtmc)
            {
                double result = 0;
                try
                {
                    Request firstElement = Dtmc.GroupUniqueRequest.Find(x => x.NameType == Requests[0]);

                    if (firstElement == null)
                    {
                        result = 0.00001;
                    }
                    else
                    {
                        result = firstElement.StarChances == 0 ? 0.00001 : Math.Log10(firstElement.StarChances);
                    }
                }
                catch (ArgumentNullException ex)
                {
                    result = 0;
                }

                //Dla każdego obecnego żądania
                for (int i = startIndex; i < Requests.Count; i++)
                {
                    result += GetLogPr(Dtmc, Requests[i]);

                    if (Dtmc.SessionsList[0].RealType == "R")
                    {
                        if (result > Rresult || Rresult == 0)
                        {
                            Rresult = result;
                        }
                    }
                    else
                    {
                        if (result > Hresult || Hresult == 0)
                        {
                            Hresult = result;
                        }
                    }
                }
            }

            PredictedType = Rresult < Hresult ? "H" : "R";
        }

        private double GetLogPr(SessionsGroup Dtmc, string req)
        {
            double result = 0;

            var firstIndex = Dtmc.GroupUniqueRequest.IndexOf(Dtmc.GroupUniqueRequest.Find(x => x.NameType == req));
            var nextIndex = Requests.IndexOf(req);

            if (!(nextIndex >= Requests.Count - 1))
            {
                var nextElement = new Request();
                nextElement.NameType = Requests[nextIndex + 1];
                var secondIndeks =
                    Dtmc.GroupUniqueRequest.IndexOf(
                        Dtmc.GroupUniqueRequest.Find(x => x.NameType == nextElement.NameType));

                double subResult = 0;
                if (secondIndeks == -1 || firstIndex == -1)
                    subResult = 0;
                else
                    subResult = Dtmc.Probability[firstIndex, secondIndeks];
                if (subResult == 0 || double.IsNaN(subResult)) subResult = 0.00001;
                result += Math.Log10(subResult);
            }

            return result;
        }

        #endregion
    }
}