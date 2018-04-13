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
        ///     Rzeczywisty typ sesji R lub H
        /// </summary>
        public string RealType { get; set; }

        /// <summary>
        ///     Przewidziany typ sesji R lub H
        /// </summary>
        public string PredictedType { get; set; }



        #endregion



        #region Public methods

        public void PerformOfflineDetection(SessionsGroup Dtmc)
        {

            double result = 0;
            try
            {
                Request firstElement = Dtmc.GroupUniqueRequest.Find(x => x.NameType == Requests[0]);

                result = firstElement == null ? 0.00001 : Math.Log10(firstElement.Quantity);
            }
            catch (ArgumentNullException ex)
            {
                result = 0;
            }

            //Dla każdego obecnego żądania
            foreach (var req in Requests)
            {
                var firstIndex = Dtmc.GroupUniqueRequest.IndexOf(Dtmc.GroupUniqueRequest.Find(x => x.NameType == req));
                var nextIndex = Requests.IndexOf(req);

                if (!(nextIndex >= Requests.Count - 1))
                {

                    var nextElement = new Request();
                    nextElement.NameType = Requests[nextIndex + 1];
                    var secondIndeks = Dtmc.GroupUniqueRequest.IndexOf(Dtmc.GroupUniqueRequest.Find(x => x.NameType == nextElement.NameType));

                    double subResult = 0;
                    if (secondIndeks == -1 || firstIndex == -1)
                        subResult = 0;
                    else
                        subResult = Dtmc.Probability[firstIndex, secondIndeks];
                    if (subResult == 0 || double.IsNaN(subResult)) subResult = 0.00001;
                    result += Math.Log10(subResult);
                }
            }
        }
        #endregion
    }
}