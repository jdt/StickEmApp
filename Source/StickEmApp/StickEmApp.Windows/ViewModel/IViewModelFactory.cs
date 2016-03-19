using StickEmApp.Entities;

namespace StickEmApp.Windows.ViewModel
{
    public interface IViewModelFactory
    {
        VendorListItemViewModel VendorListItemViewModel(Vendor vendor);
    }
}