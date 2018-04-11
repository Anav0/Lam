namespace Projekt.Windows.Design
{
    public class InputDialogDesign : InputDialogViewModel
    {
       public static InputDialogDesign Instance => new InputDialogDesign();

        public InputDialogDesign()
        {
            Name = "Test1";
            Question = "Podaj nazwę testową";
            WindowTitle = "Test";
        }
    }
}
