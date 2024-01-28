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
    /// <summary>
    /// MyMessageBox.xaml 的交互逻辑
    /// </summary>
    public partial class MyMessageBox : Window
    {
        public MyMessageBox(string title, string content, Window owner)
        {
            InitializeComponent();
            b.Click += (s, e) => { Close(); };
            b.MouseEnter += (s, e) => { MainWindow.ScaleAniShow(b, 1, 1.05); };
            b.MouseLeave += (s, e) => { MainWindow.ScaleAniShow(b, 1.05, 1); };
            b.PreviewMouseDown += (s, e) => { MainWindow.ScaleAniShow(b, 1.05, 0.95); };
            b.PreviewMouseUp += (s, e) => { MainWindow.ScaleAniShow(b, 0.95, 1.05); };

            c.Click += (s, e) => { Clipboard.SetText(content); };
            c.MouseEnter += (s, e) => { MainWindow.ScaleAniShow(c, 1, 1.05); };
            c.MouseLeave += (s, e) => { MainWindow.ScaleAniShow(c, 1.05, 1); };
            c.PreviewMouseDown += (s, e) => { MainWindow.ScaleAniShow(c, 1.05, 0.95); };
            c.PreviewMouseUp += (s, e) => { MainWindow.ScaleAniShow(c, 0.95, 1.05); };
            Title = title;
            Owner = owner;
            t.Text = content;
        }

        public static void Display(string title, string content, Window owner)
        {
            MyMessageBox m = new MyMessageBox(title, content, owner);
            m.ShowDialog();
        }
    }
}
