using System.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Numerics;
using System.Security.Cryptography;

namespace Lottery
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly RNGCryptoServiceProvider random = new RNGCryptoServiceProvider();

        public MainWindow()
        {
            InitializeComponent();

            Closed += (s, e) => { Environment.Exit(0); };
            Animation.ButtonBind(genb, Brushes.DeepSkyBlue, Brushes.DodgerBlue, Brushes.CornflowerBlue);
            Animation.ButtonBind(scrb, Brushes.DeepSkyBlue, Brushes.DodgerBlue, Brushes.CornflowerBlue);
            ndc.MouseEnter += (s, e) => { Animation.Scale(ndc, 1, 1.05); };
            ndc.MouseLeave += (s, e) => { Animation.Scale(ndc, 1.05, 1); };
            ndc.PreviewMouseDown += (s, e) => { Animation.Scale(ndc, 1.05, 0.95); };
            ndc.PreviewMouseUp += (s, e) => { Animation.Scale(ndc, 0.95, 1.05); };
            scrb.Click += (s, e) => { System.Diagnostics.Process.Start("https://github.com/Unqualified-Developers/Lottery"); };
            genb.Click += (s, e) => { GenbClick(); };
            Animation.TextBoxBind(mint);
            Animation.TextBoxBind(maxt);
            Animation.TextBoxBind(ignt);
            Animation.TextBoxBind(quat);
            (mint.Text, maxt.Text, ignt.Text, quat.Text, ndc.IsChecked) = Storage.Load();
        }
        
        /// <summary>
        /// Generates a random <see cref="BigInteger"/> value within the specified range, excluding the numbers in the given HashSet.
        /// </summary>
        /// <remarks>
        /// This method generates a random <see cref="BigInteger"/> value within the range specified by min and max, while ensuring that the generated number is not present in the provided HashSet. <br/>
        /// If a suitable number cannot be found within 10,000,000 iterations, a <see cref="NotImplementedException"/> is thrown.
        /// </remarks>
        /// <param name="min">The minimum value of the range.</param>
        /// <param name="max">The maximum value of the range.</param>
        /// <param name="iset">The HashSet containing the numbers to be excluded.</param>
        /// <param name="r">The RNGCryptoServiceProvider object used for generating random numbers.</param>
        /// <returns>A random <see cref="BigInteger"/> value within the specified range, excluding the numbers in the HashSet.</returns>
        /// <exception cref="NotImplementedException">Thrown when the maximum number of iterations is reached without finding a suitable number.</exception>
        private BigInteger Generate(BigInteger min, BigInteger max, HashSet<BigInteger> iset, RNGCryptoServiceProvider r)
        {
            int i = 0;
            BigInteger re;
            do
            {
                BigInteger zeroBasedUpperBound = max - min;
                byte[] bytes = zeroBasedUpperBound.ToByteArray();
                byte lastByteMask = 0b11111111;
                for (byte mask = 0b10000000; mask > 0; mask >>= 1, lastByteMask >>= 1)
                { if ((bytes[bytes.Length - 1] & mask) == mask) break; }
                do
                {
                    r.GetBytes(bytes);
                    bytes[bytes.Length - 1] &= lastByteMask;
                    re = new BigInteger(bytes);
                }
                while (re > zeroBasedUpperBound);
                re += min;
                i++;
            }
            while (iset.Contains(re) && i <= 10000000);
            if (i == 10000001) throw new NotImplementedException();
            else return re;
        }

        private void GenbClick()
        {
            MyMessageBox m = new MyMessageBox();
            HashSet<BigInteger> iset = new HashSet<BigInteger>();
            foreach (string str in ignt.Text.Split(' '))
            {
                if (str.Contains('~'))
                {
                    string[] range = str.Split('~');
                    if (BigInteger.TryParse(range[0], out BigInteger min) && BigInteger.TryParse(range[1], out BigInteger max))
                    {
                        if (min > max) (min, max) = (max, min);
                        for (BigInteger i = min; i <= max; i++) { iset.Add(i); }
                    }
                }
                else if (BigInteger.TryParse(str, out BigInteger num)) { iset.Add(num); }
            }
            try
            {
                BigInteger mini = BigInteger.Parse(mint.Text);
                BigInteger maxi = BigInteger.Parse(maxt.Text);
                if (mini > maxi) (mini, maxi) = (maxi, mini);
                int quai = int.TryParse(quat.Text, out int _quai) ? _quai : 1;
                if (quai < 1 || quai > 99999) m.Display("Range", "The value of 'Quality' you entered is not in the valid range. Valid range: 1~99999.", this, MyMessageBoxStyles.Error);
                else if (quai != 1)
                {
                    BigInteger r;
                    BigInteger[] rl = new BigInteger[quai];
                    bool ndc_checked = (bool)ndc.IsChecked;
                    for (int i = 0; i < quai; i++)
                    {
                        r = Generate(mini, maxi, iset, random);
                        rl[i] = r;
                        if (ndc_checked) iset.Add(r);
                    }
                    m.Display("Generate", $"Numbers: {string.Join(", ", rl)}.", this, GenbClick);
                }
                else m.Display("Generate", $"Number {Generate(mini, maxi, iset, random)}.", this, GenbClick);
                Storage.Save(mini.ToString(), maxi.ToString(), ignt.Text, quai.ToString(), ndc.IsChecked ?? false, App.MyMessageBoxFontSize);
            }
            catch (FormatException) { m.Display("Check", "Please enter correct numbers.", this, MyMessageBoxStyles.Warning); }
            catch (NotImplementedException) { m.Display("Joke", "This is not a joke.", this, MyMessageBoxStyles.Error); }
        }
    }
}
