using System;
using System.Collections.Generic;
using System.Linq;

namespace Projekt.Classes
{
    /// <summary>
    ///     Klasa która reprezentuje jadną sesję
    /// </summary>
    public class Session
    {
        public Session()
        {
            Requests = new List<string>();
            RealType = SessionTypes.NotRecognized;
        }

        #region Public properties

        /// <summary>
        ///     Lista żądań
        /// </summary>
        public List<string> Requests { get; set; }

        /// <summary>
        ///     Rzeczywisty typ sesji
        /// </summary>
        public SessionTypes RealType { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        ///     Dodaje nowe żądanie do sesji
        /// </summary>
        /// <param name="request">Nazwa żądania</param>
        public void AddRequest(string request)
        {
            if (!string.IsNullOrEmpty(request))
            {
                request = request.First().ToString().ToUpper() + request.Substring(1);

                if (request == "H")
                {
                    RealType = SessionTypes.Human;
                }
                else if (request == "R")
                {
                    RealType = SessionTypes.Robot;
                }
                else
                    Requests.Add(request);
            }
        }

        #endregion


    }
}