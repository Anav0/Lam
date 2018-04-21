using System.Windows.Input;
using Projekt.ViewModels;

namespace Projekt
{
    /// <summary>
    /// A view model for a menu item
    /// </summary>
    public class MenuItemViewModel : BasicViewModel
    {
        /// <summary>
        /// The text to display for the menu item
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The icon for this menu item
        /// </summary>
        public IconType Icon { get; set; }

        /// <summary>
        /// The type of this menu item
        /// </summary>
        public MenuItemType Type { get; set; }

        /// <summary>
        /// Click command of an item
        /// </summary>
        public ICommand ClickCommand { get; set; }
    }
}
