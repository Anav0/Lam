using System.Collections.Generic;
using Projekt.Classes;

namespace Projekt
{
    public class Group
    {

        #region Public Methods

        /// <summary>
        ///     Dodaje do <see cref="Sessions" /> takie <see cref="Request" /> które nie wystąpiło wcześniej
        /// </summary>
        /// <param name="request">Sprawdzane żądanie</param>
        public void AddUniqueRequest(Request request)
        {
            // Jeśli żądanie nie wystąpiło wcześniej to:
            if (UniqueRequest.FindAll(x => x.NameType == request.NameType).Count <= 0)
            {
                request.Quantity = 1;
                UniqueRequest.Add(request);
            }
            // Jeśli wystąpiło
            else
            {
                UniqueRequest
                    .Find(x => x.NameType == request.NameType).Quantity++;
            }
        }

        #endregion

        #region Public properties

        public string GivenName { get; set; }

        /// <summary>
        ///     Lista sesji znajdujących się we wczytanej grupie
        /// </summary>
        public List<Session> Sessions { get; set; } = new List<Session>();

        /// <summary>
        ///     Występujące typy żądań w danej grupie wraz z ich ilością
        /// </summary>
        public List<Request> UniqueRequest { get; set; } = new List<Request>();

        #endregion

    
    }
}