
namespace Projekt
{
    public class DtmcGroup : Group
    {

        #region Public Properties

        /// <summary>
        ///     Tablica zawierająca w sobie prawdopodobieństwo wystąpienia jednego żądania po drugim
        /// </summary>
        public float[,] Probability { get; set; }

        #endregion

        #region Public Methods

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
            foreach (var request in UniqueRequest)
            {
                request.SessionStarts = Sessions.FindAll(x => x.Requests[0] == request.NameType).Count;

                request.StarChances = (double)request.SessionStarts / Sessions.Count;
            }
        }

        /// <summary>
        ///     Tworzy tablice wielowymiarową zawierającą ilość wystąpień poszczególnych żądań po sobie oraz prawdobodobieństwo
        ///     takiego wystąpienia.
        /// </summary>
        private void CreateMatrix()
        {
            Probability = new float[UniqueRequest.Count, UniqueRequest.Count];
            Probability.Initialize();

            foreach (var session in Sessions)
                for (var i = 0; i < session.Requests.Count - 1; i++)
                {
                    var firstReq = session.Requests[i];
                    var nextReq = session.Requests[i + 1];

                    var posI = UniqueRequest.FindIndex(x => x.NameType == firstReq);
                    var posJ = UniqueRequest.FindIndex(x => x.NameType == nextReq);

                    Probability[posI, posJ]++;
                }

            for (var i = 0; i < UniqueRequest.Count; i++)
                for (var j = 0; j < UniqueRequest.Count; j++)
                    Probability[i, j] /= UniqueRequest[i].Quantity;
        }
        #endregion
    }



}
