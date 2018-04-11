using Projekt.GUI;
using System.Collections.Generic;

namespace Projekt.ViewModels
{
    public class List : BasicViewModel
    {
        /// <summary>
        /// Lista wszystkich elementów
        /// </summary>
        public List<SessionItemViewModel> Items { get; set; } = new List<SessionItemViewModel>();
    }
}
