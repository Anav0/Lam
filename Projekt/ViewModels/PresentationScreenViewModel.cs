using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using Projekt.Commands;
using Projekt.GUI.UserControls;
using Projekt.ViewModels;

namespace Projekt
{
    public class PresentationScreenViewModel : BasicViewModel
    {
        public PresentationScreenViewModel()
        {
            ShowMenuCommand = new RelayCommand(ShowMenu);
            ClickAwayCommand = new RelayCommand(ClickAway);
            ActiveResults = new ObservableCollection<ResultsViewModel>();
            CreatePopup();
        }

        #region Private Methods

        private void CreatePopup()
        {
            var poupViewModel = new BasePopupViewModel
            {
                ArrowAlignmentHorizontal = ElementHorizontalAlignment.Right,
                TopArrowVisibility = true,
                BottomArrowVisibility = false,
                BubbleContent = new VerticalMenu
                {
                    DataContext = new MenuViewModel
                    {
                        Items =
                        {
                            new MenuItemViewModel {Text = "Wyniki", Icon = IconType.None, Type = MenuItemType.Header},
                            new MenuItemViewModel {Icon = IconType.None, Type = MenuItemType.Divider},
                            //new MenuItemViewModel
                            //{
                            //    Text = "Zapisz wynik do pliku",
                            //    Icon = IconType.Save,
                            //    Type = MenuItemType.TextAndIcon,
                            //    ClickCommand = new RelayCommand(SaveResults)
                            //},
                            new MenuItemViewModel
                            {
                                Text = "Wyczyść wynik",
                                Icon = IconType.Cancel,
                                Type = MenuItemType.TextAndIcon,
                                ClickCommand = new RelayCommand(ClearResults)
                            }
                        }
                    }
                }
            };


            MenuPopupDataContext = poupViewModel;
        }

        #endregion

        #region Public properties

        public string Title { get; set; }

        public bool MenuVisibility { get; set; }

        public bool HasResultsShown { get; set; }

        public FrameworkElement ContentPresented { get; set; }

        public BasePopupViewModel MenuPopupDataContext { get; set; }

        public BasicViewModel PerentViewModel { get; set; }

        public ObservableCollection<ResultsViewModel> ActiveResults { get; set; }

        #endregion

        #region Public Commands

        public ICommand ShowMenuCommand { get; set; }

        public ICommand ClickAwayCommand { get; set; }

        #endregion

        #region Command Methods

        private void ShowMenu(object obj)
        {
            MenuVisibility ^= true;
        }

        private void ClearResults(object obj)
        {
            ContentPresented = new PlaceHolderControl();
            HasResultsShown = false;
        }

        private void SaveResults(object obj)
        {
            //var dialog = new SaveFileDialog
            //{
            //    Filter = "Text documents (.txt)|*.txt",
            //    DefaultExt = ".txt"
            //};

            //if (HasResultsShown)
            //{
            //    foreach (var result in ActiveResults)
            //        if (dialog.ShowDialog() == true)
            //            File.WriteAllLines(dialog.FileName, result.PropertiesToList());
            //    MessageBox.Show("Pomyślnie zapisano wyniki", Title, MessageBoxButton.OK, MessageBoxImage.Information);
            //}
            //else
            //{
            //    MessageBox.Show("Nie można zapisać wyników", Title, MessageBoxButton.OK, MessageBoxImage.Exclamation);
            //}
        }

        private void ClickAway(object obj)
        {
            MenuVisibility = false;
        }

        #endregion
    }
}