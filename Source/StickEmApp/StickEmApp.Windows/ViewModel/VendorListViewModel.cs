using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows.Input;
using Prism.Events;
using Prism.Regions;
using StickEmApp.Dal;
using StickEmApp.Windows.Infrastructure;
using StickEmApp.Windows.Infrastructure.Events;

namespace StickEmApp.Windows.ViewModel
{
    [Export(typeof(VendorListViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class VendorListViewModel : ViewModelBase
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly IRegionManager _regionManager;
        private ObservableCollection<VendorListItemViewModel> _vendorList;

        [ImportingConstructor]
        public VendorListViewModel(IVendorRepository vendorRepository, IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _vendorRepository = vendorRepository;
            _regionManager = regionManager;

            LoadData();

            eventAggregator.GetEvent<VendorUpdatedEvent>().Subscribe(VendorUpdated, ThreadOption.UIThread);
        }

        private void VendorUpdated(Guid id)
        {
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
                    vendorList.Add(new VendorListItemViewModel(vendor.Name));
                }
                VendorList = vendorList;
            }
        }

        public ICommand AddVendorCommand { get { return new Command(i => AddVendor()); } }

        private void AddVendor()
        {
            _regionManager.RequestNavigate(RegionNames.EditVendorRegion, new Uri("VendorDetailView", UriKind.Relative));
        }
    }
}