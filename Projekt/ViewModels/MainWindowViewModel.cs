using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Microsoft.Win32;
using Projekt.Animations;
using Projekt.Classes;
using Projekt.Commands;
using Projekt.GUI.UserControls;
using Projekt.GUI.Windows;
using Projekt.ViewModels;

namespace Projekt
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
                ContentPresented = new PlaceHolderControl(),
                PerentViewModel = this
            };
            MainContent = new EndResultsControl(viewmodel);
        }

        #region Private Methods

       

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
        ///     Lista zawierająca badane grupy
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
            else
            {
                if (TestGroupsList.Count > 0)
                {
                    DTMCList = new List<SessionsGroup>();
                    TestGroupsList = new List<SessionsGroup>();
                }
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
                                            float.Parse(Kwindow.viewmodel.InsertValue);

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

                                        if (req2 == currentSession.RealType)
                                        {
                                            if (req2 == "R") currentGroup.RobotCount++;
                                            else currentGroup.HumanCount++;
                                            continue;
                                        }

                                        currentGroup.AddUniqueRequest(currentRequest);

                                        if (isTestFile && !currentSession.wasClassified && (string) obj == "Online")
                                            PerformDetection(currentSession, DetectionType.Online);
                                    }

                                    if((string) obj == "Offline") PerformDetection(currentSession, DetectionType.Offline);

                                    currentGroup.SessionsList.Add(currentSession);

                                    currentGroup.CalculateQuantities(currentSession);
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
                                DisplayResults();
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

        private void DisplayResults()
        {
            List< ClassResultsViewModel> resultsToDisplay = new List<ClassResultsViewModel>();

            ModelEvaluation evaluator = new ModelEvaluation();

            foreach (var group in TestGroupsList)
            {

                double k = group.SessionsList[0].Kpercent;

                float incorrectHitsProcent = ((float)(group.WronglyAsHuman+group.WronglyAsRobot) / group.SessionsList.Count) * 100;
                var correctHits = 100 - incorrectHitsProcent;


                ClassResultsViewModel resultsViewModel = new ClassResultsViewModel
                {
                    FailurePercent = incorrectHitsProcent,
                    SucessPercent = correctHits,
                    KPercent = k,
                    SessionsCount = group.SessionsList.Count,
                    OnlineMethodUsed = group.SumOnlineDetections,
                    OfflineMethodUsed = group.SumOfflineDetections,
                    Recall = evaluator.CalculateRecall(group),
                    Precision = evaluator.CalculatePrecision(group),
                    Measure = evaluator.CalculateMeasure(),
                    TrueNegative = evaluator.TrueNegative,
                    TruePositive = -1,
                    FalsePositive = evaluator.FalsePositive,
                    FalseNegative = evaluator.FalseNegative,

                };

                resultsToDisplay.Add(resultsViewModel);
            }

            ClassResultsControl resultsControl = new ClassResultsControl(resultsToDisplay[0]);

            if (MainContent.GetType() == typeof(EndResultsControl))
            {
                EndResultsControl mainScreen = MainContent as EndResultsControl;
                mainScreen.mViewModel.ContentPresented = resultsControl;
            }
        }

        private void PerformDetection(Session currentSession, DetectionType type)
        {
            if (DTMCList != null && DTMCList.Count >= 2)
            {
                try
                {
                    switch (type)
                    {
                        case DetectionType.Online:

                            currentSession.PerformOnlineDetection(DTMCList);
                            break;

                        case DetectionType.Offline:
                            currentSession.PerformOfflineDetection(DTMCList);

                            break;

                    }
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