using StickEmApp.Dal;
using StickEmApp.Entities;
using StickEmApp.Windows.View;

namespace StickEmApp.Windows.ViewModel
{
    public class ViewModelFactory : IViewModelFactory
    {
        public VendorListViewModel VendorListViewModel
        {
            get
            {
                return new VendorListViewModel(this, new ViewManager(), new VendorRepository());
            }
        }

        public VendorListItemViewModel VendorListItemViewModel(Vendor vendor)
        {
            return new VendorListItemViewModel(vendor.Name);
        }

        public VendorViewModel VendorViewModel
        {
            get
            {
                return new VendorViewModel(new VendorRepository(), new ViewManager());
            }
        }
    }
}