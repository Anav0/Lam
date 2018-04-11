using System.Collections.Generic;

namespace Projekt.ViewModels
{
    public class DataGridDisplayViewModel : BasicViewModel
    {
        /// <summary>
        /// Lista <see cref="NewDataGridWithLabelViewModel"/> do wyświetlenia
        /// </summary>
        public List<NewDataGridWithLabelViewModel> Items { get; set; } = new List<NewDataGridWithLabelViewModel>();
    }
}
