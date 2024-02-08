using Microsoft.Win32;
using System;
using System.IO;
using System.Runtime.CompilerServices;
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
        
        private void Set(Brush start, Brush mid, Brush end)
        {
            Ani.ButtonBind(b, start, mid, end);
            Ani.ButtonBind(c, start, mid, end);
            Ani.ButtonBind(sb, start, mid, end);
        }

        private void SetMore(Brush start, Brush mid, Brush end)
        {
            Set(start, mid, end);
            Ani.ButtonBind(conb, start, mid, end);
        }

        private void Register(string title, string content, Window owner, bool c, MyMessageBoxStyles style)
        {
            switch (style)
            {
                case MyMessageBoxStyles.Information:
                    if (c) SetMore(Brushes.DeepSkyBlue, Brushes.DodgerBlue, Brushes.CornflowerBlue);
                    else Set(Brushes.DeepSkyBlue, Brushes.DodgerBlue, Brushes.CornflowerBlue);
                    break;
                case MyMessageBoxStyles.Warning:
                    if (c) SetMore(Brushes.Orange, Brushes.DarkOrange, Brushes.Coral);
                    else Set(Brushes.Orange, Brushes.DarkOrange, Brushes.Coral);
                    break;
                case MyMessageBoxStyles.Error:
                    if (c) SetMore(new SolidColorBrush(Color.FromRgb(255, 75, 75)), Brushes.Red, Brushes.Crimson);
                    else Set(new SolidColorBrush(Color.FromRgb(255, 75, 75)), Brushes.Red, Brushes.Crimson);
                    break;
            }
            Title = title;
            Owner = owner;
            t.Text = content;
        }

        private void Build()
        {
            InitializeComponent();
            cb.ItemsSource = new int[] { 9, 11, 13, 15, 17, 19, 21, 23, 25, 27, 29 };
            cb.SelectedIndex = 4;
            cb.SelectionChanged += (s, e) => { t.FontSize = int.Parse(cb.SelectedItem.ToString()); };
            b.Click += (s, e) => { Close(); };
            c.Click += (s, e) => { Clipboard.SetText(t.Text); };
            sb.Click += (s, e) =>
            {
                SaveFileDialog dialog = new SaveFileDialog()
                {
                    Title = "Save File",
                    Filter = "Text Files (*.txt)|*.txt"
                };
                if (dialog.ShowDialog() == true)
                {
                    try { File.WriteAllText(dialog.FileName, t.Text); }
                    catch (Exception ex) { Display("Error", $"Error message: {ex.Message}", this, MyMessageBoxStyles.Error); }
                }
            };
        }

        public MyMessageBox(string title, string content, Window owner, MyMessageBoxStyles style)
        {
            Build();
            Register(title, content, owner, false, style);
        }

        public MyMessageBox(string title, string content, Window owner, Action action, MyMessageBoxStyles style)
        {
            Build();
            conb.Click += (s, e) => {
                Close();
                action();
            };
            RowDefinition newRow = new RowDefinition { Height = new GridLength(36) };
            g.RowDefinitions.Add(newRow);
            Grid.SetRow(conb, 4);
            Grid.SetColumnSpan(conb, 2);
            g.Children.Add(conb);
            MinHeight = 236;
            Register(title, content, owner, true, style);
        }

        public static void Display(string title, string content, Window owner, Action action, MyMessageBoxStyles style = MyMessageBoxStyles.Information)
        {
            MyMessageBox m = new MyMessageBox(title, content, owner, action, style);
            m.ShowDialog();
        }

        public static void Display(string title, string content, Window owner, MyMessageBoxStyles style = MyMessageBoxStyles.Information)
        {
            MyMessageBox m = new MyMessageBox(title, content, owner, style);
            m.ShowDialog();
        }
    }
}
