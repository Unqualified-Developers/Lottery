using System.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
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
            Ani.ButtonBind(genb, Brushes.DeepSkyBlue, Brushes.DodgerBlue, Brushes.CornflowerBlue);
            Ani.ButtonBind(scrb, Brushes.DeepSkyBlue, Brushes.DodgerBlue, Brushes.CornflowerBlue);
            c.MouseEnter += (s, e) => { Ani.ScaleAniShow(c, 1, 1.05); };
            c.MouseLeave += (s, e) => { Ani.ScaleAniShow(c, 1.05, 1); };
            c.PreviewMouseDown += (s, e) => { Ani.ScaleAniShow(c, 1.05, 0.95); };
            c.PreviewMouseUp += (s, e) => { Ani.ScaleAniShow(c, 0.95, 1.05); };
            scrb.Click += (s, e) => { System.Diagnostics.Process.Start("https://github.com/Unqualified-Developers/Lottery"); };
            genb.Click += (s, e) => { Gen(); };

            Ani.TextBoxBind(mint);
            Ani.TextBoxBind(maxt);
            Ani.TextBoxBind(ignt);
            Ani.TextBoxBind(quat);
        }
        
        public void Gen()
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
                        if (min > max) (min, max) = (max, min);
                        for (int i = min; i <= max; i++) { iset.Add(i); }
                    }
                }
                else if (int.TryParse(str, out int num)) { iset.Add(num); }
            }
            try
            {
                int mini = int.Parse(mint.Text);
                int maxi = int.Parse(maxt.Text);
                if (mini > maxi) (mini, maxi) = (maxi, mini);
                int quai = int.TryParse(quat.Text, out int _quai) ? _quai : 1;
                if (quai > maxi - mini + 1 && c.IsChecked == true) throw new NotImplementedException();
                if (quai < 1 || quai > 99999) MyMessageBox.Display("Range", "The value of 'Quality' you entered is not in the valid range. Valid range: 1~99999.", this, MyMessageBoxStyles.Error);
                else if (quai != 1)
                {
                    int r;
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
                    MyMessageBox.Display("Generate", $"Numbers: {string.Join(", ", rl)}.", this, Gen);
                }
                else MyMessageBox.Display("Generate", $"Number {Generate(mini, maxi, iset, random)}.", this, Gen);
            }
            catch (FormatException) { MyMessageBox.Display("Check", "Please enter correct numbers.", this, MyMessageBoxStyles.Warning); }
            catch (NotImplementedException) { MyMessageBox.Display("Joke", "This is not a joke.", this, MyMessageBoxStyles.Warning); }
            catch (Exception ex) when (ex is OverflowException || ex is ArgumentException) { MyMessageBox.Display("Range", "The value of 'Minimum' or 'Maximum' entered is not in the valid range. Valid range: -2147483648~2147483646.", this, MyMessageBoxStyles.Error); }
        }

        public int Generate(int min, int max, HashSet<int> iset, Random r)
        {
            int i = 0;
            int re;
            do
            {
                re = r.Next(min, max + 1);
                i++;
            }
            while (iset.Contains(re) && i <= 10000000);
            if (i >= 10000000) throw new NotImplementedException();
            else return re;
        }
    }
}
