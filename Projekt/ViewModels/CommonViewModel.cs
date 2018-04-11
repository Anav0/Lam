using Microsoft.Win32;
using Projekt.Classes;
using Projekt.Enums;
using Projekt.GUI;
using Projekt.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Projekt.ViewModels
{

    public class MainWindowViewModel : BasicViewModel
    {
        public MainWindowViewModel()
        {
            //Inicjalizacja komend
            CalculateCommand = new RelayCommand(Calculate);
            GetDataFromFileCommand = new RelayCommand((parameter) => GetDataFromFile(parameter));
            ShowUnknownReqTypesCommand = new RelayCommand((parameter) => ShowUnknownReqTypes(parameter));
            MaximizeDataGridCommand = new RelayCommand(MaximizeDataGrid);
            EvaluationCommand = new RelayCommand((parameter) => Evaluation(parameter));
            ShowCurrentSessionsCommand = new RelayCommand((parameter) => ShowCurrentSessions(parameter));
            ExpandMenuCommand = new RelayCommand((parameter) => AnimateMenu(parameter));
            GetDataFromTestFileCommand = new RelayCommand(GetDataFromTestFile);

        }

        #region Public Command
        /// <summary>
        /// Komenda która wykonuje metodę wykonującą obliczenia
        /// </summary>
        public ICommand CalculateCommand { get; set; }

        /// <summary>
        /// Komenda która wykonuje metodę pobierającą dane z pliku txt
        /// </summary>
        public ICommand GetDataFromFileCommand { get; set; }

        /// <summary>
        /// Komenda pokazująca nierozpoznane typy żądań
        /// </summary>
        public ICommand ShowUnknownReqTypesCommand { get; set; }
        /// <summary>
        /// Komanda która pokaże dany DataGrid na całym ekranie
        /// </summary>
        public ICommand MaximizeDataGridCommand { get; set; }

        /// <summary>
        /// Komenda która ocenia czy sesja jest sesją człowieka czy robota
        /// </summary>
        public ICommand EvaluationCommand { get; set; }

        /// <summary>
        /// Komenda pokazująca obecne sesję w nowym oknie
        /// </summary>
        public ICommand ShowCurrentSessionsCommand { get; set; }
        /// <summary>
        /// Komenda do wysunięcia menu bocznego
        /// </summary>
        public ICommand ExpandMenuCommand { get; set; }

        public ICommand GetDataFromTestFileCommand { get; set; }
        #endregion region

        #region Public Properties

        /// <summary>
        /// Zawiera informację o prawdopodobieństwie rozpoczęcia zapytania danym typem żądania + nazwę żądania
        /// </summary>
        public List<Pair<string, float>> StartChances { get; set; } = new List<Pair<string, float>>();

        /// <summary>
        /// Lista zwiera wszystkie sesję
        /// </summary>
        public ObservableCollection<Session> CurrentSessions { get; set; } = new ObservableCollection<Session>();

        /// <summary>
        /// To co ma się obecnie wyświetlić na ekranie
        /// </summary>
        public DataView CurrentContext { get; set; }

        /// <summary>
        /// To co ma się obecnie wyświetlić na ekranie 2
        /// </summary>
        public DataView CurrentContext2 { get; set; }

        /// <summary>
        /// Tablica zawierająca w sobie prawdopodobieństwo wystąpienia jednego elementu po drugim
        /// </summary>
        public float[,] Probability { get; set; } = new float[10, 10];

        /// <summary>
        /// Tablica zawierająca nazwy żądań wraz z liczbą ich występowań we wszystkich sesjach
        /// </summary>
        public List<Pair<string, float>> ArrayOfSymbols { get; set; } = new List<Pair<string, float>>();

        /// <summary>
        /// Informacja do wyświetlenia w oknie informacji
        /// </summary>
        public object InfoContent { get; set; } = new object();

        /// <summary>
        /// Lista wszystkich nieznanych typów
        /// </summary>
        public List<string> ArrayOfUnknownTypes { get; set; } = new List<string>();

        /// <summary>
        /// Lista wszystkich występujących wcześniej ArraysOfSymbols
        /// </summary>
        public List<List<Pair<string, float>>> ListOfAllSymbols { get; set; } = new List<List<Pair<string, float>>>();

        /// <summary>
        /// Lista wszystkich występujących wcześniej Probability
        /// </summary>
        public List<float[,]> ListOfAllArrayOfProbability { get; set; } = new List<float[,]>();

        /// <summary>
        /// Lista wszystkich występujących wcześniej CurrentSessions
        /// </summary>
        public List<ObservableCollection<Session>> ListOfAllListOfAllSessions { get; set; } = new List<ObservableCollection<Session>>();

        /// <summary>
        /// Lista wszystkich występujących wcześniej CurrentSessions
        /// </summary>
        public List<List<Pair<string, float>>> ListOfAllArrayOfStartChances { get; set; } = new List<List<Pair<string, float>>>();

        /// <summary>
        /// Lista zawierająca informację o typie poprzednich sesji
        /// </summary>
        public List<string> ListOfAllTypesOfSession { get; set; } = new List<string>();

        /// <summary>
        /// Mówi nam czy menu ma być rozszerzone
        /// </summary>
        public bool MenuExpand { get; set; } = false;

        public object DataGridDataContext { get; set; }
        #endregion

        #region Command Methods
        /// <summary>
        /// Metoda wywietlająca w nowym oknie listę obecnych sesji
        /// </summary>
        /// <param name="obj"></param>
        private void ShowCurrentSessions(object obj)
        {

            if (CurrentSessions.Count > 0)
            {
                var okienko = new InfoWindow();
                okienko.Title = "Wynik";
                okienko.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                okienko.DataContext = this;
                okienko.Width = 800;
                okienko.Height = 600;
                var ListDataContext = new List();
                var ItemDataContext = new SessionItemViewModel();

                var list = new SessionItemList();
                var item = new SessionItem();

                item.DataContext = ItemDataContext;
                list.DataContext = ListDataContext;

                foreach (var session in CurrentSessions)
                {
                    ItemDataContext = new SessionItemViewModel();
                    var currentSession = session;
                    string text = "";

                    foreach (var element in currentSession.Requests)
                    {
                        text += element.item1 + " ";
                    }

                    if (session.PredictedType != null)
                    {
                        ItemDataContext.PredictedType = session.PredictedType;
                    }
                    else
                    {
                        ItemDataContext.PredictedType = "-";
                    }
                    if (session.Type != null)
                    {
                        ItemDataContext.Type = session.Type;
                    }
                    else
                    {
                        ItemDataContext.Type = "-";

                    }
                    ItemDataContext.SessionElements = text;

                    ListDataContext.Items.Add(ItemDataContext);
                }

                InfoContent = list;
                okienko.ShowDialog();
            }
            else
            {
                MessageBox.Show("Nie ma sesji które mogły by zostać pokazane", "Alpaka", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Metoda wykonująca animację menu
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private void AnimateMenu(object parameter)
        {
            MenuExpand ^= true;
            var sb = new Storyboard();
            var menu = parameter as DockPanel;
            sb.Animate(MenuExpand, menu, 0.3, 0.2, 0.2);
        }

        /// <summary>
        /// Przeprowadza wszystkie potrzebne obliczenia
        /// </summary>
        /// <param name="obj"></param>
        /// 
        private void Calculate(object obj)
        {
            if (CurrentSessions.IsListPopulated())
            {
                StartChances = CurrentSessions.CalculateProbability();
                Probability = SessionCalculation.CreateMatrix(CurrentSessions, ArrayOfSymbols);

                if (StartChances.Count == 0)
                {
                    MessageBox.Show("Aby użyć tej opcji najpierw wczytaj dane i wykonaj obliczenia", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                ListOfAllTypesOfSession.Add(CurrentSessions[0].Type);
                ListOfAllSymbols.Add(ArrayOfSymbols);
                ListOfAllArrayOfProbability.Add(Probability);
                ListOfAllListOfAllSessions.Add(CurrentSessions);
                ListOfAllArrayOfStartChances.Add(StartChances);
            }
        }

        private void Evaluation(object obj)
        {
            RealTime realTime = new RealTime();

            List<string> ListOfSessionTypes = new List<string>();
            List<Pair<string, double>> ListOfPrevSessTypes = new List<Pair<string, double>>();

            var pair = new Pair<string, double>("coś", 0);
            var ListOfLastResult = new List<Pair<double[], string>>();

            double currHightNumber = 0;

            int index = 0, RNumber = 0, HNumber = 0;

            if (CurrentSessions.Count == 0)
            {
                MessageBox.Show("Aby użyć tej opcji najpierw wczytaj dane", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                foreach (var prevSession in ListOfAllListOfAllSessions)
                {
                    if (index != ListOfAllListOfAllSessions.Count - 1)
                    {
                        pair = new Pair<string, double>(prevSession[index].Type, 0);
                        ListOfSessionTypes.Add(prevSession[index].Type);
                        ListOfPrevSessTypes.Add(pair);
                        ListOfLastResult.Add(SessionCalculation.Traning(CurrentSessions, ListOfAllArrayOfStartChances[index], ListOfAllArrayOfProbability[index], ListOfAllSymbols[index], ListOfAllTypesOfSession[index]));
                        index++;
                    }
                }
                index = 0;

                //Lista zawiarająca dwie tablice z prawdopodobieństwami dla R i H
                ListOfLastResult = realTime.OptimizeDTMC(ListOfLastResult);

                //Porównuje wyniki dla różnych DTMC
                for (int i = 0; i < CurrentSessions.Count; i++)
                {
                    currHightNumber = -5000000;
                    for (int j = 0; j < ListOfLastResult.Count; j++)
                    {
                        for (int k = 0; k < ListOfLastResult.Count; k++)
                        {
                            if (ListOfLastResult[j].item1[i] < ListOfLastResult[k].item1[i] && ListOfLastResult[k].item1[i] > currHightNumber)
                            {
                                currHightNumber = ListOfLastResult[k].item1[i];
                                //TUTAJ NASTĘPUJE OCENA R CZY H
                                CurrentSessions[i].PredictedType = ListOfLastResult[k].item2;
                            }
                        }
                    }
                }
                MessageBoxResult result = MessageBox.Show("Skończono ewaluację danych, czy chcesz wyświetlić wynik? UWAGA dla dużej ilości wyników trwa to wieki", "Wybór", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (result == MessageBoxResult.Yes)
                {
                    DisplayPredictedResults();
                }
                else { }

                SavePredictedResults(RNumber, HNumber);

            }
        }


        private void GetDataFromTestFile(object obj)
        {
            #region Fields
            //Dialog wczytujący plik
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"

            };

            //Para nazwa - typ
            var pair = new Pair<string, ListOfRequestTypes>("coś", ListOfRequestTypes.Unknown);

            //Para nazwa - ilość występowań
            var pair2 = new Pair<string, float>("coś", 0);

            //Tablica zawierająca żądania z danej sesji
            var arrayOfRequests = new string[10];

            //Dana sesja
            var session = new Session();

            //Zerowanie
            StartChances = new List<Pair<string, float>>();
            Probability = new float[10, 10];
            CurrentSessions = new ObservableCollection<Session>();
            ArrayOfSymbols = new List<Pair<string, float>>();

            //Just some bools bro
            bool ExistsInTypeList = false, ExistsInUnknownList = false, IsFirstReq = true;
            #endregion

            try
            {
                if (openFileDialog.ShowDialog() == true)
                {
                    foreach (var file in openFileDialog.FileNames)
                    {
                        //Zerowanie
                        StartChances = new List<Pair<string, float>>();
                        Probability = new float[10, 10];
                        CurrentSessions = new ObservableCollection<Session>();
                        ArrayOfSymbols = new List<Pair<string, float>>();

                        string[] arrayOfLines = File.ReadAllLines(file);
                        //Jeśli pusty to krzycz
                        if (arrayOfLines.Length == 0)
                        {
                            MessageBox.Show("W pliku nie było danych które można by wczytać", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            //Linijka po linijce AKA sesja po sesji
                            foreach (string line in arrayOfLines)
                            {
                                arrayOfRequests = line.Split(new char[0]);
                                session = new Session();
                                var req2 = "";

                                //Dla każdego żądania w sesji
                                foreach (string req in arrayOfRequests)
                                {
                                    req2 = req.First().ToString().ToUpper() + req.Substring(1);

                                    if (IsFirstReq)
                                    {
                                        session.Type = req2;
                                        IsFirstReq = false;
                                    }
                                    else
                                    {
                                        if (req2 != session.Type)
                                        {

                                            if (!ArrayOfSymbols.Exists(x => x.item1 == req2))
                                            {
                                                ArrayOfSymbols.Add(new Pair<string, float>(req2, 1));
                                            }
                                            else
                                            {
                                                ArrayOfSymbols.Find(x => x.item1 == req2).item2++;
                                            }
                                            //Sprawdza czy znamy "klasę" żądania
                                            foreach (ListOfRequestTypes type in Enum.GetValues(typeof(ListOfRequestTypes)))
                                            {
                                                if (req2 == type.ToString())
                                                {

                                                    pair = new Pair<string, ListOfRequestTypes>(req2, type);
                                                    session.Requests.Add(pair);
                                                    ExistsInTypeList = true;
                                                }

                                            }
                                            if (!ExistsInTypeList) //Nie znamy :(
                                            {
                                                //Sprawdza czy ten nieznany typ został już kiedyś uwzględniony
                                                foreach (var value in ArrayOfUnknownTypes)
                                                {
                                                    if (value == req2)
                                                    {
                                                        ExistsInUnknownList = true;
                                                        break;
                                                    }
                                                }
                                                //Jeśli nie został uwzględnony to go dodajemy
                                                if (!ExistsInUnknownList)
                                                {
                                                    ArrayOfUnknownTypes.Add(req2);
                                                }
                                                pair = new Pair<string, ListOfRequestTypes>(req2, ListOfRequestTypes.Unknown);
                                                session.Requests.Add(pair);
                                            }

                                            ExistsInUnknownList = false;
                                            ExistsInTypeList = false;
                                        }
                                    }
                                }
                                IsFirstReq = true;
                                CurrentSessions.Add(session);

                            }

                            ArrayOfSymbols.Sort((x, y) => x.item1.CompareTo(y.item1));
                            Calculate(null);
                        }
                    }
                }
                if (!(openFileDialog.FileName == ""))
                {

                    MessageBox.Show("Wczytano informację z pliku znajdującego się: " + openFileDialog.FileName, "Udało się!", MessageBoxButton.OK, MessageBoxImage.Information);

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        /// <summary>
        /// Pobiera dane z pliku txt
        /// </summary>
        /// <param name="obj"></param>
        private void GetDataFromFile(object obj)
        {

            #region Fields
            //Dialog wczytujący plik
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"

            };
            openFileDialog.Multiselect = true;

            //Para nazwa - typ
            var pair = new Pair<string, ListOfRequestTypes>("coś", ListOfRequestTypes.Unknown);

            //Para nazwa - ilość występowań
            var pair2 = new Pair<string, float>("coś", 0);

            //Tablica zawierająca żądania z danej sesji
            var arrayOfRequests = new string[10];

            string oldName = "";

            //Dana sesja
            var session = new Session();
            int index = 0;

            //Zerowanie
            StartChances = new List<Pair<string, float>>();
            Probability = new float[10, 10];
            CurrentSessions = new ObservableCollection<Session>();
            ArrayOfSymbols = new List<Pair<string, float>>();

            //Just some bools bro
            bool ExistsInTypeList = false, ExistsInUnknownList = false, IsFirstReq = true;

            #endregion

            try
            {
                if (openFileDialog.ShowDialog() == true)
                {
                    foreach (var file in openFileDialog.FileNames)
                    {
                        //Zerowanie
                        StartChances = new List<Pair<string, float>>();
                        Probability = new float[10, 10];
                        CurrentSessions = new ObservableCollection<Session>();
                        ArrayOfSymbols = new List<Pair<string, float>>();

                        string[] arrayOfLines = File.ReadAllLines(file);
                        //Jeśli pusty to krzycz
                        if (arrayOfLines.Length == 0)
                        {
                            MessageBox.Show("W pliku nie było danych które można by wczytać", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {

                            InputDialog dialog = new InputDialog();
                            InputDialogViewModel viewModel = new InputDialogViewModel
                            {
                                Question = "Podaj nazwę dla przyszłego DTMC z pliku: ",
                                WindowTitle = "Nazwa DTMC"

                            };
                            viewModel.Question += openFileDialog.SafeFileNames[index];
                            dialog.DataContext = viewModel;
                            if (obj == null)
                            {
                                dialog.ShowDialog();
                                index++;
                            }

                            //Linijka po linijce AKA sesja po sesji
                            foreach (string line in arrayOfLines)
                            {
                                arrayOfRequests = line.Split(new char[0]);
                                session = new Session();
                                var req2 = "";

                                //Dla każdego żądania w sesji
                                foreach (string req in arrayOfRequests)
                                {
                                    req2 = req.First().ToString().ToUpper() + req.Substring(1);
                                    if (obj != null && IsFirstReq)
                                    {
                                        session.Type = viewModel.Name = req2;
                                    }
                                    if (IsFirstReq)
                                    {
                                        oldName = req2;

                                        session.Type = viewModel.Name;

                                        IsFirstReq = false;
                                    }
                                    else
                                    {
                                        if (req2 != oldName)
                                        {

                                            if (!ArrayOfSymbols.Exists(x => x.item1 == req2))
                                            {
                                                ArrayOfSymbols.Add(new Pair<string, float>(req2, 1));
                                            }
                                            else
                                            {
                                                ArrayOfSymbols.Find(x => x.item1 == req2).item2++;
                                            }
                                            //Sprawdza czy znamy "klasę" żądania
                                            foreach (ListOfRequestTypes type in Enum.GetValues(typeof(ListOfRequestTypes)))
                                            {
                                                if (req2 == type.ToString())
                                                {

                                                    pair = new Pair<string, ListOfRequestTypes>(req2, type);
                                                    session.Requests.Add(pair);
                                                    ExistsInTypeList = true;
                                                }

                                            }
                                            if (!ExistsInTypeList) //Nie znamy :(
                                            {
                                                //Sprawdza czy ten nieznany typ został już kiedyś uwzględniony
                                                foreach (var value in ArrayOfUnknownTypes)
                                                {
                                                    if (value == req2)
                                                    {
                                                        ExistsInUnknownList = true;
                                                        break;
                                                    }
                                                }
                                                //Jeśli nie został uwzględnony to go dodajemy
                                                if (!ExistsInUnknownList)
                                                {
                                                    ArrayOfUnknownTypes.Add(req2);
                                                }
                                                pair = new Pair<string, ListOfRequestTypes>(req2, ListOfRequestTypes.Unknown);
                                                session.Requests.Add(pair);
                                            }

                                            ExistsInUnknownList = false;
                                            ExistsInTypeList = false;
                                        }
                                    }
                                }
                                IsFirstReq = true;
                                CurrentSessions.Add(session);

                            }

                            ArrayOfSymbols.Sort((x, y) => x.item1.CompareTo(y.item1));
                            Calculate(null);
                        }
                    }
                }
                if (!(openFileDialog.FileName == ""))
                {

                    MessageBox.Show("Wczytano informację z pliku znajdującego się: " + openFileDialog.FileNames, "Udało się!", MessageBoxButton.OK, MessageBoxImage.Information);

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }



        }

        /// <summary>
        /// Pokazuję okno z nieznanymi typami żądań
        /// </summary>
        /// <param name="obj"></param>
        private void ShowUnknownReqTypes(object obj)
        {

            if (ArrayOfUnknownTypes.Count == 0)
            {
                MessageBox.Show("Nie ma nieznanych typów żądań", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                ListBox listBox = new ListBox();
                var okienko = new InfoWindow();
                okienko.DataContext = this;
                okienko.Title = "Lista nieznanych typów";
                okienko.ShowInTaskbar = false;

                foreach (var type in ArrayOfUnknownTypes)
                {
                    listBox.Items.Add(type);
                }
                InfoContent = listBox;
                okienko.ShowDialog();
            }

        }

        /// <summary>
        /// Pokazuję DataGrid w nowym oknie
        /// </summary>
        /// <param name="obj"></param>
        private void MaximizeDataGrid(object obj)
        {
            try
            {
                var pice = obj as DataGridWithLabel;
                var parent = pice.Parent;
                var panel = parent as Panel;
                panel.Children.Remove(pice);

                InfoWindow infoWindow = new InfoWindow();
                infoWindow.DataContext = this;
                infoWindow.Title = pice.Label;
                infoWindow.Width = 800;
                infoWindow.Height = 600;
                DataGrid dataGrid = new DataGrid();
                InfoContent = pice;
                infoWindow.ShowDialog();
                infoWindow.Content = null;

                panel.Children.Add(pice);
                parent = panel;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Methods

        void DisplayPredictedResults()
        {

            var okienko = new InfoWindow();
            okienko.Title = "Wynik";
            okienko.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            okienko.DataContext = this;

            var ListDataContext = new List();
            var ItemDataContext = new SessionItemViewModel();

            var list = new SessionItemList();
            var item = new SessionItem();

            item.DataContext = ItemDataContext;
            list.DataContext = ListDataContext;

            foreach (var session in CurrentSessions)
            {
                ItemDataContext = new SessionItemViewModel();
                var currentSession = session;
                string text = "";

                foreach (var element in currentSession.Requests)
                {
                    text += element.item1 + " ";
                }
                ItemDataContext.PredictedType = session.PredictedType;
                ItemDataContext.SessionElements = text;

                ListDataContext.Items.Add(ItemDataContext);


            }

            InfoContent = list;
            okienko.ShowDialog();

        }

        private void SavePredictedResults(float RNumber, float HNumber)
        {
            var CurrentSession = new Session();
            var Lista = new List<string>();
            float CorrectHits = 0;
            float CorrectHitsProcent = 0;
            float IncorrectHitsProcent = 0;
            float IncorrectHits = 0;
            string text = "";

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text file (*.txt)|*.txt|C# file (*.cs)|*.cs";


            if (saveFileDialog.ShowDialog() == true)
            {
                if (saveFileDialog.FileName != null)
                {
                    Lista.Add(text = "Znaleziono: " + CurrentSessions.Count);
                    text = "";
                    foreach (Session session in CurrentSessions)
                    {
                        if (session.PredictedType == session.Type)
                        {
                            CorrectHits++;
                        }
                        else
                        {
                            IncorrectHits++;
                        }

                        CurrentSession = session;
                        text += session.Type + " ";
                        text += session.PredictedType + " ";
                        foreach (var request in CurrentSession.Requests)
                        {
                            text += request.item1 + " ";

                        }
                        Lista.Add(text);
                        text = "";
                    }
                    CorrectHitsProcent = (CorrectHits / CurrentSessions.Count) * 100;
                    IncorrectHitsProcent = (IncorrectHits / CurrentSessions.Count) * 100;

                    text = "Liczba poprawnych trafień: " + CorrectHits + " co stanowi: " + CorrectHitsProcent + "%";
                    Lista.Add(text);
                    text = "Liczba niepoprawnych trafień: " + IncorrectHits + " co stanowi: " + IncorrectHitsProcent + "%";
                    Lista.Add(text);
                    File.WriteAllLines(saveFileDialog.FileName, Lista);
                }
                else
                {
                    MessageBox.Show("Nie udało się zapisać wyników do pliku tekstowego", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        #endregion
    }
}
