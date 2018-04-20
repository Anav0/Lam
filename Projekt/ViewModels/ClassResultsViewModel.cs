namespace Projekt.ViewModels
{
    public class ClassResultsViewModel : BasicViewModel
    {
        #region Public properties

        public int SessionsCount { get; set; }

        public int SucessPercent { get; set; }

        public int FailurePercent { get; set; }

        public int KPercent { get; set; }

        public string MethodUsed { get; set; }

        public int NegativePositive { get; set; }

        public int PositiveNegative { get; set; }

        #endregion
    }
}