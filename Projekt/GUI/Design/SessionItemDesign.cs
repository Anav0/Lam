using Projekt.ViewModels;

namespace Projekt.GUI.Design
{
    public class SessionItemDesign : SessionItemViewModel
    {
        public SessionItemDesign()
        {
            PredictedType = "X";
            RealType = "X";
            SessionElements = "test test test test test";
        }

        public static SessionItemDesign Instance => new SessionItemDesign();
    }
}