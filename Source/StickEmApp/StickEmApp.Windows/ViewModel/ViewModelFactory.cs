using StickEmApp.Dal;
using StickEmApp.Entities;

namespace StickEmApp.Windows.ViewModel
{
    public class ViewModelFactory : IViewModelFactory
    {
        public VendorListViewModel VendorListViewModel
        {
            get
            {
                return new VendorListViewModel(this, new VendorRepository());
            }
        }

        public VendorViewModel VendorViewModel(Vendor vendor)
        {
            return new VendorViewModel(vendor.Name);
        }
    }
}