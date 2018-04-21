using Projekt.ViewModels;

namespace Projekt.GUI.Design
{
    public class ClassResultsDesign : ClassResultsViewModel
    {
        public ClassResultsDesign()
        {
            FailurePercent = 50;
            SucessPercent = 50;
            SessionsCount = 21550;
            KPercent = 75;
            TruePositive = 0;
            TrueNegative = 0;
            FalsePositive = 0;
            FalseNegative = 0;
            OfflineMethodUsed = 20000;
            OnlineMethodUsed = 1550;
        }

        public static ClassResultsDesign Instance => new ClassResultsDesign();
    }
}