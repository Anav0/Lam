using System.Collections.Generic;

namespace Projekt.ViewModels
{
    public class SessionItemListViewModel : BasicViewModel
    {
        public SessionItemListViewModel(List<SessionItemViewModel> _ItemList)
        {
            ItemList = _ItemList;
        }

        public SessionItemListViewModel()
        {
        }

        public List<SessionItemViewModel> ItemList { get; set; }
    }
}