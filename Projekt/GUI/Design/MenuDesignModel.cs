﻿using System.Collections.Generic;

namespace Projekt
{
    /// <summary>
    ///     The design-time data for a <see cref="MenuViewModel" />
    /// </summary>
    public class MenuDesignModel : MenuViewModel
    {
        #region Constructor

        /// <summary>
        ///     Default constructor
        /// </summary>
        public MenuDesignModel()
        {
            Items = new List<MenuItemViewModel>(new[]
            {
                new MenuItemViewModel {Type = MenuItemType.Header, Text = "Design time header..."},
                new MenuItemViewModel {Text = "Menu item 1", Icon = IconType.Save},
                new MenuItemViewModel {Text = "Menu item 2", Icon = IconType.Cancel}
            });
        }

        #endregion

        #region Singleton

        /// <summary>
        ///     A single instance of the design model
        /// </summary>
        public static MenuDesignModel Instance => new MenuDesignModel();

        #endregion
    }
}