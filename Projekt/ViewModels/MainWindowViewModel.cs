using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Microsoft.Win32;
using Projekt.Classes;
using Projekt.GUI;

namespace Projekt.ViewModels
{
    public class MainWindowViewModel : BasicViewModel
    {
        public MainWindowViewModel()
        {
            //Inicjalizacja komend
            ShowUnknownReqTypesCommand = new RelayCommand(ShowUnknownReqTypes);
            ShowCurrentSessionsCommand = new RelayCommand(ShowCurrentSessions);
            ExpandMenuCommand = new RelayCommand(AnimateMenu);
            GetDataFromFileCommand = new RelayCommand(GetDataFromFile);
        }

        #region Public Command

        /// <summary>
        ///     Komenda pokazująca nierozpoznane typy żądań
        /// </summary>
        public ICommand ShowUnknownReqTypesCommand { get; set; }

        /// <summary>
        ///     Komenda pokazująca obecne sesję w nowym oknie
        /// </summary>
        public ICommand ShowCurrentSessionsCommand { get; set; }

        /// <summary>
        ///     Komenda wysuwająca menu bocznego
        /// </summary>
        public ICommand ExpandMenuCommand { get; set; }

        /// <summary>
        /// Komenda wczytująca dane z pliku
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
        /// Lista zawierająca wytrenowane grupy
        /// </summary>
        public List<SessionsGroup> DTMCList { get; set; }

        /// <summary>
        /// Lista zawierająca wytrenowane grupy
        /// </summary>
        public List<SessionsGroup> TestGroupsList { get; set; }

        #endregion

        #region Command Methods

        /// <summary>
        ///     Metoda wywietlająca w nowym oknie listę obecnych sesji
        /// </summary>
        /// <param name="obj"></param>
        private void ShowCurrentSessions(object obj)
        {

            //if (CurrentSessions.Count > 0)
            //{
            //    //Okno w którym pokażemy sesję
            //    var okienko = new InfoWindow
            //    {
            //        Title = "Wynik",
            //        WindowStartupLocation = WindowStartupLocation.CenterOwner,
            //        DataContext = this,
            //        Width = 800,
            //        Height = 600
            //    };

            //    // kontekst dla wyświetlanej w oknie listy
            //    var listDataContext = new List<SessionItemViewModel>();

            //    var list = new SessionItemList {DataContext = listDataContext};


            //    foreach (var session in CurrentSessions)
            //    {
            //        var itemDataContext = new SessionItemViewModel();
            //        var currentSession = session;
            //        var text = "";

            //        foreach (var element in currentSession.Requests) text += element.item1 + " ";

            //        itemDataContext.PredictedType = session.PredictedType ?? "-";
            //        itemDataContext.RealType = session.RealType ?? "-";
            //        itemDataContext.SessionElements = text;

            //        listDataContext.Add(itemDataContext);
            //    }

            //    InfoWindowContent = list;
            //    okienko.ShowDialog();
            //}
            //else
            //{
            //    MessageBox.Show("Nie ma sesji które mogły by zostać pokazane", "Alpaka", MessageBoxButton.OK,
            //        MessageBoxImage.Error);
            //}
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
            var menu = parameter as DockPanel;
            sb.Animate(IsMenuExpand, menu, 0.3, 0.2, 0.2);
        }


        private void GetDataFromFile(object obj)
        {
            SessionsGroup currentGroup = new SessionsGroup();

            //TODO: USUŃ PO TEŚCIE
            ShowParameterDialog();

            //Just some bools bro
            bool isItFirstRequest = true, isTestFile = false;

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
            }

            try
            {
                if (openFileDialog.ShowDialog() == true)
                    foreach (var file in openFileDialog.FileNames)
                    {
                        var arrayOfLines = File.ReadAllLines(file);

                        //Jeśli pusty to krzycz
                        if (arrayOfLines.Length == 0)
                        {
                            MessageBox.Show("W pliku nie było danych które można by wczytać", "Błąd",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            //Linijka po linijce AKA sesja po sesji
                            foreach (var line in arrayOfLines)
                            {
                                if (!string.IsNullOrEmpty(line))
                                {

                                    //TODO : Można zrobić przez GroupBy

                                    Session currentSession = new Session();
                                    var arrayOfRequests = line.Split();

                                    //Dla każdego żądania w sesji
                                    foreach (var req in arrayOfRequests)
                                    {
                                        Request currentRequest = new Request();
                                        var req2 = req.First().ToString().ToUpper() + req.Substring(1);

                                        if (isItFirstRequest)
                                        {
                                            // Jeśli jest to pierwsze słowo wczytanej linijki to traktuj je jako RealType
                                            currentSession.RealType = req2;
                                            isItFirstRequest = false;
                                        }
                                        else
                                        {
                                            currentRequest.NameType = req2;

                                            if (req2 == currentSession.RealType) continue;
                                            currentSession.Requests.Add(currentRequest.NameType);

                                            // Jeśli żądanie nie wystąpiło wcześniej to:
                                            if (currentGroup.GroupUniqueRequest.FindAll(x => x.NameType == req2)
                                                    .Count <= 0)
                                            {
                                                currentRequest.Quantity = 1;
                                                currentGroup.GroupUniqueRequest.Add(currentRequest);
                                            }
                                            // Jeśli wystąpiło
                                            else
                                            {
                                                currentGroup.GroupUniqueRequest
                                                    .Find(x => x.NameType == currentRequest.NameType).Quantity++;
                                            }

                                        }
                                    }

                                    currentGroup.SessionsList.Add(currentSession);

                                    isItFirstRequest = true;
                                }
                            }
                            //Sortuje listę występujących żądań
                            currentGroup.GroupUniqueRequest.Sort((x, y) => String.Compare(x.NameType, y.NameType, StringComparison.Ordinal));

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
                                //TODO: Zmień lokalizację komendy niżej w odpowiednie miejsce
                                ShowParameterDialog();
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

        private void ShowParameterDialog()
        {
            DialogWindow window = new DialogWindow();
            DialogWindowViewModel viewModel = new DialogWindowViewModel
            {
                Message = "Podaj procentową wartość jako ilość żądań z sesji jakie program ma przepracowaćaby ocenić czy sesja jest sesją R czy H",
                InsertValue = "50",
                ButtonContent = "Zatwierdź"
            };

            window.DataContext = viewModel;


            window.ShowDialog();
        }


        /// <summary>
        ///     Pokazuję okno z nieznanymi typami żądań
        /// </summary>
        /// <param name="obj"></param>
        private void ShowUnknownReqTypes(object obj)
        {
            //if (ArrayOfUnknownTypes.Count == 0)
            //{
            //    MessageBox.Show("Nie ma nieznanych typów żądań", "Info", MessageBoxButton.OK,
            //        MessageBoxImage.Information);
            //}
            //else
            //{
            //    var listBox = new ListBox();
            //    var okienko = new InfoWindow();
            //    okienko.DataContext = this;
            //    okienko.Title = "Lista nieznanych typów";
            //    okienko.ShowInTaskbar = false;

            //    foreach (var type in ArrayOfUnknownTypes) listBox.Items.Add(type);
            //    InfoWindowContent = listBox;
            //    okienko.ShowDialog();
            //}
        }


        #endregion

        #region Private Methods

        private void SavePredictedResults(SessionsGroup Dtmc)
        {
            //var CurrentSession = new Session();
            //var Lista = new List<string>();
            //float CorrectHits = 0;
            //float CorrectHitsProcent = 0;
            //float IncorrectHitsProcent = 0;
            //float IncorrectHits = 0;
            //var text = "";

            //var saveFileDialog = new SaveFileDialog();
            //saveFileDialog.Filter = "Text file (*.txt)|*.txt|C# file (*.cs)|*.cs";


            //if (saveFileDialog.ShowDialog() == true)
            //    if (saveFileDialog.FileName != null)
            //    {
            //        Lista.Add(text = "Znaleziono: " + CurrentSessions.Count);
            //        text = "";
            //        foreach (var session in CurrentSessions)
            //        {
            //            if (session.PredictedType == session.RealType)
            //                CorrectHits++;
            //            else
            //                IncorrectHits++;

            //            CurrentSession = session;
            //            text += session.RealType + " ";
            //            text += session.PredictedType + " ";
            //            foreach (var request in CurrentSession.Requests) text += request.item1 + " ";
            //            Lista.Add(text);
            //            text = "";
            //        }

            //        CorrectHitsProcent = CorrectHits / CurrentSessions.Count * 100;
            //        IncorrectHitsProcent = IncorrectHits / CurrentSessions.Count * 100;

            //        text = "Liczba poprawnych trafień: " + CorrectHits + " co stanowi: " + CorrectHitsProcent + "%";
            //        Lista.Add(text);
            //        text = "Liczba niepoprawnych trafień: " + IncorrectHits + " co stanowi: " + IncorrectHitsProcent +
            //               "%";
            //        Lista.Add(text);
            //        File.WriteAllLines(saveFileDialog.FileName, Lista);
            //    }
            //    else
            //    {
            //        MessageBox.Show("Nie udało się zapisać wyników do pliku tekstowego", "Błąd", MessageBoxButton.OK,
            //            MessageBoxImage.Error);
            //    }

        }

        #endregion
    }
}