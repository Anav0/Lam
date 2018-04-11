using System;
using System.Windows.Input;

namespace Projekt.Windows
{
    public class InputDialogViewModel : BasicViewModel
    {
        public InputDialogViewModel()
        {
             OkClickCommand = new RelayCommand(OkClick);
        }

       
        #region Public Properties
        /// <summary>
        /// Nazwa DTMC
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Nazwa okna
        /// </summary>
        public string WindowTitle { get; set; }

        /// <summary>
        /// Jakie pytanie/polecenia zawiera okno
        /// </summary>
        public string Question { get; set; }
        #endregion

        #region Command
        /// <summary>
        /// Komenda dla przycisku OK
        /// </summary>
        public ICommand OkClickCommand { get; set; }
        #endregion

        #region Command Methods
        private void OkClick(object obj)
        {
            var okno = obj as InputDialog;
            okno.Close();
        }
        #endregion
    }
}
