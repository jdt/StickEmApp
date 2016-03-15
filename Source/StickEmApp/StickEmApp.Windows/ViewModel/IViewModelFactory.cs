using StickEmApp.Entities;

namespace StickEmApp.Windows.ViewModel
{
    public interface IViewModelFactory
    {
        VendorViewModel VendorViewModel(Vendor vendor);
    }
}