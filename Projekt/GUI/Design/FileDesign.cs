
using Projekt.Commands;

namespace Projekt
{
    class FileDesign :FileViewModel
    {
        public static FileDesign Instance => new FileDesign();

        public FileDesign()
        {
            FileName = "Plik testowy 1";
            FilePath = "C:\\User:\\Anna\\testowy.txt";
            IsSelected = true;
        }
    }
}
