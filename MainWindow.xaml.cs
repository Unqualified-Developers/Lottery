using System.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Animation;
using System.Windows.Media;

namespace Lottery
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            genb.MouseEnter += (s, e) => { ScaleAniShow(genb, 1, 1.05); };
            genb.MouseLeave += (s, e) => { ScaleAniShow(genb, 1.05, 1); };
            genb.PreviewMouseDown += (s, e) => { ScaleAniShow(genb, 1.05, 0.95); };
            genb.PreviewMouseUp += (s, e) => { ScaleAniShow(genb, 0.95, 1.05); };
            c.MouseEnter += (s, e) => { ScaleAniShow(c, 1, 1.05); };
            c.MouseLeave += (s, e) => { ScaleAniShow(c, 1.05, 1); };
            c.PreviewMouseDown += (s, e) => { ScaleAniShow(c, 1.05, 0.95); };
            c.PreviewMouseUp += (s, e) => { ScaleAniShow(c, 0.95, 1.05); };
            mint.PreviewMouseDown += (s, e) => { ScaleAniShow(mint, 1, 0.95); };
            mint.PreviewMouseUp += (s, e) => { ScaleAniShow(mint, 0.95, 1); };
            maxt.PreviewMouseDown += (s, e) => { ScaleAniShow(maxt, 1, 0.95); };
            maxt.PreviewMouseUp += (s, e) => { ScaleAniShow(maxt, 0.95, 1); };
            ignt.PreviewMouseDown += (s, e) => { ScaleAniShow(ignt, 1, 0.95); };
            ignt.PreviewMouseUp += (s, e) => { ScaleAniShow(ignt, 0.95, 1); };
            quat.PreviewMouseDown += (s, e) => { ScaleAniShow(quat, 1, 0.95); };
            quat.PreviewMouseUp += (s, e) => { ScaleAniShow(quat, 0.95, 1); };
        }
        public int Generate(int min, int max, HashSet<int> iset, Random r)
        {
            int i = 0;
            int re;
            do
            {
                re = r.Next(min, max + 1);
                i++;
            } while (iset.Contains(re) && i <= 10000000);
            if (i >= 10000000) throw new NotImplementedException();
            else return re;
        }

        private void Gen(object sender, RoutedEventArgs e)
        {
            Random random = new Random();
            HashSet<int> iset = new HashSet<int>();
            foreach (string str in ignt.Text.Split(' '))
            {
                if (str.Contains('~'))
                {
                    string[] range = str.Split('~');
                    if (int.TryParse(range[0], out int min) && int.TryParse(range[1], out int max)) for (int i = min; i <= max; i++) { iset.Add(i); }
                }
                else if (int.TryParse(str, out int num)) { iset.Add(num); }
            }
            try
            {
                
                int mini = int.Parse(mint.Text);
                int maxi = int.Parse(maxt.Text);
                int num1 = maxi - mini - iset.Count + 1;
                string str1 = Convert.ToString(num1);
                if (mini > maxi) throw new InvalidOperationException();
                if (!int.TryParse(quat.Text, out int quai)) quai = 1;
                else quai = int.Parse(quat.Text);
                int r = Generate(mini, maxi, iset, random);
                if (quai < 1 || quai > 99999) MyMessageBox.Display("Range", "The value of 'Quality' you entered is not in the valid range. Valid range: 1~99999.", this);
                else if (c.IsChecked == true || num1 > quai) MyMessageBox.Display("Range", "The value of 'Quality' you entered is not in the valid range. Valid range: 1~" + str1 + ".", this);
                else if (quai != 1)
                {
                    int[] rl = new int[quai];
                    if (c.IsChecked == true)
                    {
                        for (int i = 0; i < quai; i++)
                        {
                            r = Generate(mini, maxi, iset, random);
                            rl[i] = r;
                            iset.Add(r);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < quai; i++)
                        {
                            r = Generate(mini, maxi, iset, random);
                            rl[i] = r;
                        }
                    }
                    MyMessageBox.Display("Generate", $"Numbers: {string.Join(", ", rl)}.", this);
                }
                else MyMessageBox.Display("Generate", $"Number {r}.", this);
            }
            catch (FormatException) { MyMessageBox.Display("Check", "The value of 'Quality' you entered is not in the valid range. Valid range: 1~99999.", this); }
            catch (NotImplementedException) { MyMessageBox.Display("Joke", "The value of 'Quality' you entered is not in the valid range. Valid range: 1~3200.", this); }
            catch (Exception ex) when (ex is OverflowException || ex is ArgumentException) { MyMessageBox.Display("Range", "The value of 'Quality' you entered is not in the valid range. Valid range: 1~3200.", this); }
            catch (InvalidOperationException) { MyMessageBox.Display("Check", "The value of 'Quality' you entered is not in the valid range. Valid range: 1~3200.", this); }
        }

        public static void ScaleAniShow(UIElement element, double Sizefrom, double Sizeto, double RenderX = 0.5, double RenderY = 0.5, int power = 5)
        {
            ScaleTransform scale = new ScaleTransform();
            element.RenderTransform = scale;  // Define the central position of the circle.
            element.RenderTransformOrigin = new Point(RenderX, RenderY);  // Define the transition animation, 'power' is the strength of the transition.
            DoubleAnimation scaleAnimation = new DoubleAnimation()
            {
                From = Sizefrom,  // Start value
                To = Sizeto,  // End value
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
    }
}
