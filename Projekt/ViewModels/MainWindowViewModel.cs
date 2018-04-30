using System;
using System.Collections.Generic;
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
            EvaluateSelectedFilesCommand = new RelayCommand(EvaluateSelectedFiles);

            var viewmodel = new PresentationScreenViewModel
            {
                Title = "Tu pojawią się obliczone wyniki",
                ContentPresented = new PlaceHolderControl(),
                PerentViewModel = this
            };
            MainScreenContent = new PresentationScreenControl(viewmodel);

            LoadSavedFile(SavedFilesPath);
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

        /// <summary>
        ///     Komenda która ocenia ponownie zaznaczone pliki
        /// </summary>
        public ICommand EvaluateSelectedFilesCommand { get; set; }

        #endregion region

        #region Private properties

        private string SavedFilesPath { get; } = AppDomain.CurrentDomain.BaseDirectory + @"\SavedFiles\";

        #endregion

        #region Public Properties

         /// <summary>
        ///     Mówi nam czy menu ma być rozszerzone
        /// </summary>
        public bool IsMenuExpand { get; set; }

        /// <summary>
        ///     Content wyświetlany na ekranie głównym
        /// </summary>
        public UserControl MainScreenContent { get; set; }

        public FilesListViewModel SavedResultsData { get; set; }

        public FilesListViewModel SavedDtmcData { get; set; }

        #endregion

        #region Command Methods
        private void EvaluateSelectedFiles(object obj)
        {
            Serializer serializer = new Serializer();
            List<TestedGroup> testedGroups = new List<TestedGroup>();
            List<DtmcGroup> dtmcGroups = new List<DtmcGroup>();

            var selectedResults = SavedResultsData.List.Where(x => x.IsSelected).ToList();
            var selectedDtmc = SavedDtmcData.List.Where(x => x.IsSelected).ToList();

            if (selectedResults.Count == 0 || selectedDtmc.Count < 2)
            {
                MessageBox.Show("Trzeba zaznaczyć przynajmniej jeden plik do oceny i dwa pliki DTMC, jeden R i drugi H", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            foreach (var element in selectedResults)
            {
                testedGroups.Add(serializer.ReadFromJsonFile<TestedGroup>(element.FilePath));
            }
            foreach (var element in selectedDtmc)
            {
                dtmcGroups.Add(serializer.ReadFromJsonFile<DtmcGroup>(element.FilePath));
            }

            if (dtmcGroups.FindAll(x => x.Sessions[0].RealType == SessionTypes.Robot).Count > 1 ||
                dtmcGroups.FindAll(x => x.Sessions[0].RealType == SessionTypes.Human).Count > 1)
            {
                MessageBox.Show("Zaznaczono więcej niż jeden plik Dtmc tego samego typu", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if ((string)obj == "Online")
            {
                try
                {
                    int index = 0;
                    var kwindow = CreateParameterDialog();
                    kwindow.ShowDialog();
                    List<string> allRequests = new List<string>();

                    foreach (var testedGroup in testedGroups)
                    {
                        testedGroup.Delta = float.Parse(kwindow.viewmodel.DeltaValue, CultureInfo.InvariantCulture);
                        testedGroup.ClearResults();
                        testedGroup.DetectionMethodSelected = DetectionType.Online;

                        foreach (var session in testedGroup.Sessions)
                        {
                            allRequests.Clear();
                            allRequests.AddRange(session.Requests);

                            session.ClearResults();
                            session.Requests.Clear();
                            session.Kpercent = float.Parse(kwindow.viewmodel.KValue, CultureInfo.InvariantCulture);
                            session.K = (int)Math.Round((session.Kpercent / 100) * allRequests.Count);

                            foreach (var request in allRequests)
                            {
                                session.Requests.Add(request);
                                session.PerformOnlineDetection(dtmcGroups, testedGroup);
                                if (session.WasClassified)
                                {
                                    session.Requests.Clear();
                                    session.Requests.AddRange(allRequests);
                                    break;
                                };
                            }

                            testedGroup.CalculateQuantities(session);
                        }

                        var fileViewModel = selectedResults[index];
                        fileViewModel.WasEvaluated = true;
                        DisplayResults(testedGroup);
                        StoreGroup(testedGroup, fileViewModel.FilePath);
                        index++;
                    }
                   

                }
                catch (ArgumentNullException)
                {
                    MessageBox.Show("Nie podano prawidłowego argumentu", "Błąd",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (FormatException)
                {
                    MessageBox.Show("Podano zły typ danych", "Błąd",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (OverflowException overEx)
                {
                    MessageBox.Show(overEx.Message, "Błąd",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if ((string)obj == "Offline")
            {
                int index = 0;

                foreach (var testedGroup in testedGroups)
                {
                    
                    testedGroup.DetectionMethodSelected = DetectionType.Offline;
                    testedGroup.ClearResults();

                    foreach (var session in testedGroup.Sessions)
                    {
                        session.PerformOfflineDetection(dtmcGroups);
                        testedGroup.CalculateQuantities(session);
                    }
                    var fileViewModel = selectedResults[index];
                    fileViewModel.WasEvaluated = true;
                    DisplayResults(testedGroup);
                    StoreGroup(testedGroup, fileViewModel.FilePath);
                    index++;
                }
            }
        }

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
                    LoadTestDataset(linesInFile);
                }
                else
                {
                    LoadDtmcDataset(linesInFile);
                }
            }
        }

        #endregion

        #region Private methods

        private void DisplayResults(TestedGroup testedGroup)
        {
            if (MainScreenContent.GetType() != typeof(PresentationScreenControl))
            {
                MainScreenContent = new PresentationScreenControl(new PresentationScreenViewModel());
            }

            var mainContentViewModel = (PresentationScreenViewModel)MainScreenContent.DataContext;

            mainContentViewModel.ContentPresented = testedGroup.AsResultsControl();
            mainContentViewModel.PerentViewModel = this;
            mainContentViewModel.HasResultsShown = true;
        }

        /// <summary>
        /// Zapisuje grupy jako pliki json
        /// </summary>
        /// <param name="group">Grupa do zapisania</param>
        /// <param name="pathToFIle">ścieżka zapisu, Jeśli nie zostanie po dana to plik zostanie zutomatycznie zapisany w domyślnej ścieżce</param>
        public void StoreGroup(Group group, string pathToFIle = null)
        {
            var serializer = new Serializer();
            bool isNewFile = false;
            FileViewModel fileViewModel = new FileViewModel();

            if (pathToFIle == null)
            {
                isNewFile = true;
                pathToFIle = SavedFilesPath + Guid.NewGuid() + ".json";

                if (SavedDtmcData == null) SavedDtmcData = new FilesListViewModel();
                if (SavedResultsData == null) SavedResultsData = new FilesListViewModel();

                fileViewModel = new FileViewModel
                {
                    FilePath = pathToFIle,
                    DisplayDataFrame = MainScreenContent.DataContext as PresentationScreenViewModel
                };
            }
           

            if (group.GetType() == typeof(TestedGroup))
            {
                var tsGroup = group as TestedGroup;
                fileViewModel.WasEvaluated = tsGroup.Sessions.Exists(x => x.WasClassified);

                fileViewModel.FileName = group.GivenName;
                fileViewModel.Parent = SavedResultsData;
                fileViewModel.Represents = FileControlRepresents.SavedResults;
                if (isNewFile) SavedResultsData.List.Add(fileViewModel);
                serializer.WriteToJsonFile(pathToFIle, group as TestedGroup);

            }
            else if(group.GetType() == typeof(DtmcGroup))
            {
                fileViewModel.FileName = group.GivenName;
                fileViewModel.Parent = SavedDtmcData;
                fileViewModel.Represents = FileControlRepresents.DTMC;
                if (isNewFile) SavedDtmcData.List.Add(fileViewModel);
                serializer.WriteToJsonFile(pathToFIle, group as DtmcGroup);
            }

        }

        private void LoadSavedFile(string path)
        {
            Directory.CreateDirectory(path);

            var serializer = new Serializer();

            if (SavedDtmcData == null) SavedDtmcData = new FilesListViewModel();
            if (SavedResultsData == null) SavedResultsData = new FilesListViewModel();

            foreach (var filename in Directory.EnumerateFiles(path))
            {
                var loadedGroup = serializer.ReadFromJsonFile<Group>(filename);

                var viewModel = new FileViewModel
                {
                    IsSelected = false,
                    FilePath = filename,
                    FileName = loadedGroup.GivenName,
                    DisplayDataFrame = (PresentationScreenViewModel) MainScreenContent.DataContext,
                };

                if (loadedGroup.GetType() == typeof(TestedGroup))
                {
                    var tsGroup = loadedGroup as TestedGroup;
                    viewModel.Parent = SavedResultsData;
                    viewModel.WasEvaluated = tsGroup.Sessions.Exists(x => x.WasClassified);
                    viewModel.Parent = SavedResultsData;
                    SavedResultsData.List.Add(viewModel);
                    viewModel.Represents = FileControlRepresents.SavedResults;
                }
                else if (loadedGroup.GetType() == typeof(DtmcGroup))
                {
                    viewModel.Parent = SavedDtmcData;
                    SavedDtmcData.List.Add(viewModel);
                    viewModel.Represents = FileControlRepresents.DTMC;
                }

            }
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

        private void LoadTestDataset(string[] textLines)
        {
            var thisGroup = new TestedGroup {GivenName = "Plik testowy"};

            foreach (var line in textLines)
            {
                var session = new TestedSession();
                var requests = line.Split();
                requests = requests.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                
                session.NumberOfRequests = requests.Length - 1;

                if (requests[0] == "R") session.RealType = SessionTypes.Robot;
                else if (requests[0] == "H") session.RealType = SessionTypes.Human;

                for (var i = 1; i < requests.Length; i++)
                {
                    var currentRequest = new Request();
                    session.AddRequest(requests[i]);

                    var req2 = requests[i].First().ToString().ToUpper() + requests[i].Substring(1);
                    currentRequest.NameType = req2;

                    thisGroup.AddUniqueRequest(currentRequest);
                }

                thisGroup.AddSession(session);
                thisGroup.UniqueRequest.Sort((x, y) =>
                    string.Compare(x.NameType, y.NameType, StringComparison.Ordinal));
            }

            StoreGroup(thisGroup);
        }

        private void LoadDtmcDataset(string[] textLines)
        {
            var dtmcGroup = new DtmcGroup();

            foreach (var line in textLines)
            {
                var session = new Session();
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

            dtmcGroup.GivenName = "Grupa " + dtmcGroup.Sessions[0].RealType;
            dtmcGroup.CalculateDTMC();

            StoreGroup(dtmcGroup);
        }

        #endregion

    }

}