using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Projekt.Commands;
using Projekt.ViewModels;

namespace Projekt
{
    public class FileViewModel : BasicViewModel
    {

        public FileViewModel()
        {
            DeleteCommand = new RelayCommand(Delete);
            ShowDataCommand = new RelayCommand(ShowData);
        }


        #region Public properties

        private string _fileName = "";
        public string FileName
        {
            get => _fileName;
            set
            {
                if (_fileName != value)
                {
                    _fileName = value;
                    new Serializer().UpdateXmlFile(FilePath, "GivenName", FileName);
                    SetDisplayDataFrameTitle(FileName);
                }
            }
        }

        public string FilePath { get; set; }

        public bool IsSelected { get; set; }

        public FilesListViewModel Parent { get; set; }

        public PresentationScreenViewModel DisplayDataFrame { get; set; }


        #endregion

        #region Public commands

        public ICommand DeleteCommand { get; set; }

        public ICommand ShowDataCommand { get; set; }

        #endregion

        #region Command methods
        private void Delete(object obj)
        {
            Parent?.List.Remove(this);
            File.Delete(FilePath);
        }

        private void ShowData(object obj)
        {
            if (DisplayDataFrame != null)
            {
                Serializer serializer = new Serializer();

                try
                {
                    ResultsViewModel viewmodel = serializer.ReadFromXmlFile<ResultsViewModel>(FilePath);

                    Results dataFrame = new Results(viewmodel);

                    DisplayDataFrame.ContentPresented = dataFrame;

                    DisplayDataFrame.Title = FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, FileName, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SetDisplayDataFrameTitle(string title)
        {
            if (DisplayDataFrame != null) DisplayDataFrame.Title = title;
        }
        #endregion
    }
}
