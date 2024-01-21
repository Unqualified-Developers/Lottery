using System.Windows;
using System;
using System.Collections.Generic;
using System.Linq;

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
        }

        public static int Generate(int min, int max, HashSet<int> iset, Random r)
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
                if (mini > maxi) throw new InvalidOperationException();
                if (!int.TryParse(quat.Text, out int quai)) quai = 1;
                else quai = int.Parse(quat.Text);
                int r = Generate(mini, maxi, iset, random);
                if (quai < 1 || quai > 999) MessageBox.Show("The value of 'Quality' you entered is not in the valid range.\nValid range: 1~999.", "Range", 0, MessageBoxImage.Warning);
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
                    MessageBox.Show($"Numbers: {string.Join(", ", rl)}.", "Generate", 0, MessageBoxImage.Information);
                }
                else MessageBox.Show($"Number {r}.", "Generate", 0, MessageBoxImage.Information);
            }
            catch (FormatException) { MessageBox.Show("Please enter correct numbers.", "Check", 0, MessageBoxImage.Warning); }
            catch (NotImplementedException) { MessageBox.Show("This is not a joke.", "Joke", 0, MessageBoxImage.Warning); }
            catch (Exception ex) when (ex is OverflowException || ex is ArgumentException) { MessageBox.Show("The value of 'Minimum' or 'Maximum' entered is not in the valid range.\nValid range: -2147483648~2147483646.", "Range", 0, MessageBoxImage.Warning); }
            catch (InvalidOperationException) { MessageBox.Show("Why did 'Minimum' > 'Maximum'?", "Check", 0, MessageBoxImage.Warning); }
        }
    }
}