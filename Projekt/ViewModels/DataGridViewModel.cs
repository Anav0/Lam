namespace Projekt.ViewModels
{
    public class DataGridViewModel : BasicViewModel
    {
        /// <summary>
        ///     Źródło informacji DataGrida
        /// </summary>
        public object DataSource { get; set; }

        /// <summary>
        ///     Treść pt. co znajduje się w labelu
        /// </summary>
        public string ColumnTitle { get; set; } = "";
    }
}