using Projekt.Classes;
using Projekt.Enums;
using System;
using System.Collections.Generic;

namespace Projekt
{
    /// <summary>
    /// Klasa która reprezentuje jadną sesję
    /// </summary>
    public class Session
    {
        #region Constructors
        public Session()
        {
        }
        #endregion

        #region Properties

        /// <summary>
        /// Lista ciągu żądań otrzymany do analizy, wraz z typem
        /// </summary>
        public List<Pair<string, ListOfRequestTypes>> Requests { get; set; } = new List<Pair<string, ListOfRequestTypes>>();

        /// <summary>
        /// Jaki jest to typ sesji | Robot albo Human
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Typ który ocenił algorytm uczący
        /// </summary>
        public string PredictedType { get; set; }

        /// <summary>
        /// Zawiera informację o prawdopodobieństwie rozpoczęcia zapytania danym typem żądania + nazwę żądania
        /// </summary>
        public List<Pair<string, float>> StartChances { get; set; }

        /// <summary>
        /// Tablica zawierająca w sobie prawdopodobieństwo wystąpienia jednego elementu po drugim
        /// </summary>
        public float[,] Probability { get; set; } = new float[10, 10];


        #endregion
    }
}

