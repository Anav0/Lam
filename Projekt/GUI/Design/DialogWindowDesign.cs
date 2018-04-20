using Projekt.ViewModels;

namespace Projekt.GUI.Design
{
    public class DialogWindowDesign : DialogWindowViewModel
    {
        public DialogWindowDesign()
        {
            InsertValue = "Wartość liczbowa lub tekstowa";
            Message = "Instrukcja co do wprowadzane j przez użytkownika wartości";
            ButtonContent = "Zatwierdź";
        }

        public static DialogWindowDesign Instance => new DialogWindowDesign();
    }
}