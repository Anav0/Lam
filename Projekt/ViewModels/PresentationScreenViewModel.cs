using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using Projekt.Animations;
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
            ActiveResults = new List<ResultsViewModel>();
            CreatePopup();
        }

        #region Public properties

        public string Title { get; set; }

        public bool MenuVisibility { get; set; } 

        public bool HasResultsShown { get; set; }

        public FrameworkElement ContentPresented { get; set; }

        public BasePopupViewModel MenuPopupDataContext { get; set; }

        public BasicViewModel PerentViewModel { get; set; }

        public List<ResultsViewModel> ActiveResults { get; set; }

        public List<ResultsViewModel> StoredResults { get; set; }



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
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Text documents (.txt)|*.txt";
            dialog.DefaultExt = ".txt";

            if (HasResultsShown)
            {

                foreach (var result in ActiveResults)
                {
                    
                    if (dialog.ShowDialog() == true)
                    {
                        File.WriteAllLines(dialog.FileName, result.PropertiesToList());
                    }
                }
                MessageBox.Show("Pomyślnie zapisano wyniki", Title, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Nie można zapisać wyników", Title, MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void StoreResult(object obj)
        {

            if (HasResultsShown)
            {
                if (StoredResults == null)
                {
                    StoredResults = new List<ResultsViewModel>();
                }

                foreach (var result in ActiveResults)
                {
                    StoredResults.Add(result);
                    string filename = Guid.NewGuid() + ".txt";

                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"\SavedResults\");

                    File.WriteAllLines(AppDomain.CurrentDomain.BaseDirectory+@"\SavedResults\ "+ filename, result.PropertiesToList());
                }

            }
            else
            {
                MessageBox.Show("Nie można zapamiętać wyniku", Title, MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void ClickAway(object obj)
        {
            MenuVisibility = false;
        }

        #endregion

        #region Private Methods

        private void CreatePopup()
        {
            BasePopupViewModel poupViewModel = new BasePopupViewModel();

            poupViewModel.ArrowAlignmentHorizontal = ElementHorizontalAlignment.Right;
            poupViewModel.TopArrowVisibility = true;
            poupViewModel.BottomArrowVisibility = false;

            poupViewModel.BubbleContent = new VerticalMenu
            {
                DataContext = new MenuViewModel
                {
                    Items =
                    {
                        new MenuItemViewModel{Text = "Wyniki",Icon = IconType.None,Type = MenuItemType.Header},
                        new MenuItemViewModel{Icon = IconType.None,Type = MenuItemType.Divider},
                        new MenuItemViewModel{Text = "Zapisz wynik do pliku",Icon = IconType.Save,Type = MenuItemType.TextAndIcon,ClickCommand = new RelayCommand(SaveResults)},
                        new MenuItemViewModel{Text = "Zapamiętaj wynik",Icon = IconType.Drive,Type = MenuItemType.TextAndIcon,ClickCommand = new RelayCommand(StoreResult)},
                        new MenuItemViewModel{Text = "Wyczyść wynik",Icon = IconType.Cancel,Type = MenuItemType.TextAndIcon,ClickCommand = new RelayCommand(ClearResults)}
                    },
                },
            };

            MenuPopupDataContext = poupViewModel;
        }

        #endregion

    }
}