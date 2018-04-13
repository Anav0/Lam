

using System.Collections.Generic;
using Projekt.ViewModels;

namespace Projekt
{
    public class SessionItemListViewModel : BasicViewModel
    {
        public List<SessionItemViewModel> ItemList { get; set; }

        public SessionItemListViewModel(List<SessionItemViewModel> _ItemList)
        {
            ItemList = _ItemList;
        }

        public SessionItemListViewModel()
        {
        }
    }
}
