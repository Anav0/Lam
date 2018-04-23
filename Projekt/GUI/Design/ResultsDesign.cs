using Projekt.ViewModels;

namespace Projekt.GUI.Design
{
    public class ResultsDesign : ResultsViewModel
    {
        public ResultsDesign()
        {
            FailurePercent = 50;
            SuccessPercent = 50;
            SessionsCount = 21550;
            KPercent = 75;
            TruePositive = 0;
            TrueNegative = 0;
            FalsePositive = 0;
            FalseNegative = 0;
            OfflineMethodUsed = 20000;
            OnlineMethodUsed = 1550;
        }

        public static ResultsDesign Instance => new ResultsDesign();
    }
}