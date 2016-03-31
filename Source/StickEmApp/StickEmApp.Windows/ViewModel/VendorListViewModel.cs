using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using StickEmApp.Dal;
using StickEmApp.Windows.Builders;
using StickEmApp.Windows.Infrastructure;
using StickEmApp.Windows.Infrastructure.Events;

namespace StickEmApp.Windows.ViewModel
{
    [Export(typeof(VendorListViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class VendorListViewModel : BindableBase
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly IVendorListItemBuilder _listItemBuilder;
        private readonly IRegionManager _regionManager;
        private readonly IEventBus _eventBus;

        private ObservableCollection<VendorListItem> _vendorList;

        [ImportingConstructor]
        public VendorListViewModel(IVendorRepository vendorRepository, IVendorListItemBuilder listItemBuilder, IRegionManager regionManager, IEventBus eventBus)
        {
            _vendorRepository = vendorRepository;
            _listItemBuilder = listItemBuilder;
            _regionManager = regionManager;
            _eventBus = eventBus;

            _eventBus.On<VendorUpdatedEvent, Guid>(VendorUpdated);
        }

        private void VendorUpdated(Guid id)
        {
            LoadData();
        }
        
        public ObservableCollection<VendorListItem> VendorList
        {
            get
            {
                if(_vendorList == null)
                    LoadData();

                return _vendorList;
            }
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
                var items = _listItemBuilder.BuildFrom(_vendorRepository.SelectVendors());
                VendorList = new ObservableCollection<VendorListItem>(items);
            }
        }

        public ICommand AddVendorCommand { get { return new DelegateCommand(AddVendor); } }
        public ICommand EditVendorCommand { get { return new DelegateCommand<VendorListItem>(EditVendor); } }
        public ICommand RemoveVendorCommand { get { return new DelegateCommand<VendorListItem>(RemoveVendor); } }

        private void AddVendor()
        {
            _regionManager.RequestNavigate(RegionNames.EditVendorRegion, new Uri("VendorDetailView", UriKind.Relative));
        }

        private void EditVendor(VendorListItem item)
        {
        }

        private void RemoveVendor(VendorListItem item)
        {
            using (new UnitOfWork())
            {
                var vendor = _vendorRepository.Get(item.Id);
                vendor.Remove();

                _vendorRepository.Save(vendor);

                _eventBus.Publish<VendorRemovedEvent, Guid>(vendor.Id);
            }
        }

    }
}