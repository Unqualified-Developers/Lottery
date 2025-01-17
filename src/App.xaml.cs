using System.Windows;

namespace Lottery
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public const string dataFilePath = "data.json";
        public static int MyMessageBoxFontSize { get; set; } = 17;
    }
}
