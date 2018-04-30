using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Projekt.ViewModels;

namespace Projekt
{
    public class FilesListViewModel : BasicViewModel
    {
        #region Public properties

        /// <summary>
        ///     Lista zapamiętanych plików do wyświetlenia
        /// </summary>
        public ObservableCollection<FileViewModel> List { get; set; } = new ObservableCollection<FileViewModel>();

        #endregion

        public List<FileViewModel> GetSelected()
        {
            return List.Where(x => x.IsSelected).ToList();
        }
    }
}