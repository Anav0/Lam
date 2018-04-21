using System.Windows;
using System.Windows.Input;
using Projekt.Animations;
using Projekt.Commands;
using Projekt.GUI.UserControls;
using Projekt.ViewModels;

namespace Projekt
{
    public class EndResultsViewModel : BasicViewModel
    {
        public EndResultsViewModel()
        {
            ShowMenuCommand = new RelayCommand(ShowMenu);
            ClickAwayCommand = new RelayCommand(ClickAway);
            CreatePopup();
        }

        #region Public properties

        public string Title { get; set; }

        public bool MenuVisibility { get; set; } 

        public bool HasResultsShown { get; set; }

        public FrameworkElement ContentPresented { get; set; }

        public BasePopupViewModel MenuPopupDataContext { get; set; }

        public BasicViewModel PerentViewModel { get; set; }


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
        }

        private void SaveResults(object obj)
        {
            if (HasResultsShown)
            {
                ClassResultsViewModel viewmodel = new ClassResultsViewModel
                {
                    
                };
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
                        new MenuItemViewModel{Text = "Zapisz wynik",Icon = IconType.Save,Type = MenuItemType.TextAndIcon,ClickCommand = new RelayCommand(SaveResults)},
                        new MenuItemViewModel{Icon = IconType.None,Type = MenuItemType.Divider},
                        new MenuItemViewModel{Text = "Wyczyść wynik",Icon = IconType.Cancel,Type = MenuItemType.TextAndIcon,ClickCommand = new RelayCommand(ClearResults)}
                    },
                },
            };

            MenuPopupDataContext = poupViewModel;
        }


        #endregion

    }
}