using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Lottery
{
    public enum MyMessageBoxStyle
    {
        Information,
        Warning,
        Error
    }
    /// <summary>
    /// MyMessageBox.xaml 的交互逻辑
    /// </summary>
    public partial class MyMessageBox : Window
    {
        public MyMessageBox(string title, string content, Window owner, MyMessageBoxStyle style)
        {
            InitializeComponent();
            b.Click += (s, e) => { Close(); };
            c.Click += (s, e) => { Clipboard.SetText(content); };

            switch (style)
            {
                case MyMessageBoxStyle.Information:
                    t.Foreground = Brushes.DodgerBlue;
                    Ani.ButtonBind(b, Brushes.DeepSkyBlue, Brushes.DodgerBlue, Brushes.CornflowerBlue);
                    Ani.ButtonBind(c, Brushes.DeepSkyBlue, Brushes.DodgerBlue, Brushes.CornflowerBlue);
                    break;
                case MyMessageBoxStyle.Warning:
                    t.Foreground = Brushes.DarkOrange;
                    Ani.ButtonBind(b, Brushes.Orange, Brushes.DarkOrange, Brushes.Coral);
                    Ani.ButtonBind(c, Brushes.Orange, Brushes.DarkOrange, Brushes.Coral);
                    break;
                case MyMessageBoxStyle.Error:
                    t.Foreground = Brushes.Red;
                    Ani.ButtonBind(b, Brushes.Tomato, Brushes.Red, Brushes.Crimson);
                    Ani.ButtonBind(c, Brushes.Tomato, Brushes.Red, Brushes.Crimson);
                    break;
            }

            Title = title;
            Owner = owner;
            t.Text = content;
        }

        public static void Display(string title, string content, Window owner, MyMessageBoxStyle style = MyMessageBoxStyle.Information)
        {
            MyMessageBox m = new MyMessageBox(title, content, owner, style);
            m.ShowDialog();
        }
    }
}
