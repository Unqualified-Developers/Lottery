using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Lottery
{
    internal class Ani
    {
        /// <summary>
        /// Apply a scaling animation to the specified <see cref="UIElement"/>, animating its size from one value to another.
        /// </summary>
        /// <param name="element">The UI element to which the scaling animation will be applied.</param>
        /// <param name="sizeFrom">The starting size of the element.</param>
        /// <param name="sizeTo">The target size to which the element will be scaled.</param>
        /// <param name="renderX">The X-coordinate of the center of the scaling transformation (default is 0.5).</param>
        /// <param name="renderY">The Y-coordinate of the center of the scaling transformation (default is 0.5).</param>
        /// <param name="power">The strength of the transition animation (default is 5).</param>
        public static void ScaleAniShow(UIElement element, double sizeFrom, double sizeTo, double renderX = 0.5, double renderY = 0.5, int power = 5)
        {
            ScaleTransform scale = new ScaleTransform();
            element.RenderTransform = scale;  // Define the central position of the circle.
            element.RenderTransformOrigin = new Point(renderX, renderY);  // Define the transition animation, 'power' is the strength of the transition.
            DoubleAnimation scaleAnimation = new DoubleAnimation()
            {
                From = sizeFrom,  // Start value
                To = sizeTo,  // End value
                FillBehavior = FillBehavior.HoldEnd,
                Duration = TimeSpan.FromMilliseconds(250),  // Animation playback time
                EasingFunction = new PowerEase()  // Ease function
                {
                    EasingMode = EasingMode.EaseInOut,
                    Power = power
                }
            };
            scale.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
            scale.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);
        }

        public static void ButtonBind(Button b, Brush start, Brush mid, Brush end)
        {
            b.Background = mid;
            b.Foreground = Brushes.White;
            b.MouseEnter += (s, e) => { ScaleAniShow(b, 1, 1.05); b.Background = start; };
            b.MouseLeave += (s, e) => { ScaleAniShow(b, 1.05, 1); b.Background = mid; };
            b.PreviewMouseDown += (s, e) => { ScaleAniShow(b, 1.05, 0.95); b.Background = end; };
            b.PreviewMouseUp += (s, e) => { ScaleAniShow(b, 0.95, 1.05); b.Background = start; };
        }

        public static void TextBoxBind(TextBox t)
        {
            t.PreviewMouseDown += (s, e) => { ScaleAniShow(t, 1, 0.95); };
            t.PreviewMouseUp += (s, e) => { ScaleAniShow(t, 0.95, 1); };
        }
    }
}
