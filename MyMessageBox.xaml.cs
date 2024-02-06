using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Lottery
{
    /// <summary>
    /// Interaction logic for MyMessageBox.xaml
    /// </summary>
    public enum MyMessageBoxStyles
    {
        Information,
        Warning,
        Error
    }
    
    public partial class MyMessageBox : Window
    {
        private readonly Button conb = new Button
        {
            Margin = new Thickness(3),
            Content = "Continue",
            Style = (Style)Application.Current.FindResource("ButtonStyle")
        };

        public void Set(Brush start, Brush mid, Brush end)
        {
            Ani.ButtonBind(b, start, mid, end);
            Ani.ButtonBind(c, start, mid, end);
        }

        public void SetMore(Brush start, Brush mid, Brush end)
        {
            Ani.ButtonBind(b, start, mid, end);
            Ani.ButtonBind(c, start, mid, end);
            Ani.ButtonBind(conb, start, mid, end);
        }

        public MyMessageBox()
        {
            InitializeComponent();
            cb.ItemsSource = new int[] { 9, 11, 13, 15, 17, 19, 21, 23, 25, 27, 29 };
            cb.SelectedIndex = 4;
            cb.SelectionChanged += (s, e) => { t.FontSize = int.Parse(cb.SelectedItem.ToString()); };
            b.Click += (s, e) => { Close(); };
        }

        public void Display(string title, string content, Window owner, Action action, MyMessageBoxStyles style = MyMessageBoxStyles.Information)
        {
            conb.Click += (s, e) => {
                Close();
                action(); 
            };
            RowDefinition newRow = new RowDefinition { Height = new GridLength(36) };
            g.RowDefinitions.Add(newRow);
            Grid.SetRow(conb, 3);
            Grid.SetColumnSpan(conb, 2);
            g.Children.Add(conb);
            c.Click += (s, e) => { Clipboard.SetText(content); };
            switch (style)
            {
                case MyMessageBoxStyles.Information:
                    SetMore(Brushes.DeepSkyBlue, Brushes.DodgerBlue, Brushes.CornflowerBlue);
                    break;
                case MyMessageBoxStyles.Warning:
                    SetMore(Brushes.Orange, Brushes.DarkOrange, Brushes.Coral);
                    break;
                case MyMessageBoxStyles.Error:
                    SetMore(new SolidColorBrush(Color.FromRgb(255, 75, 75)), Brushes.Red, Brushes.Crimson);
                    break;
            }
            Title = title;
            Owner = owner;
            t.Text = content;
            ShowDialog();
        }

        public void Display(string title, string content, Window owner, MyMessageBoxStyles style = MyMessageBoxStyles.Information)
        {
            c.Click += (s, e) => { Clipboard.SetText(content); };
            switch (style)
            {
                case MyMessageBoxStyles.Information:
                    Set(Brushes.DeepSkyBlue, Brushes.DodgerBlue, Brushes.CornflowerBlue);
                    break;
                case MyMessageBoxStyles.Warning:
                    Set(Brushes.Orange, Brushes.DarkOrange, Brushes.Coral);
                    break;
                case MyMessageBoxStyles.Error:
                    Set(new SolidColorBrush(Color.FromRgb(255, 50, 50)), Brushes.Red, Brushes.Crimson);
                    break;
            }
            Title = title;
            Owner = owner;
            t.Text = content;
            ShowDialog();
        }
    }
}
