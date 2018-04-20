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
            MethodUsed = "Offline";
            PositiveNegative = 0;
            NegativePositive = 0;
        }

        public static ClassResultsDesign Instance => new ClassResultsDesign();
    }
}