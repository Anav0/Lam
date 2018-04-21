using Projekt.GUI.UserControls;
using Projekt.ViewModels;

namespace Projekt
{
    public class EndResultsDesign : EndResultsViewModel
    {
        public EndResultsDesign()
        {
            Title = "Wynik dla grupy 1";
            var viewmodel = new ClassResultsViewModel
            {
                FailurePercent = 50,
                SucessPercent = 50,
                SessionsCount = 21550,
                KPercent = 75,
                TruePositive = 0,
                TrueNegative = 0,
                FalsePositive = 0,
                FalseNegative = 0,
                OnlineMethodUsed = 20000,
                OfflineMethodUsed = 1550
            };
            ContentPresented = new ClassResultsControl(viewmodel);
        }

        public static EndResultsDesign Instance => new EndResultsDesign();
    }
}