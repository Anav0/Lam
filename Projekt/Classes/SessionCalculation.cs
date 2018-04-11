using Projekt.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Projekt.Classes
{
    /// <summary>
    /// Klasa do przeprowadzania wszystkich potrzebnych operacji
    /// </summary>
    public static class SessionCalculation
    {
        #region Fields
        public static double maxHuman = 0;
        public static double maxRobot = 0;
        public static double maxHumanResult = 0;
        public static double maxRobotResult = 0;
        public static int HighestNumberOfRequests = 0;

        public static double[] prevValues = new double[10];
        public static double[] currentValues = new double[10];

        #endregion

        #region Methods

        /// <summary>
        /// Sprawdza czy lista nie jest pusta
        /// </summary>
        /// <param name="List">Lista sesji</param>
        /// <returns></returns>
        public static bool IsListPopulated(this ObservableCollection<Session> List)
        {
            if (List.Count <= 0)
            {
                MessageBox.Show("Podana lista sesji jest pusta", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else return true;
        }

        /// <summary>
        /// Metoda licząca prawdopodobieństwo rozpoczęcia się zapytania od danego typu żądania txt, web itd.
        /// </summary>
        /// <param name="List">Lista żądań wraz z prawdopodobieństwem rozpoczęcia of niego</param>
        public static List<Pair<string, float>> CalculateProbability(this ObservableCollection<Session> List)
        {
            bool IsAlreadyAdded = false;
            var pair = new Pair<string, float>("coś", 0);
            var Array = new List<Pair<string, float>>();
            var currentELement = new Pair<string, float>("Coś", 0);

            foreach (var session in List)
            {
                foreach (var element in Array)
                {
                    currentELement = element;
                    if (session.Requests[0].item1 == element.item1)
                    {
                        Array.Find(x => x.item1 == element.item1).item2++;
                        IsAlreadyAdded = true;
                        break;
                    }
                }
                if (!IsAlreadyAdded)
                {
                    pair = new Pair<string, float>(session.Requests[0].item1, 1);
                    Array.Add(pair);
                }
                IsAlreadyAdded = false;

            }
            foreach (var element in Array)
            {
                element.item2 /= List.Count;
            }

            return Array;
        }

        /// <summary>
        /// Tworzy tablice wielowymiarowe zawierające ilość wystąpień poszczególnych elementów po sobie oraz prawdobodobieństwo takiego wystąpienia.
        /// </summary>
        /// <param name="List">Lista wszystkich sesji</param>
        /// <param name="ArrayOfSymbols">Lista wszystkich występujących typów żądań</param>
        public static float[,] CreateMatrix(this ObservableCollection<Session> List, List<Pair<string, float>> ArrayOfSymbols)
        {
            //Inicjalizacja
            var ArrayOfProbability = new float[ArrayOfSymbols.Count, ArrayOfSymbols.Count];
            var ArrayOfHits = new float[ArrayOfSymbols.Count, ArrayOfSymbols.Count];

            //Używane do stworzenia odpowiedniej ilości wierszy w tabeli
            int HighestNrOfReq = 0;

            foreach (Session session in List)
            {
                if (session.Requests.Count > HighestNrOfReq)
                {
                    HighestNrOfReq = session.Requests.Count;
                }

                for (int i = 0; i < session.Requests.Count; i++)
                {
                    if (i > 0)
                    {
                        var prev = session.Requests[i - 1];
                        //Sprawdza jakiemu żądaniu odpowiada poprzednia wartość
                        for (int k = 0; k < ArrayOfSymbols.Count; k++)
                        {
                            if (prev.item1.First().ToString().ToUpper() + prev.item1.Substring(1) == ArrayOfSymbols[k].item1)
                            {
                                //Sprawdza jakiemu żądaniu odpowiada obecna wartość
                                for (int o = 0; o < ArrayOfSymbols.Count; o++)
                                {
                                    if (session.Requests[i].item1.First().ToString().ToUpper() + session.Requests[i].item1.Substring(1) == ArrayOfSymbols[o].item1)
                                    {
                                        //Zaznacza wystąpienie w odpowiednim miejscu w tablicy
                                        ArrayOfHits[k, o] += 1;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < ArrayOfSymbols.Count; i++)
            {
                for (int j = 0; j < ArrayOfSymbols.Count; j++)
                {
                    ArrayOfProbability[i, j] = ArrayOfHits[i, j];
                    ArrayOfProbability[i, j] /= ArrayOfSymbols[i].item2;
                }
            }
            HighestNumberOfRequests = HighestNrOfReq;
            return ArrayOfProbability;
        }

        /// <summary>
        /// TODO: UZUPEŁNIĆ OPIS
        /// </summary>
        /// <param name="List">Lista wszystkich sesji</param>
        /// <param name="Chances">Lista szans na rozpoczęcie się zapytania danym żądaniem</param>
        /// <param name="Probability">Lista zawierająca prawd. wystapienia jednego żądania po drugim</param>
        /// <param name="RequestTypesList">Lista wszystkich występujących typów żądań</param>
        public static Pair<double[], string> Traning(this ObservableCollection<Session> List, List<Pair<string, float>> Chances, float[,] Probability, List<Pair<string, float>> RequestTypesList, string typeOfSession)
        {
            RealTime realTime = new RealTime();

            var NextElement = new Pair<string, ListOfRequestTypes>("coś", ListOfRequestTypes.Unknown);
            var FirstElement = new Pair<string, float>("coś", 0);
            var arrayOfResults = new double[List.Count];
            var FinalList = new List<Pair<double[], string>>();

            int firstIndex = 0, secondIndeks = 0, nextIndex = 0, index = 0;
            double result = 0;
            double subResult = 0;

            string sessionType = typeOfSession;

            foreach (Session session in List)
            {
                Session CurrentSession = session;

                try
                {
                    FirstElement = Chances.Find(x => x.item1 == session.Requests[0].item1);
                    if (FirstElement == null)
                    {
                        result = 0.00001;
                    }
                    else
                    {
                        result = Math.Log10(FirstElement.item2);
                    }
                }
                catch (ArgumentNullException ex)
                {
                    result = 0;
                }

                //Dla każdego obecnego żądania
                foreach (var req in CurrentSession.Requests)
                {
                    firstIndex = RequestTypesList.IndexOf(RequestTypesList.Find(x => x.item1 == req.item1));
                    nextIndex = CurrentSession.Requests.IndexOf(req);

                    if (!(nextIndex >= CurrentSession.Requests.Count - 1))
                    {
                        NextElement = CurrentSession.Requests[nextIndex + 1];
                        secondIndeks = RequestTypesList.IndexOf(RequestTypesList.Find(x => x.item1 == NextElement.item1));

                        if (secondIndeks == -1 || firstIndex == -1)
                        {
                            subResult = 0;
                        }
                        else
                        {
                            subResult = Probability[firstIndex, secondIndeks];
                        }
                        if (subResult == 0 || subResult == Double.NaN)
                        {
                            subResult = 0.00001;
                        }
                        result += Math.Log10(subResult);

                        //TODO: OBLICZ P(S|R) I P(S|H)

                        //TODO: TU MA BYĆ OBLICZANIE REALTIME
                        realTime.JudgeSession(nextIndex, CurrentSession.Requests.Count, result, result);

                    }
                }
                arrayOfResults[index] = result;
                index++;
            }
            return new Pair<double[], string>(arrayOfResults, sessionType);
        }

        #endregion
    }
}
