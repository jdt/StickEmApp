using System.Collections.ObjectModel;
using StickEmApp.Dal;

namespace StickEmApp.Windows.ViewModel
{
    public class VendorListViewModel : ViewModelBase
    {
        private ObservableCollection<VendorViewModel> _vendorList;

        public VendorListViewModel(IViewModelFactory viewModelFactory, IVendorRepository vendorRepository)
        {
            using (new UnitOfWork())
            {
                _vendorList = new ObservableCollection<VendorViewModel>();
                foreach (var vendor in vendorRepository.SelectVendors())
                {
                    _vendorList.Add(viewModelFactory.VendorViewModel(vendor));
                }
            }
        }

        public ObservableCollection<VendorViewModel> VendorList
        {
            get { return _vendorList; }
            set
            {
                _vendorList = value;
                OnPropertyChanged("VendorList");
            }
        }
    }
}