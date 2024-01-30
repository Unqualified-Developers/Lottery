using System.Windows;
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
        public void Set(Brush start, Brush mid, Brush end)
        {
            Ani.ButtonBind(b, start, mid, end);
            Ani.ButtonBind(c, start, mid, end);
        }

        public MyMessageBox(string title, string content, Window owner, MyMessageBoxStyles style)
        {
            InitializeComponent();
            b.Click += (s, e) => { Close(); };
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
                    Set(Brushes.Tomato, Brushes.Red, Brushes.Crimson);
                    break;
            }
            Title = title;
            Owner = owner;
            t.Text = content;
        }

        public static void Display(string title, string content, Window owner, MyMessageBoxStyles style = MyMessageBoxStyles.Information)
        {
            MyMessageBox m = new MyMessageBox(title, content, owner, style);
            m.ShowDialog();
        }
    }
}
