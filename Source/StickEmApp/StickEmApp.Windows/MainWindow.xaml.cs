using System.IO;
using System.Reflection;
using System.Windows;
using StickEmApp.Dal;

namespace StickEmApp.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            var dbFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "data.db");
            UnitOfWorkManager.Initialize(dbFile, DatabaseFileMode.CreateIfNotExists);

            InitializeComponent();
        }
    }
}
