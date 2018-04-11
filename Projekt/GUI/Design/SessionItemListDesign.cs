using Projekt.ViewModels;
using System.Collections.Generic;

namespace Projekt.GUI.Design
{
    public class SessionItemListDesign : List
    {
        public static SessionItemListDesign Instance => new SessionItemListDesign();

        public SessionItemListDesign()
        {
            Items = new List<SessionItemViewModel>
            {
                new SessionItemViewModel
                {
                    PredictedType = "R",
                    SessionElements = "test1 test1 test1 test1 test1 "

                },
                new SessionItemViewModel
                {
                    PredictedType = "H",
                    SessionElements = "test2 test2 test2 test2 test2 "

                },
                new SessionItemViewModel
                {
                    PredictedType = "H",
                    SessionElements = "test3 test3 test3 test3 test3 "

                },
                new SessionItemViewModel
                {
                    PredictedType = "H",
                    SessionElements = "test4 test4 test4 test4 test4 "

                },
            };

            

        }
    }
}
