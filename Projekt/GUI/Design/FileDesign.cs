namespace Projekt
{
    internal class FileDesign : FileViewModel
    {
        public FileDesign()
        {
            FileName = "Plik testowy 1";
            FilePath = "C:\\User:\\Anna\\testowy.txt";
            IsSelected = true;
        }

        public static FileDesign Instance => new FileDesign();
    }
}