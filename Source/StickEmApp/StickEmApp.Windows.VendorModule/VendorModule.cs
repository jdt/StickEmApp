using Prism.Modularity;
using Prism.Regions;

namespace StickEmApp.Windows.VendorModule
{
    public class VendorModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public VendorModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion("MainRegion", typeof(Views.VendorListView));
        }
    }
}
