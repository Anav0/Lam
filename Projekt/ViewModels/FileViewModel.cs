using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
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
            OpenDirectoryCommand = new RelayCommand(OpenDirectory);
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
                    SetDisplayDataFrameTitle(FileName);
                }
            }
        }

        public string FilePath { get; set; }

        public bool IsSelected { get; set; }

        public FilesListViewModel Parent { get; set; }

        public bool WasEvaluated { get; set; }

        public PresentationScreenViewModel DisplayDataFrame { get; set; }

        public FileControlRepresents Represents { get; set; }

        #endregion

        #region Public commands

        public ICommand DeleteCommand { get; set; }

        public ICommand ShowDataCommand { get; set; }

        public ICommand OpenDirectoryCommand { get; set; }

        #endregion

        #region Command methods

        private void Delete(object obj)
        {
            if (Parent != null)
            {
                Parent.List.Remove(this);
                File.Delete(FilePath);
            }
        }

        private void ShowData(object obj)
        {
            //TODO: add show data for DTMC variant, for now return

            if (Represents == FileControlRepresents.DTMC) return;
            if (!WasEvaluated) return;

            if (DisplayDataFrame != null)
            {
                var serializer = new Serializer();

                try
                {
                    //TODO: optimise for better performance
                    var dataFrame = serializer.ReadFromJsonFile<TestedGroup>(FilePath).AsResultsControl();

                    DisplayDataFrame.ContentPresented = dataFrame;

                    DisplayDataFrame.Title = FileName;

                    DisplayDataFrame.HasResultsShown = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, FileName, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void OpenDirectory(object obj)
        {
            if (File.Exists(FilePath)) Process.Start(FilePath);
        }
        #endregion

        #region Private mathods

        private void SetDisplayDataFrameTitle(string title)
        {
            if (DisplayDataFrame != null) DisplayDataFrame.Title = title;
        }

        #endregion
    }
}