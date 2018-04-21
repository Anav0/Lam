

using System.Windows;
using Projekt.ViewModels;

namespace Projekt
{
    public class BasePopupViewModel : BasicViewModel
    {

        #region Public properties

        /// <summary>
        /// Kierunek horyzontalny strzałki bąbelka
        /// </summary>
        public ElementHorizontalAlignment ArrowAlignmentHorizontal { get; set; }

        /// <summary>
        /// Kierunek wertykalny strzałki bąbelka
        /// </summary>
        public ElementVerticalAligment ArrowAlignmentVertical { get; set; }

        /// <summary>
        /// Content który bąbelek wyświetla
        /// </summary>
        public FrameworkElement BubbleContent { get; set; }

        /// <summary>
        /// Widoczność górnej strzałki
        /// </summary>
        public bool TopArrowVisibility { get; set; }

        /// <summary>
        /// Widoczność dolnej strzałki
        /// </summary>
        public bool BottomArrowVisibility { get; set; }

        #endregion
    }
}
