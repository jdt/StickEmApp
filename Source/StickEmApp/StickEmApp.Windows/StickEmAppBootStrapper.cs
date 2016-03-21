using System.ComponentModel.Composition.Hosting;
using System.Windows;
using Prism.Mef;
using Prism.Regions;
using StickEmApp.Windows.Infrastructure.Behaviors;

namespace StickEmApp.Windows
{
    public class StickEmAppBootStrapper : MefBootstrapper
    {
        protected override void ConfigureAggregateCatalog()
        {
            AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(StickEmAppBootStrapper).Assembly));
            AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(UnitOfWork).Assembly));
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();

            Application.Current.MainWindow = (Shell)Shell;
            Application.Current.MainWindow.Show();
        }

        protected override IRegionBehaviorFactory ConfigureDefaultRegionBehaviors()
        {
            var factory = base.ConfigureDefaultRegionBehaviors();

            factory.AddIfMissing("AutoPopulateExportedViewsBehavior", typeof(AutoPopulateExportedViewsBehavior));

            return factory;
        }
        
        protected override DependencyObject CreateShell()
        {
            return Container.GetExportedValue<Shell>();
        }
    }
}