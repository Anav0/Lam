using Projekt.ViewModels;

namespace Projekt.GUI.Design
{
    public class SessionItemDesign : SessionItemViewModel
    {
        public static SessionItemDesign Instance => new SessionItemDesign();

        public SessionItemDesign()
        {
            PredictedType = "X";
            Type = "X";
            SessionElements = "test test test test test";
        }

    }

}
