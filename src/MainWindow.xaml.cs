using System.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Numerics;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Lottery
{
    public class Interval
    {
        public BigInteger Left {get; set;}
        public BigInteger Right {get; set;}

        public Interval(BigInteger left, BigInteger right)
        {
            Left = left;
            Right = right;
        }

        public bool Contains(BigInteger number) { return number >= Left && number <= Right; }

        public static bool In(List<Interval> list, BigInteger number)
        {
            if (list != null && list.Count > 0) { foreach (Interval interval in list) { if (interval.Contains(number)) return true; } }
            return false;
        }
    }

    internal class CannotGenerateException : Exception { public CannotGenerateException(string message) : base(message) { } }
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDisposable
    {
        private readonly RandomNumberGenerator random = RandomNumberGenerator.Create();
        private readonly object lockObject = new object();
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing) { random?.Dispose(); }
                disposed = true;
            }
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public MainWindow()
        {
            InitializeComponent();
            Closed += (s, e) => Dispose();
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
            Animation.TextBoxBind(splt);
            (mint.Text, maxt.Text, ignt.Text, quat.Text, splt.Text, ndc.IsChecked) = Storage.Load();
        }

        /// <summary>
        /// Generates a random <see cref="BigInteger"/> value within the specified range, excluding the numbers in the given HashSet and Intervals.
        /// </summary>
        /// <remarks>
        /// This method generates a random <see cref="BigInteger"/> value within the range specified by min and max, while ensuring that the generated number is not present in the provided HashSet and Intervals. <br/>
        /// If a suitable number cannot be found within 1, 000, 000 iterations, a <see cref="CannotGenerateException"/> is thrown.
        /// </remarks>
        /// <param name="min">The minimum value of the range.</param>
        /// <param name="max">The maximum value of the range.</param>
        /// <param name="iset">The HashSet containing the numbers to be excluded.</param>
        /// <param name="ilist">The list containing the intervals to be excluded.</param>
        /// <param name="r">The RandomNumberGenerator object used for generating random numbers.</param>
        /// <returns>A random <see cref="BigInteger"/> value within the specified range, excluding the numbers in the HashSet and Intervals.</returns>
        /// <exception cref="CannotGenerateException">Thrown when the maximum number of iterations is reached without finding a suitable number.</exception>
        private BigInteger Generate(BigInteger min, BigInteger max, HashSet<BigInteger> iset, List<Interval> ilist, RandomNumberGenerator r)
        {
            int i = 0;
            BigInteger re;
            BigInteger zeroBasedUpperBound = max - min;
            byte[] bytes = zeroBasedUpperBound.ToByteArray();
            byte lastByteMask = 0b11111111;
            for (byte mask = 0b10000000; mask > 0; mask >>= 1, lastByteMask >>= 1)
            { if ((bytes[bytes.Length - 1] & mask) == mask) break; }
            do
            {
                do
                {
                    r.GetBytes(bytes);
                    bytes[bytes.Length - 1] &= lastByteMask;
                    re = new BigInteger(bytes);
                }
                while (re > zeroBasedUpperBound);
                re += min;
                i++;
                lock (lockObject) { if (!iset.Contains(re) && !Interval.In(ilist, re)) { return re; } }
            }
            while (i <= 1000000);
            throw new CannotGenerateException("Cannot generate a valid random number.");
        }

        private async void GenbClick()
        {
            MyMessageBox m = new MyMessageBox();
            HashSet<BigInteger> iset = new HashSet<BigInteger>();
            List<Interval> ilist = new List<Interval>();
            foreach (string str in ignt.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (str.Contains('~'))
                {
                    string[] range = str.Split('~');
                    if (BigInteger.TryParse(range.FirstOrDefault(), out BigInteger min) && BigInteger.TryParse(range.Last(), out BigInteger max))
                    {
                        if (min > max) (min, max) = (max, min);
                        ilist.Add(new Interval(min, max));
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
                string spls = splt.Text != "" ? splt.Text : ", ";
                if (quai < 1 || quai > 99999)
                {
                    m.Display("Range", "The value of 'Quality' you entered is not in the valid range. Valid range: 1~99999.", this, MyMessageBoxStyles.Error);
                    return;
                }
                BigInteger r;
                BigInteger[] rl = new BigInteger[quai];
                bool ndc_checked = (bool)ndc.IsChecked;

                await Task.Run(() => 
                {
                    for (int i = 0; i < quai; i++)
                    {
                        r = Generate(mini, maxi, iset, ilist, random);
                        rl[i] = r;
                        if (ndc_checked) { lock (lockObject) { iset.Add(r); } }
                    }
                });

                Dispatcher.Invoke(() => { m.Display("Generate", string.Join(spls, rl), this, GenbClick); });
                Storage.Save(mini.ToString(), maxi.ToString(), ignt.Text, quai.ToString(), spls, ndc.IsChecked ?? false, App.MyMessageBoxFontSize);
            }
            catch (FormatException) { m.Display("Check", "Please enter correct numbers.", this, MyMessageBoxStyles.Warning); }
            catch (CannotGenerateException) { m.Display("Joke", "This is not a joke.", this, MyMessageBoxStyles.Error); }
        }
    }
}
