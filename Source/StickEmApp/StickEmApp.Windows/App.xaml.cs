using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Markup;

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

            Thread.CurrentThread.CurrentCulture = new CultureInfo("nl-BE");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("nl-BE");
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
            
            var bootstrapper = new StickEmAppBootStrapper();
            bootstrapper.Run();
        }
    }
}
