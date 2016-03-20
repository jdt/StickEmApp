using System.Collections.ObjectModel;
using System.Windows.Input;
using StickEmApp.Dal;
using StickEmApp.Windows.View;

namespace StickEmApp.Windows.ViewModel
{
    public class VendorListViewModel : ViewModelBase
    {
        private readonly IViewModelFactory _viewModelFactory;
        private readonly IViewManager _viewManager;
        private readonly IVendorRepository _vendorRepository;
        private ObservableCollection<VendorListItemViewModel> _vendorList;

        public VendorListViewModel(IViewModelFactory viewModelFactory, IViewManager viewManager, IVendorRepository vendorRepository)
        {
            _viewModelFactory = viewModelFactory;
            _viewManager = viewManager;
            _vendorRepository = vendorRepository;

            LoadData();
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

        private void LoadData()
        {
            using (new UnitOfWork())
            {
                var vendorList = new ObservableCollection<VendorListItemViewModel>();
                foreach (var vendor in _vendorRepository.SelectVendors())
                {
                    vendorList.Add(_viewModelFactory.VendorListItemViewModel(vendor));
                }
                VendorList = vendorList;
            }
        }

        public ICommand AddVendorCommand { get { return new Command(i => AddVendor()); } }

        private void AddVendor()
        {
            var view = _viewManager.VendorView();
            view.ViewClosed += LoadData;
            view.Display();
        }
    }
}