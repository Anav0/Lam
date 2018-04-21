namespace Projekt.ViewModels
{
    public class ClassResultsViewModel : BasicViewModel
    {
        #region Public properties

        public int SessionsCount { get; set; }

        public float SucessPercent { get; set; }

        public float FailurePercent { get; set; }

        public double KPercent { get; set; }

        public float TrueNegative { get; set; }

        public float TruePositive { get; set; }

        public float FalseNegative { get; set; }

        public float FalsePositive { get; set; }


        public int OnlineMethodUsed { get; set; }

        public int OfflineMethodUsed { get; set; }

        public float Recall { get; set; }

        public float Precision { get; set; }

        public float Measure { get; set; }


        #endregion
    }
}