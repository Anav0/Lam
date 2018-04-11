using Projekt.ViewModels;
using System.Data;
using System.Windows.Controls;

namespace Projekt.GUI.Design
{
    public class NewDataGridWithLabelDesign : NewDataGridWithLabelViewModel
    {
        public static NewDataGridWithLabelDesign Instance => new NewDataGridWithLabelDesign();
        public NewDataGridWithLabelDesign()
        {
            DataTable dataTable = new DataTable("MatrixOfProbability");
            dataTable.Columns.Add("Testowa Kolumna1", typeof(string));
            dataTable.Columns.Add("Testowa Kolumna2", typeof(string));
            dataTable.Columns.Add("Testowa Kolumna3", typeof(string));
            dataTable.Columns.Add("Testowa Kolumna4", typeof(string));

            dataTable.Rows.Add("Testowy wiersz1", typeof(string));
            dataTable.Rows.Add("Testowy wiersz2", typeof(string));
            dataTable.Rows.Add("Testowy wiersz3", typeof(string));
            dataTable.Rows.Add("Testowy wiersz4", typeof(string));

            Data = dataTable.DefaultView;
            LabelContent = "Testowy LabelContent";

        }
    }
}
