

using System;
using System.Collections.Generic;
using Projekt.Classes;

namespace Projekt
{
    public class TestedSession : Session
    {
        public TestedSession()
        {
            PredictedType = SessionTypes.NotRecognized;
        }

        #region Public properties

        /// <summary>
        ///     Przewidziany typ sesji R lub H
        /// </summary>
        public SessionTypes PredictedType { get; set; }

        /// <summary>
        ///     Procent żądań jakie mają zostać przepracowane aby dokonać oceny
        /// </summary>
        public float Kpercent { get; set; }

        /// <summary>
        ///     Liczba żądań jakie mają zostać przepracowane aby dokonać oceny
        /// </summary>
        public int K { get; set; }

        public int NumberOfRequests { get; set; }

        private float Hresult { get; set; }

        private float Rresult { get; set; }

        private float Result { get; set; }

        /// <summary>
        ///     Mówi nam czy sesja została zaklasyfikowana
        /// </summary>
        public bool WasClassified { get; set; }

        /// <summary>
        /// Metoda użyta do klasyfikacji
        /// </summary>
        public DetectionType DetectionMethodUsed { get; set; }
        #endregion

        #region Public Methods

        /// <summary>
        ///     Przeprowadza ocenę sesji po każdym nowym żądaniu
        /// </summary>
        /// <param name="listOfDtmc">DTMC na podstawie których zostanie dokonana ocena</param>
        public void PerformOnlineDetection(List<DtmcGroup> listOfDtmc, TestedGroup myGroup)
        {
            if (Requests.Count <= 0) return;

            foreach (var dtmc in listOfDtmc)
            {
                Result = 0;
                Result += GetStartChanceValue(dtmc);

                Result += GetLogPr(dtmc, Requests.Count - 1);

                if (dtmc.Sessions[0].RealType == SessionTypes.Robot)
                {
                    if (Result > Rresult || Rresult == 0) Rresult = Result;
                }
                else
                {
                    if (Result > Hresult || Hresult == 0) Hresult = Result;
                }
            }
           


            // Przepracowano k-sesji
            if (Requests.Count >= K)
            {
                var d = Math.Log10(Rresult / Hresult);

                if (Math.Abs(d) < myGroup.Delta)
                {
                }
                else if (d >= 0)
                {
                    PredictedType = SessionTypes.Robot;
                    DetectionMethodUsed = DetectionType.Online;
                }
                else if (d < 0)
                {
                    PredictedType =SessionTypes.Human;
                    DetectionMethodUsed = DetectionType.Online;
                }
            }

            if (Requests.Count == NumberOfRequests && !WasClassified) PerformOfflineDetection(listOfDtmc);

            if (PredictedType == SessionTypes.Human || PredictedType == SessionTypes.Robot) WasClassified = true;


        }

        /// <summary>
        ///     Przeprowadza ocenę sesji na podstawie wszystkich jej żądań
        /// </summary>
        /// <param name="ListOfDtmc">DTMC na podstawie których zostanie dokonana ocena</param>
        public void PerformOfflineDetection(List<DtmcGroup> ListOfDtmc)
        {
            Rresult = 0;
            Hresult = 0;

            foreach (var Dtmc in ListOfDtmc)
            {
                Result = 0;
                Result += GetStartChanceValue(Dtmc);

                for (int i = 0; i < Requests.Count; i++)
                {
                    Result += GetLogPr(Dtmc, i);
                }

                if (Dtmc.Sessions[0].RealType == SessionTypes.Robot)
                {
                    if (Result > Rresult || Rresult == 0) Rresult = Result;
                }
                else
                {
                    if (Result > Hresult || Hresult == 0) Hresult = Result;
                }
            }

            PredictedType = Rresult < Hresult ? SessionTypes.Human : SessionTypes.Robot;
            WasClassified = true;
            DetectionMethodUsed = DetectionType.Offline;
        }

        #endregion

        #region Private Methods

        private float GetStartChanceValue(DtmcGroup dtmc)
        {
            float result = 0;
            try
            {
                var firstElement = dtmc.UniqueRequest.Find(x => x.NameType == Requests[0]);

                if (firstElement == null)
                    result = 0.00001f;
                else
                    result = firstElement.StarChances == 0 ? 0.00001f : (float)Math.Log10(firstElement.StarChances);
            }
            catch (ArgumentNullException ex)
            {
                result = 0;
            }

            return result;
        }

        private float GetLogPr(DtmcGroup Dtmc, int firstReqPos)
        {
            float result = 0;

            if (firstReqPos == 0 || firstReqPos == Requests.Count)
            {
                return 0f;
            }

            var firstIndex = Dtmc.UniqueRequest.IndexOf(Dtmc.UniqueRequest.Find(x => x.NameType == Requests[firstReqPos - 1]));
            var nextIndex = Dtmc.UniqueRequest.IndexOf(Dtmc.UniqueRequest.Find(x => x.NameType == Requests[firstReqPos]));

            if (nextIndex == -1 || firstIndex == -1)
            {
                result += 0.00001f;
            }
            else
            {
                result += Dtmc.Probability[firstIndex, nextIndex];
            }

            if (result == 0)
            {
                result += 0.00001f;
            }

            result += (float)Math.Log10(result);

            return result;
        }

        #endregion
    }
}
