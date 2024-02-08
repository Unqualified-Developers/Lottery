using Microsoft.Win32;
using System;
using System.IO;
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
            Ani.ButtonBind(sb, start, mid, end);
        }

        public void SetMore(Brush start, Brush mid, Brush end)
        {
            Set(start, mid, end);
            Ani.ButtonBind(conb, start, mid, end);
        }

        public MyMessageBox()
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
                    try
                    {
                        using (StreamWriter sw = new StreamWriter(dialog.FileName))
                        {
                            sw.Write(t.Text);
                            sw.Close();
                            sw.Dispose();
                        }
                    }
                    catch (Exception ex)
                    {
                        MyMessageBox m = new MyMessageBox();
                        m.Display("Error", $"Error message: {ex.Message}", this, MyMessageBoxStyles.Error);
                    }
                }
            };
        }

        public void Display(string title, string content, Window owner, Action action, MyMessageBoxStyles style = MyMessageBoxStyles.Information)
        {
            conb.Click += (s, e) => {
                Close();
                action(); 
            };
            RowDefinition newRow = new RowDefinition { Height = new GridLength(36) };
            g.RowDefinitions.Add(newRow);
            Grid.SetRow(conb, 4);
            Grid.SetColumnSpan(conb, 2);
            g.Children.Add(conb);
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
            MinHeight = 236;
            ShowDialog();
        }

        public void Display(string title, string content, Window owner, MyMessageBoxStyles style = MyMessageBoxStyles.Information)
        {
            switch (style)
            {
                case MyMessageBoxStyles.Information:
                    Set(Brushes.DeepSkyBlue, Brushes.DodgerBlue, Brushes.CornflowerBlue);
                    break;
                case MyMessageBoxStyles.Warning:
                    Set(Brushes.Orange, Brushes.DarkOrange, Brushes.Coral);
                    break;
                case MyMessageBoxStyles.Error:
                    Set(new SolidColorBrush(Color.FromRgb(255, 75, 75)), Brushes.Red, Brushes.Crimson);
                    break;
            }
            Title = title;
            Owner = owner;
            t.Text = content;
            ShowDialog();
        }
    }
}
