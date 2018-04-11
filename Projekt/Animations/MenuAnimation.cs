using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace Projekt
{
    public static class MenuAnimation
    {

        public static void Animate(this Storyboard sb, bool direction, FrameworkElement element, double duration, double deacceleration, double acceleration)
        {
            switch (direction)
            {
                //Expand
                case true:
                    AnimateIn(element, duration, deacceleration);
                    break;
                //Collapse
                case false:
                    AnimateOut(element, duration, acceleration);
                    break;
            }
        }

        public static void AnimateIn(FrameworkElement element, double duration, double acceleration)
        {

            var sb = new Storyboard();
            var animation = new ThicknessAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(duration)),
                From = new Thickness(-element.Width + 70, 0, 0, 0),
                To = new Thickness(0),
                AccelerationRatio = acceleration
            };
            Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));
            sb.Children.Add(animation);
            sb.Begin(element);
        }

        public static void AnimateOut(FrameworkElement element, double duration, double deacceleration)
        {
            var sb = new Storyboard();
            var animation = new ThicknessAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(duration)),
                From = new Thickness(0),
                To = new Thickness(-element.Width + 70, 0, 0, 0),
                DecelerationRatio = deacceleration
            };
            Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));
            sb.Children.Add(animation);
            sb.Begin(element);
        }
    }
}
