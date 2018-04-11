using Projekt.ViewModels;
using System.Collections.Generic;
using System.Data;

namespace Projekt.GUI.Design
{
    public class DataGridDisplayDesign : DataGridDisplayViewModel
    {
        public static DataGridDisplayDesign Instance => new DataGridDisplayDesign();

        public DataGridDisplayDesign()
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

            Items = new List<NewDataGridWithLabelViewModel>
            {
                new NewDataGridWithLabelViewModel
                {
                    Data = dataTable.DefaultView,
                    LabelContent = "Label testowy numer 1"
                },
                 new NewDataGridWithLabelViewModel
                {
                    Data = dataTable.DefaultView,
                    LabelContent = "Label testowy numer 2"
                },
                  new NewDataGridWithLabelViewModel
                {
                    Data = dataTable.DefaultView,
                    LabelContent = "Label testowy numer 3"
                },
                   new NewDataGridWithLabelViewModel
                {
                    Data = dataTable.DefaultView,
                    LabelContent = "Label testowy numer 4"
                },
            };
        }
    }
}
