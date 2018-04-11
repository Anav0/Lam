namespace Projekt.ViewModels
{
    public class SessionItemViewModel : BasicViewModel
    {

        #region Public Properties

        /// <summary>
        /// Typ jaki przewidział program
        /// </summary>
        public string PredictedType { get; set; } = "";

        /// <summary>
        /// Typ jaki został nadany program
        /// </summary>
        public string Type { get; set; } = "";

        /// <summary>
        /// Elementy sesji
        /// </summary>
        public string SessionElements { get; set; } = "";

        #endregion
    }
}
