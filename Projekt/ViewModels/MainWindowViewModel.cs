using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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

            DTMCGroupList = new List<DtmcGroup>();
            TestGroupsList = new List<TestedGroup>();

            var viewmodel = new PresentationScreenViewModel
            {
                Title = "Tu pojawią się obliczone wyniki",
                ContentPresented = new PlaceHolderControl(),
                PerentViewModel = this
            };
            MainContent = new PresentationScreenControl(viewmodel);

            LoadSavedResults(PathToSavedResults, FileType.Xml);
            LoadSavedDtmc(PathToSavedDTMCs, FileType.Json);

        }

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

        #region Private properties

        private string PathToSavedDTMCs { get; } = AppDomain.CurrentDomain.BaseDirectory + @"\SavedDTMC\";

        private string PathToSavedResults { get; } = AppDomain.CurrentDomain.BaseDirectory + @"\SavedResults\";
        #endregion

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
        public List<DtmcGroup> DTMCGroupList { get; set; }

        /// <summary>
        ///     Lista zawierająca badane grupy
        /// </summary>
        public List<TestedGroup> TestGroupsList { get; set; }

        /// <summary>
        ///     Content wyświetlany na ekranie głównym
        /// </summary>
        public UserControl MainContent { get; set; }

        public FilesListViewModel SavedResultsData { get; set; }

        public FilesListViewModel SavedDTMCData { get; set; }

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
            var menu = parameter as FrameworkElement;
            sb.Animate(IsMenuExpand, menu, 0.3, 0.2, 0.2);
        }

        private void GetDataFromFile(object obj)
        {
            //Dialog wczytujący plik
            var openFileDialog = new OpenFileDialog
            {
                Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*",
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() != true) return;

            foreach (var file in openFileDialog.FileNames)
            {
                var linesInFile = File.ReadAllLines(file);
                linesInFile = linesInFile.Where(x => !string.IsNullOrEmpty(x)).ToArray();

                if (linesInFile.Length == 0)
                {
                    MessageBox.Show("W pliku nie było danych które można wczytać", "Błąd",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (obj != null)
                {
                    if (DTMCGroupList.Count < 2 || DTMCGroupList.Count > 2)
                    {
                        DTMCGroupList = new List<DtmcGroup>();
                        MessageBox.Show(
                            "Nie wczytano odpowiedniej liczby plików testowych aby przeprowadzenie testu było możliwe",
                            "Błąd",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (DTMCGroupList.FindAll(x => x.Sessions[0].RealType == SessionTypes.Human).Count > 1)
                    {
                        DTMCGroupList = new List<DtmcGroup>();
                        MessageBox.Show(
                            "Wczytano więcej niż jeden plik testowy typu H", "Błąd",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    else if (DTMCGroupList.FindAll(x => x.Sessions[0].RealType == SessionTypes.Robot).Count > 1)
                    {
                        DTMCGroupList = new List<DtmcGroup>();
                        MessageBox.Show(
                            "Wczytano więcej niż jeden plik testowy typu R", "Błąd",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    TestGroupsList = new List<TestedGroup>();
                    CreateTestedFromFile(linesInFile,
                        (string)obj == "Online" ? DetectionType.Online : DetectionType.Offline);
                }
                else
                {
                    CreateDtmcFromFile(linesInFile);
                    if (DTMCGroupList[DTMCGroupList.Count - 1].Sessions.Count > 0)
                    {
                        MessageBox.Show("Wczytano plik i utworzono DTMC", "Sukces",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }


        #endregion

        #region Private methods

        private void LoadSavedResults(string path, FileType fileType)
        {
            if (SavedResultsData == null) SavedResultsData = new FilesListViewModel();
            Directory.CreateDirectory(path);

            Serializer serializer = new Serializer();

            foreach (var filename in Directory.EnumerateFiles(path))
            {
                FileViewModel viewModel = new FileViewModel
                {
                    IsSelected = false,
                    FilePath = filename,
                    Parent = SavedResultsData,
                    DisplayDataFrame = (PresentationScreenViewModel)MainContent.DataContext

                };
                if (fileType == FileType.Json)
                {
                    viewModel.FileName = serializer.ReadFromJsonFile<ResultsViewModel>(filename).GivenName;
                }
                else if (fileType == FileType.Xml)
                {
                    viewModel.FileName = serializer.ReadFromXmlFile<ResultsViewModel>(filename).GivenName;
                }
                SavedResultsData.List.Add(viewModel);

            }
        }

        private void LoadSavedDtmc(string path, FileType fileType)
        {
            if (SavedDTMCData == null) SavedDTMCData = new FilesListViewModel();
            Directory.CreateDirectory(path);

            Serializer serializer = new Serializer();

            foreach (var filename in Directory.EnumerateFiles(path))
            {
                FileViewModel viewModel = new FileViewModel
                {
                    IsSelected = false,
                    FilePath = filename,
                    Parent = SavedDTMCData,
                    DisplayDataFrame = (PresentationScreenViewModel)MainContent.DataContext

                };
                if (fileType == FileType.Json)
                {
                    viewModel.FileName = serializer.ReadFromJsonFile<TestedGroup>(filename).Name;
                    SavedDTMCData.List.Add(viewModel);

                }
                else if (fileType == FileType.Xml)
                {
                    viewModel.FileName = serializer.ReadFromXmlFile<TestedGroup>(filename).Name;
                    SavedDTMCData.List.Add(viewModel);
                }

            }
        }

        private void DisplayResults()
        {
            var resultsToDisplay = new ObservableCollection<ResultsViewModel>();
            var evaluator = new ModelEvaluation();
            var index = 0;

            foreach (var group in TestGroupsList)
            {
                index++;
                double k = group.Sessions[0].Kpercent;

                var incorrectHitsProcent =
                    (float)(group.WronglyAsHuman + group.WronglyAsRobot) / group.Sessions.Count * 100;
                var correctHits = 100 - incorrectHitsProcent;
                evaluator.EvaluateResults(group);

                var resultsViewModel = new ResultsViewModel
                {
                    FailurePercent = incorrectHitsProcent,
                    SuccessPercent = correctHits,
                    KPercent = k,
                    Delta = group.Delta,
                    SessionsCount = group.Sessions.Count,
                    OnlineMethodUsed = group.SumOnlineDetections,
                    OfflineMethodUsed = group.SumOfflineDetections,
                    MethodSelected = group.DetectionMethodSelected,
                    Recall = evaluator.Recall,
                    Precision = evaluator.Precision,
                    Measure = evaluator.Measure,
                    Accuracy = evaluator.Accuracy,
                    TrueNegative = evaluator.TrueNegative,
                    TruePositive = evaluator.TruePositive,
                    FalsePositive = evaluator.FalsePositive,
                    FalseNegative = evaluator.FalseNegative,
                    GivenName = "Wynik" + index
                };
                resultsToDisplay.Add(resultsViewModel);
            }

            var results = new Results(resultsToDisplay[0]);

            if (MainContent.GetType() == typeof(PresentationScreenControl))
            {
                var mainScreen = MainContent as PresentationScreenControl;
                mainScreen.mViewModel.ContentPresented = results;

                if (resultsToDisplay.Count > 0)
                {
                    mainScreen.mViewModel.ActiveResults = resultsToDisplay;
                    mainScreen.mViewModel.HasResultsShown = true;
                }

            }
        }

        private void PerformDetection(TestedGroup group, TestedSession session, DetectionType type)
        {
            if (DTMCGroupList != null && DTMCGroupList.Count >= 2)
                try
                {
                    switch (type)
                    {
                        case DetectionType.Online:

                            session.PerformOnlineDetection(DTMCGroupList, group);
                            break;

                        case DetectionType.Offline:
                            session.PerformOfflineDetection(DTMCGroupList);

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
                    "Pierwsza wartość to procentowa ilość sesji do przepracowania przed oceną, a druga to wartość delta różnicy między wynikiem dla DTMC R i H najlepiej w przedziale między 0.5 a 2 im mniejsza wartość tym bardziej wątpliwa ocena",
                KValue = "75",
                DeltaValue = "0.5",
                ButtonContent = "Zatwierdź"
            };
            window.viewmodel = _viewModel;
            window.DataContext = _viewModel;

            return window;
        }

        private void CreateTestedFromFile(string[] textLines, DetectionType detectionType)
        {
            var kwindow = CreateParameterDialog();

            if (detectionType == DetectionType.Online) kwindow.ShowDialog();

            var testedGroup = new TestedGroup { DetectionMethodSelected = detectionType };

            foreach (var line in textLines)
            {
                var session = new TestedSession();
                var requests = line.Split();
                requests = requests.Where(x => !string.IsNullOrEmpty(x)).ToArray();

                try
                {
                    session.Kpercent = float.Parse(kwindow.viewmodel.KValue, CultureInfo.InvariantCulture);
                    testedGroup.Delta = float.Parse(kwindow.viewmodel.DeltaValue, CultureInfo.InvariantCulture);
                }
                catch (ArgumentNullException)
                {
                    MessageBox.Show("Nie podano prawidłowego argumentu", "Błąd",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                catch (FormatException)
                {
                    MessageBox.Show("Podano zły typ danych", "Błąd",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                catch (OverflowException overEx)
                {
                    MessageBox.Show(overEx.Message, "Błąd",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                session.K = (int)(session.Kpercent / 100 * (requests.Length - 1));

                session.NumberOfRequests = requests.Length - 1;

                if (requests[0] == "R")
                    session.RealType = SessionTypes.Robot;
                else if (requests[0] == "H") session.RealType = SessionTypes.Human;

                for (var i = 1; i < requests.Length; i++)
                {
                    var currentRequest = new Request();
                    session.AddRequest(requests[i]);

                    var req2 = requests[i].First().ToString().ToUpper() + requests[i].Substring(1);
                    currentRequest.NameType = req2;

                    testedGroup.AddUniqueRequest(currentRequest);

                    if (!session.WasClassified && detectionType == DetectionType.Online)
                        PerformDetection(testedGroup, session, DetectionType.Online);
                }

                if (detectionType == DetectionType.Offline)
                    PerformDetection(testedGroup, session, DetectionType.Offline);

                testedGroup.AddSession(session);

                testedGroup.CalculateQuantities(session);

                testedGroup.UniqueRequest.Sort((x, y) =>
                    string.Compare(x.NameType, y.NameType, StringComparison.Ordinal));
            }

            TestGroupsList.Add(testedGroup);
            DisplayResults();
        }

        private void CreateDtmcFromFile(string[] textLines)
        {
            var dtmcGroup = new DtmcGroup();
            var session = new Session();

            foreach (var line in textLines)
            {
                session = new Session();
                var requests = line.Split();
                requests = requests.Where(x => !string.IsNullOrEmpty(x)).ToArray();


                if (requests[0] == "R")
                    session.RealType = SessionTypes.Robot;
                else if (requests[0] == "H") session.RealType = SessionTypes.Human;

                for (var i = 2; i < requests.Length; i++)
                {
                    var currentRequest = new Request();
                    session.AddRequest(requests[i]);

                    currentRequest.NameType = requests[i].First().ToString().ToUpper() + requests[i].Substring(1);

                    dtmcGroup.AddUniqueRequest(currentRequest);
                }

                dtmcGroup.Sessions.Add(session);

                dtmcGroup.UniqueRequest.Sort((x, y) =>
                    string.Compare(x.NameType, y.NameType, StringComparison.Ordinal));
            }

            dtmcGroup.CalculateDTMC();
            DTMCGroupList.Add(dtmcGroup);

            Serializer serializer = new Serializer();
            dtmcGroup.Name = "Grupa " + dtmcGroup.Sessions[0].RealType;
            serializer.WriteToJsonFile(PathToSavedDTMCs + Guid.NewGuid() + ".json", dtmcGroup);

            FileViewModel viewmodel = new FileViewModel
            {
                FileName = dtmcGroup.Name,
                FilePath = PathToSavedDTMCs + Guid.NewGuid() + ".json",
                Parent = SavedDTMCData,
            };
            SavedDTMCData.List.Add(viewmodel);
        }
    }
    #endregion
}




