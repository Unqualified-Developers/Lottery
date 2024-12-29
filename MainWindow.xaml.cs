using System.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Numerics;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace Lottery
{
    public partial class MainWindow : Window
    {
        private readonly RNGCryptoServiceProvider random = new RNGCryptoServiceProvider();
        private const string DataFilePath = "data.json";

        public MainWindow()
        {
            InitializeComponent();
            Closed += (s, e) => { Environment.Exit(0); };
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

            LoadData();
        }

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

        private void Gen()
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
                    bool cc = (bool)c.IsChecked;
                    for (int i = 0; i < quai; i++)
                    {
                        r = Generate(mini, maxi, iset, random);
                        rl[i] = r;
                        if (cc) iset.Add(r);
                    }
                    m.Display("Generate", $"Numbers: {string.Join(", ", rl)}.", this, Gen);
                }
                else m.Display("Generate", $"Number {Generate(mini, maxi, iset, random)}.", this, Gen);

                SaveData(mini.ToString(), maxi.ToString(), ignt.Text, quai.ToString(), c.IsChecked ?? false);
            }
            catch (FormatException) { m.Display("Check", "Please enter correct numbers.", this, MyMessageBoxStyles.Warning); }
            catch (NotImplementedException) { m.Display("Joke", "This is not a joke.", this, MyMessageBoxStyles.Error); }
        }

        private void SaveData(string min, string max, string ignore, string quantity, bool checkbox)
        {
            Dictionary<string, string> data = new Dictionary<string, string>
            {
                { "min", min },
                { "max", max },
                { "ignore", ignore },
                { "quantity", quantity },
                { "no duplication", checkbox.ToString() }
            };

            using (StreamWriter writer = new StreamWriter(DataFilePath, false, Encoding.UTF8))
            {
                writer.WriteLine("{");
                int count = data.Count;
                foreach (KeyValuePair<string, string> kvp in data)
                {
                    count--;
                    writer.Write($"  \"{kvp.Key}\": \"{kvp.Value}\"");
                    if (count > 0) writer.WriteLine(",");
                    else writer.WriteLine();
                }
                writer.WriteLine("}");
            }
        }

        private void LoadData()
        {
            if (File.Exists(DataFilePath))
            {
                Dictionary<string, string> data = new Dictionary<string, string>();
                using (StreamReader reader = new StreamReader(DataFilePath, Encoding.UTF8))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(new[] { ':' }, 2);
                        if (parts.Length == 2)
                        {
                            string key = parts[0].Trim().Trim('"');
                            string value = parts[1].Trim().Trim('"', ',');
                            data[key] = value;
                        }
                    }
                }

                mint.Text = data.GetValueOrDefault("min", string.Empty);
                maxt.Text = data.GetValueOrDefault("max", string.Empty);
                ignt.Text = data.GetValueOrDefault("ignore", string.Empty);
                quat.Text = data.GetValueOrDefault("quantity", string.Empty);
                c.IsChecked = bool.TryParse(data.GetValueOrDefault("no duplication", "false"), out bool isChecked) && isChecked;
            }
        }
    }
}
