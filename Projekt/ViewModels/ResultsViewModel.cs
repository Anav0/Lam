using System.Collections.Generic;

namespace Projekt.ViewModels
{
    public class ResultsViewModel : BasicViewModel
    {
        public List<string> PropertiesToList()
        {
            var list = new List<string>();

            foreach (var prop in GetType().GetProperties()) list.Add(prop.Name + " " + prop.GetValue(this, null));

            return list;
        }

        #region Public properties

        public string GivenName { get; set; }

        public DetectionType MethodSelected { get; set; }

        public int SessionsCount { get; set; }

        public float SuccessPercent { get; set; }

        public float FailurePercent { get; set; }

        public double KPercent { get; set; }

        public float TrueNegative { get; set; }

        public float TruePositive { get; set; }

        public float FalseNegative { get; set; }

        public float FalsePositive { get; set; }

        public float Delta { get; set; }

        public int OnlineMethodUsed { get; set; }

        public int OfflineMethodUsed { get; set; }

        public float Recall { get; set; }

        public float Precision { get; set; }

        public float Accuracy { get; set; }

        public float Measure { get; set; }


        #endregion
    }
}