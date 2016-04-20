using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;
using log4net;

namespace StickEmApp.Windows
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILog Logger = LogManager.GetLogger("StickEmApp");

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            log4net.Config.XmlConfigurator.Configure();

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            if(Logger.IsInfoEnabled)
                Logger.InfoFormat("Application started at {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            Thread.CurrentThread.CurrentCulture = new CultureInfo("nl-BE");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("nl-BE");
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
            
            var bootstrapper = new StickEmAppBootStrapper();
            bootstrapper.Run();
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Logger.Error("Unhandled exception on the AppDomain.CurrentDomain", e.ExceptionObject as Exception);
        }

        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Logger.Error("Unhandled Dispatcher exception", e.Exception);
        }
    }
}
