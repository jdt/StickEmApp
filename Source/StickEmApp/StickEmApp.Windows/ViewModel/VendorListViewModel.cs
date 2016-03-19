using System.Collections.ObjectModel;
using System.Windows.Input;
using StickEmApp.Dal;
using StickEmApp.Windows.View;

namespace StickEmApp.Windows.ViewModel
{
    public class VendorListViewModel : ViewModelBase
    {
        private readonly IViewFactory _viewFactory;
        private ObservableCollection<VendorListItemViewModel> _vendorList;

        public VendorListViewModel(IViewModelFactory viewModelFactory, IViewFactory viewFactory, IVendorRepository vendorRepository)
        {
            _viewFactory = viewFactory;
            using (new UnitOfWork())
            {
                _vendorList = new ObservableCollection<VendorListItemViewModel>();
                foreach (var vendor in vendorRepository.SelectVendors())
                {
                    _vendorList.Add(viewModelFactory.VendorListItemViewModel(vendor));
                }
            }
        }

        public ObservableCollection<VendorListItemViewModel> VendorList
        {
            get { return _vendorList; }
            set
            {
                _vendorList = value;
                OnPropertyChanged("VendorList");
            }
        }

        public ICommand AddVendorCommand { get { return new Command(i => AddVendor()); } }

        private void AddVendor()
        {
            _viewFactory.DisplayVendorView();
        }
    }
}