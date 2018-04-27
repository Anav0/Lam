

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Projekt
{
    public class FilesListDesign : FilesListViewModel
    {
        public static FilesListDesign Instance => new FilesListDesign();

        public FilesListDesign()
        {
            List = new ObservableCollection<FileViewModel>()
            {
                new FileViewModel {
                    FileName ="Testowy plik 1",
                    FilePath ="C:\\Userc\\Anna\\testowy_plik1.txt" },
                new FileViewModel {
                    FileName ="Testowy plik 2",
                    FilePath ="C:\\Userc\\Kasia\\testowy_plik2.txt" },
                new FileViewModel {
                    FileName ="Testowy plik 3",
                    FilePath ="C:\\Userc\\Zosia\\testowy_plik3.txt" },
                new FileViewModel {
                    FileName ="Testowy plik 4",
                    FilePath ="C:\\Userc\\Oliwia\\testowy_plik4.txt" },
            };
        }
    }
}
