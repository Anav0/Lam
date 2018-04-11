namespace Projekt
{
    public class WindowViewModel : BasicViewModel
    {
        public WindowViewModel()
        {
                
        }
       
        #region Public Properties
        public double WindowWidth { get; set; }
        public double WindowHeight { get; set; }
        public double PosX { get; set; }
        public double PosY { get; set; }

        public string Label { get; set; } = "Proszę czekać";

        #endregion
    }
}
