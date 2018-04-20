

namespace Projekt
{
    public class EndResultsDesign : EndResultsViewModel
    {
        public static EndResultsDesign Instance => new EndResultsDesign();

        public EndResultsDesign()
        {
            FailurePercent = 50;
            SucessPercent = 50;
            SessionsCount = 21550;
            KPercent = 75;
            Title = "Wyniki dla grupy 1";
            

        }
    }
}
