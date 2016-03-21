using System.Windows;
using Prism;

namespace StickEmApp.Windows
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var bootstrapper = new StickEmAppBootStrapper();
            bootstrapper.Run();
        }
    }
}
