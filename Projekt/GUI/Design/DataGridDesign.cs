using System.Data;
using Projekt.ViewModels;

namespace Projekt.GUI.Design
{
    public class DataGridDesign : DataGridViewModel
    {
        public DataGridDesign()
        {
            var dataTable = new DataTable("MatrixOfProbability");
            dataTable.Columns.Add("Testowa Kolumna1", typeof(string));
            dataTable.Columns.Add("Testowa Kolumna2", typeof(string));
            dataTable.Columns.Add("Testowa Kolumna3", typeof(string));
            dataTable.Columns.Add("Testowa Kolumna4", typeof(string));

            dataTable.Rows.Add("Testowy wiersz1", typeof(string));
            dataTable.Rows.Add("Testowy wiersz2", typeof(string));
            dataTable.Rows.Add("Testowy wiersz3", typeof(string));
            dataTable.Rows.Add("Testowy wiersz4", typeof(string));

            DataSource = dataTable.DefaultView;
            ColumnTitle = "Testowy columnTitle";
        }

        public static DataGridDesign Instance => new DataGridDesign();
    }
}