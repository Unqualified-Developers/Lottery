using System.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows.Controls;

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
            Ani.ButtonBind(genb, Brushes.DeepSkyBlue, Brushes.DodgerBlue, Brushes.CornflowerBlue);
            c.MouseEnter += (s, e) => { Ani.ScaleAniShow(c, 1, 1.05); };
            c.MouseLeave += (s, e) => { Ani.ScaleAniShow(c, 1.05, 1); };
            c.PreviewMouseDown += (s, e) => { Ani.ScaleAniShow(c, 1.05, 0.95); };
            c.PreviewMouseUp += (s, e) => { Ani.ScaleAniShow(c, 0.95, 1.05); };
            Ani.TextBoxBind(mint);
            Ani.TextBoxBind(maxt);
            Ani.TextBoxBind(ignt);
            Ani.TextBoxBind(quat);
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
                    if (int.TryParse(range[0], out int min) && int.TryParse(range[1], out int max))
                    {
                        if (max < min)
                        {
                            min ^= max;
                            max = min ^ max;
                            min ^= max;
                        }
                        for (int i = min; i <= max; i++) { iset.Add(i); }
                    }
                }
                else if (int.TryParse(str, out int num)) { iset.Add(num); }
            }
            try
            {
                int mini = int.Parse(mint.Text);
                int maxi = int.Parse(maxt.Text);
                if (mini > maxi)
                {
                    mini ^= maxi;
                    maxi = mini ^ maxi;
                    mini ^= maxi;
                }
                if (!int.TryParse(quat.Text, out int quai)) quai = 1;
                else quai = int.Parse(quat.Text);
                int r = Generate(mini, maxi, iset, random);
                if (quai < 1 || quai > 99999) MyMessageBox.Display("Range", "The value of 'Quality' you entered is not in the valid range. Valid range: 1~99999.", this, MyMessageBoxStyles.Error);
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
            catch (FormatException) { MyMessageBox.Display("Check", "Please enter correct numbers.", this, MyMessageBoxStyles.Warning); }
            catch (NotImplementedException) { MyMessageBox.Display("Joke", "This is not a joke.", this, MyMessageBoxStyles.Warning); }
            catch (Exception ex) when (ex is OverflowException || ex is ArgumentException) { MyMessageBox.Display("Range", "The value of 'Minimum' or 'Maximum' entered is not in the valid range. Valid range: -2147483648~2147483646.", this, MyMessageBoxStyles.Error); }
            catch (InvalidOperationException) { MyMessageBox.Display("Check", "Why did 'Minimum' > 'Maximum'?", this, MyMessageBoxStyles.Warning); }
        }
    }
}
