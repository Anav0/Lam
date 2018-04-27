using System;
using Projekt.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Projekt.Commands;

namespace Projekt
{
    public class FilesListViewModel : BasicViewModel
    {
        #region Public properties

        /// <summary>
        /// Lista zapamiętanych plików do wyświetlenia
        /// </summary>
        public ObservableCollection<FileViewModel> List { get; set; } = new ObservableCollection<FileViewModel>();

        #endregion
    }
}
