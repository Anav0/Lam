using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Microsoft.Win32;
using Projekt.Animations;
using Projekt.Classes;
using Projekt.Commands;
using Projekt.GUI.UserControls;
using Projekt.GUI.Windows;

namespace Projekt.ViewModels
{
    public class MainWindowViewModel : BasicViewModel
    {
        public MainWindowViewModel()
        {
            //Inicjalizacja komend
            ExpandMenuCommand = new RelayCommand(AnimateMenu);
            GetDataFromFileCommand = new RelayCommand(GetDataFromFile);

            DTMCList = new List<SessionsGroup>();
            TestGroupsList = new List<SessionsGroup>();

            var viewmodel = new EndResultsViewModel
            {
                Title = "Tu pojawią się obliczone wyniki",
                ContentPresented = new PlaceHolderControl()
            };
            MainContent = new EndResultsControl(viewmodel);
        }

        #region Private Methods

        private void SavePredictedResults(List<SessionsGroup> groupList)
        {
            var lista = new List<string>();
            float correctHits = 0;
            float incorrectHits = 0;
            var index = 1;
            var k = "0%";
            var d = "0";
            float CorrectHitsProcent = 0;
            float IncorrectHitsProcent = 0;
            var saveFileDialog = new SaveFileDialog {Filter = "Text file (*.txt)|*.txt|C# file (*.cs)|*.cs"};


            if (saveFileDialog.ShowDialog() == true)
            {
                foreach (var group in groupList)
                {
                    foreach (var session in group.SessionsList)
                    {
                        if (session.PredictedType == session.RealType)
                            correctHits++;
                        else
                            incorrectHits++;

                        CorrectHitsProcent = correctHits / group.SessionsList.Count * 100;
                        IncorrectHitsProcent = incorrectHits / group.SessionsList.Count * 100;
                        k = session.Kpercent + "%";
                    }

                    string text;
                    lista.Add(text = "Dla " + index + " wczytanej grupy");
                    text = "";
                    lista.Add(text = "Procent przepracowanych sesji: " + k);
                    text = "";
                    lista.Add(text = "Znaleziono: " + group.SessionsList.Count + " sesji");
                    text = "";
                    text = "Liczba poprawnych trafień: " + correctHits + " co stanowi: " + CorrectHitsProcent + "%";
                    lista.Add(text);
                    text = "Liczba niepoprawnych trafień: " + incorrectHits + " co stanowi: " + IncorrectHitsProcent +
                           "%";

                    lista.Add(text);

                    index++;
                }

                File.WriteAllLines(saveFileDialog.FileName, lista);
            }
        }

        #endregion

        #region Public Command

        /// <summary>
        ///     Komenda wysuwająca menu bocznego
        /// </summary>
        public ICommand ExpandMenuCommand { get; set; }

        /// <summary>
        ///     Komenda wczytująca dane z pliku
        /// </summary>
        public ICommand GetDataFromFileCommand { get; set; }

        #endregion region

        #region Public Properties

        /// <summary>
        ///     Informacja do wyświetlenia w oknie informacji
        /// </summary>
        public object InfoWindowContent { get; set; } = new object();

        /// <summary>
        ///     Mówi nam czy menu ma być rozszerzone
        /// </summary>
        public bool IsMenuExpand { get; set; }

        /// <summary>
        ///     Lista zawierająca wytrenowane grupy
        /// </summary>
        public List<SessionsGroup> DTMCList { get; set; }

        /// <summary>
        ///     Lista zawierająca wytrenowane grupy
        /// </summary>
        public List<SessionsGroup> TestGroupsList { get; set; }

        /// <summary>
        ///     Content wyświetlany na ekranie głównym
        /// </summary>
        public UserControl MainContent { get; set; }

        #endregion

        #region Command Methods

        /// <summary>
        ///     Metoda wykonująca animację menu
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private void AnimateMenu(object parameter)
        {
            IsMenuExpand ^= true;
            var sb = new Storyboard();
            var menu = parameter as DockPanel;
            sb.Animate(IsMenuExpand, menu, 0.3, 0.2, 0.2);
        }

        private void GetDataFromFile(object obj)
        {
            var Kwindow = new DialogWindow();
            //DTMCList = new List<SessionsGroup>();

            //Just some bool bro
            var isTestFile = false;

            //Dialog wczytujący plik
            var openFileDialog = new OpenFileDialog
            {
                Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*",
                Multiselect = true
            };

            if (obj != null)
            {
                isTestFile = true;
                openFileDialog.Multiselect = false;
                Kwindow = CreateParameterDialog();
            }

            try
            {
                if (openFileDialog.ShowDialog() == true)
                    foreach (var file in openFileDialog.FileNames)
                    {
                        var currentGroup = new SessionsGroup();
                        var linesInFile = File.ReadAllLines(file);

                        //Jeśli pusty to krzycz
                        if (linesInFile.Length == 0)
                        {
                            MessageBox.Show("W pliku nie było danych które można by wczytać", "Błąd",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            //Linijka po linijce AKA sesja po sesji
                            foreach (var line in linesInFile)
                                if (!string.IsNullOrEmpty(line))
                                {
                                    var currentSession = new Session();

                                    var arrayOfRequests = line.Split();
                                    arrayOfRequests = arrayOfRequests.Where(x => !string.IsNullOrEmpty(x)).ToArray();

                                    if (isTestFile)
                                    {
                                        currentSession.Kpercent =
                                            double.Parse(Kwindow.viewmodel.InsertValue);

                                        currentSession.K =
                                            (int) (currentSession.Kpercent / 100 * arrayOfRequests.Length - 1);
                                        currentSession.NumberOfRequests = arrayOfRequests.Length - 1;
                                    }

                                    //Dla każdego żądania w sesji
                                    foreach (var req in arrayOfRequests)
                                    {
                                        var currentRequest = new Request();
                                        currentSession.FillSession(req);

                                        var req2 = req.First().ToString().ToUpper() + req.Substring(1);
                                        currentRequest.NameType = req2;

                                        if (req2 == currentSession.RealType) continue;

                                        currentGroup.AddUniqueRequest(currentRequest);

                                        if (isTestFile && !currentSession.wasClassified)
                                            PerformOnlineDetection(currentSession);
                                    }

                                    currentGroup.SessionsList.Add(currentSession);
                                }

                            //Sortuje listę występujących żądań
                            currentGroup.GroupUniqueRequest.Sort((x, y) =>
                                string.Compare(x.NameType, y.NameType, StringComparison.Ordinal));

                            // Jeśli wczytano plik do zbadania to dodaj go do innej grupy
                            if (!isTestFile)
                            {
                                // stwórz DTMC dla każdej grupy
                                currentGroup.CalculateDTMC();

                                DTMCList.Add(currentGroup);
                            }
                            else
                            {
                                TestGroupsList.Add(currentGroup);
                                SavePredictedResults(TestGroupsList);
                            }
                        }
                    }

                if (openFileDialog.FileName != "")
                    MessageBox.Show("Wczytano informację z pliku znajdującego się: " + openFileDialog.FileName,
                        "Udało się!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PerformOnlineDetection(Session currentSession)
        {
            if (DTMCList != null && DTMCList.Count >= 2)
                try
                {
                    currentSession.PerformOnlineDetection(DTMCList);
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(
                        "Nieprawidłowy argument " + ex.Message,
                        "Wczytywanie z pliku", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
                catch (FormatException ex)
                {
                    MessageBox.Show(
                        "Nieprawidłowy format " + ex.Message,
                        "Wczytywanie z pliku", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            else
                MessageBox.Show(
                    "Nie można poddać sesji ocenie gdyż nie wczytano wcześniej pików testowych",
                    "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private DialogWindow CreateParameterDialog()
        {
            var window = new DialogWindow();
            var _viewModel = new DialogWindowViewModel
            {
                Message =
                    "Podaj procentową wartość jako ilość żądań z sesji jakie program ma przepracowaćaby ocenić czy sesja jest sesją R czy H",
                InsertValue = "75",
                ButtonContent = "Zatwierdź"
            };
            window.viewmodel = _viewModel;
            window.DataContext = _viewModel;

            return window;
        }

        #endregion
    }
}