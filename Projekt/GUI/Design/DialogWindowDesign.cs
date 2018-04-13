namespace Projekt
{
    public class DialogWindowDesign : DialogWindowViewModel
    {
        public static DialogWindowDesign Instance => new DialogWindowDesign();

        public DialogWindowDesign()
        {
            InsertValue = "Wartość liczbowa lub tekstowa";
            Message = "Instrukcja co do wprowadzane j przez użytkownika wartości";
            ButtonContent = "Zatwierdź";
        }

    }
}
