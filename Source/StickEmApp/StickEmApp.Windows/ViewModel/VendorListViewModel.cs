using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using StickEmApp.Dal;
using StickEmApp.Windows.Infrastructure;
using StickEmApp.Windows.Infrastructure.Events;

namespace StickEmApp.Windows.ViewModel
{
    [Export(typeof(VendorListViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class VendorListViewModel : BindableBase
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly IRegionManager _regionManager;
        private ObservableCollection<VendorListItem> _vendorList;

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
        
        public ObservableCollection<VendorListItem> VendorList
        {
            get { return _vendorList; }
            set
            {
                _vendorList = value;
                OnPropertyChanged();
            }
        }

        private void LoadData()
        {
            using (new UnitOfWork())
            {
                var vendorList = new ObservableCollection<VendorListItem>();
                foreach (var vendor in _vendorRepository.SelectVendors())
                {
                    vendorList.Add(new VendorListItem(vendor.Name));
                }
                VendorList = vendorList;
            }
        }

        public ICommand AddVendorCommand { get { return new DelegateCommand(AddVendor); } }

        private void AddVendor()
        {
            _regionManager.RequestNavigate(RegionNames.EditVendorRegion, new Uri("VendorDetailView", UriKind.Relative));
        }
    }
}