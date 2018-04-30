using Projekt.ViewModels;

namespace Projekt
{
    public class PresentationScreenDesign : PresentationScreenViewModel
    {
        public PresentationScreenDesign()
        {
            Title = "Wynik dla grupy 1";
            var viewmodel = new ResultsViewModel
            {
                FailurePercent = 50,
                SuccessPercent = 50,
                SessionsCount = 21550,
                KPercent = 75,
                TruePositive = 0,
                TrueNegative = 0,
                FalsePositive = 0,
                FalseNegative = 0,
                OnlineMethodUsed = 20000,
                OfflineMethodUsed = 1550
            };
            ContentPresented = new ResultsControl(viewmodel);
        }

        public static PresentationScreenDesign Instance => new PresentationScreenDesign();
    }
}