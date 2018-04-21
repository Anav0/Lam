using System.Collections.Generic;
using Projekt.Classes;

namespace Projekt
{
    /// <summary>
    ///     Reprezentuję zbiór wczytanych sesji
    /// </summary>
    public class SessionsGroup
    {
        public SessionsGroup()
        {
            SessionsList = new List<Session>();
            GroupUniqueRequest = new List<Request>();
        }

        #region Public properties
        /// <summary>
        ///     Lista sesji znajdujących się we wczytanej grupie
        /// </summary>
        public List<Session> SessionsList { get; set; }

        /// <summary>
        ///     Występujące typy żądań w danej grupie wraz z ich ilością
        /// </summary>
        public List<Request> GroupUniqueRequest { get; set; }

        public int RobotCount { get; set; }

        public int CorrectlyAsRobot { get; set; }

        public int CorrectlyAsHuman { get; set; }

        public int WronglyAsHuman { get; set; }

        public int WronglyAsRobot { get; set; }

        public int HumanCount { get; set; }

        public int SumOfflineDetections { get; set; }

        public int SumOnlineDetections { get; set; }


        /// <summary>
        ///     Tablica zawierająca w sobie prawdopodobieństwo wystąpienia jednego żądania po drugim
        /// </summary>
        public float[,] Probability { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Dodaje do <see cref="SessionsList"/> takie <see cref="Request"/> które nie wystąpiło wcześniej
        /// </summary>
        /// <param name="currentRequest">Sprawdzane żądanie</param>
        public void AddUniqueRequest(Request currentRequest)
        {
            // Jeśli żądanie nie wystąpiło wcześniej to:
            if (GroupUniqueRequest.FindAll(x => x.NameType == currentRequest.NameType)
                    .Count <= 0)
            {
                currentRequest.Quantity = 1;
                GroupUniqueRequest.Add(currentRequest);
            }
            // Jeśli wystąpiło
            else
            {
                GroupUniqueRequest
                    .Find(x => x.NameType == currentRequest.NameType).Quantity++;
            }
        }

        /// <summary>
        /// Notuję klasyfikację
        /// </summary>
        /// <param name="session">Sprawdzana sesja</param>
        public void CalculateQuantities(Session session)
        {
            if (session.RealType == "H" && session.PredictedType == session.RealType)
            {
                CorrectlyAsHuman++;
            }
            else if (session.RealType == "R" && session.PredictedType == session.RealType)
            {
                CorrectlyAsRobot++;
            }
            else if (session.RealType == "H" &&
                     session.PredictedType != session.RealType)
            {
                WronglyAsRobot++;
            }
            else if (session.RealType == "R" &&
                     session.PredictedType != session.RealType)
            {
                WronglyAsHuman++;

            }

            if (session.DetectionMethodused == DetectionType.Offline)
            {
                SumOfflineDetections++;
            }
            else if (session.DetectionMethodused == DetectionType.Online)
            {
                SumOnlineDetections++;
            }
        }

        public void CalculateDTMC()
        {
            CalculateProbability();
            CreateMatrix();
        }

        #endregion

        #region Private Methods

        /// <summary>
        ///     Metoda licząca prawdopodobieństwo rozpoczęcia się grupy od danego typu żądania txt, web itd.
        /// </summary>
        private void CalculateProbability()
        {
            foreach (var request in GroupUniqueRequest)
            {
                // znajdue sesję które zaczynają się przez request
                request.SessionStarts = SessionsList.FindAll(x => x.Requests[0] == request.NameType).Count;

                request.StarChances = (double)request.SessionStarts / SessionsList.Count;
            }
        }

        /// <summary>
        ///     Tworzy tablice wielowymiarową zawierającą ilość wystąpień poszczególnych żądań po sobie oraz prawdobodobieństwo
        ///     takiego wystąpienia.
        /// </summary>
        private void CreateMatrix()
        {
            Probability = new float[GroupUniqueRequest.Count, GroupUniqueRequest.Count];
            Probability.Initialize();

            foreach (var session in SessionsList)
                for (var i = 0; i < session.Requests.Count - 1; i++)
                {
                    var firstReq = session.Requests[i];
                    var nextReq = session.Requests[i + 1];

                    var posI = GroupUniqueRequest.FindIndex(x => x.NameType == firstReq);
                    var posJ = GroupUniqueRequest.FindIndex(x => x.NameType == nextReq);

                    Probability[posI, posJ]++;
                }

            for (var i = 0; i < GroupUniqueRequest.Count; i++)
                for (var j = 0; j < GroupUniqueRequest.Count; j++)
                    Probability[i, j] /= GroupUniqueRequest[i].Quantity;
        }
    }
}

#endregion