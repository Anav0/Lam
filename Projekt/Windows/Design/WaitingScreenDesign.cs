using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt
{
    public class WaitingScreenDesign : WindowViewModel
    {
        public static WaitingScreenDesign Instance => new WaitingScreenDesign();
        public WaitingScreenDesign()
        {
            Label = "Testowy label";
            WindowHeight = 600;
            WindowWidth = 800;

        }
    }
}
