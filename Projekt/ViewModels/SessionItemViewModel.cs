namespace Projekt.ViewModels
{
    public class SessionItemViewModel : BasicViewModel
    {
        #region Public Properties

        /// <summary>
        ///     Typ jaki przewidział program
        /// </summary>
        public string PredictedType { get; set; } = "";

        /// <summary>
        ///     Typ rzeczywisty
        /// </summary>
        public string RealType { get; set; } = "";

        /// <summary>
        ///     Żądania sesji sesji
        /// </summary>
        public string SessionElements { get; set; } = "";

        #endregion
    }
}