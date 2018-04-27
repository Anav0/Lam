using System.Collections.Generic;
using Projekt.ViewModels;

namespace Projekt
{
    /// <summary>
    ///     A view model for a menu
    /// </summary>
    public class MenuViewModel : BasicViewModel
    {
        /// <summary>
        ///     The items in this menu
        /// </summary>
        public List<MenuItemViewModel> Items { get; set; } = new List<MenuItemViewModel>();
    }
}