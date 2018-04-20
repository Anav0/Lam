using Projekt.GUI.UserControls;
using Projekt.ViewModels;

namespace Projekt.GUI.Design
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
                MethodUsed = "Offline",
                PositiveNegative = 0,
                NegativePositive = 0
            };
            ContentPresented = new ClassResultsControl(viewmodel);
        }

        public static EndResultsDesign Instance => new EndResultsDesign();
    }
}