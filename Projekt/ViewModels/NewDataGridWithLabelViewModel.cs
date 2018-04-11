using System.Data;
using System.Windows.Controls;

namespace Projekt.ViewModels
{
    public class NewDataGridWithLabelViewModel : BasicViewModel
    {

        /// <summary>
        /// Źródło informacji DataGrida
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// Treść pt. co znajduje się w labelu
        /// </summary>
        public string LabelContent { get; set; } = "";
    }
}
