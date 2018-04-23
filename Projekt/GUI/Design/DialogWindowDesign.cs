using Projekt.ViewModels;

namespace Projekt.GUI.Design
{
    public class DialogWindowDesign : DialogWindowViewModel
    {
        public DialogWindowDesign()
        {
            KValue = "Wartość k";
            DeltaValue = "Wartość delty";
            Message = "Instrukcja co do wprowadzane j przez użytkownika wartości";
            ButtonContent = "Zatwierdź";
        }

        public static DialogWindowDesign Instance => new DialogWindowDesign();
    }
}