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

        /// <summary>
        /// Apply a Color animation to the specified <see cref="Control"/>, animating its size from one value to another..
        /// </summary>
        /// <param name="button">The button to animate.</param>
        /// <param name="fromColor">The starting color of the animation.</param>
        /// <param name="toColor">The target color of the animation.</param>
        private static void AnimateColor(Control control, Brush fromColor, Brush toColor)
        {
            ColorAnimation colorAnimation = new ColorAnimation()
            {
                From = ((SolidColorBrush)fromColor).Color,
                To = ((SolidColorBrush)toColor).Color,
                Duration = new Duration(TimeSpan.FromMilliseconds(300))
            };
            control.Background = new SolidColorBrush(((SolidColorBrush)fromColor).Color);
            control.Background.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
        }

        /// <summary>
        /// Binds a button with color and scaling animations for different mouse events.
        /// </summary>
        /// <param name="b">The button to bind the animations to.</param>
        /// <param name="start">The brush color for the mouse enter state.</param>
        /// <param name="mid">The brush color for the default state.</param>
        /// <param name="end">The brush color for the mouse down state.</param>
        public static void ButtonBind(Button b, Brush start, Brush mid, Brush end)
        {
            b.Background = mid;
            b.Foreground = Brushes.White;
            b.MouseEnter += (s, e) => { AnimateColor(b, mid, start); };
            b.MouseLeave += (s, e) => { AnimateColor(b, start, mid); };
            b.PreviewMouseDown += (s, e) => 
            { 
                ScaleAniShow(b, 1.05, 0.95); 
                AnimateColor(b, start, end); 
            };
            b.PreviewMouseUp += (s, e) => 
            { 
                ScaleAniShow(b, 0.95, 1.05); 
                AnimateColor(b, end, start); 
            };
        }

        /// <summary>
        /// Binds a text box with scaling animations for mouse events.
        /// </summary>
        /// <param name="t">The text box to bind the animations to.</param>
        public static void TextBoxBind(TextBox t)
        {
            t.PreviewMouseDown += (s, e) => { ScaleAniShow(t, 1, 0.95); };
            t.PreviewMouseUp += (s, e) => { ScaleAniShow(t, 0.95, 1); };
        }
    }
}
