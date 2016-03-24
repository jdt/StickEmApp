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

        private ObservableCollection<VendorListItem> _vendorList;

        [ImportingConstructor]
        public VendorListViewModel(IVendorRepository vendorRepository, IVendorListItemBuilder listItemBuilder, IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _vendorRepository = vendorRepository;
            _listItemBuilder = listItemBuilder;
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
                var items = _listItemBuilder.BuildFrom(_vendorRepository.SelectVendors());
                VendorList = new ObservableCollection<VendorListItem>(items);
            }
        }

        public ICommand AddVendorCommand { get { return new DelegateCommand(AddVendor); } }
        public ICommand EditVendorCommand { get { return new DelegateCommand<VendorListItem>(EditVendor); } }

        private void AddVendor()
        {
            _regionManager.RequestNavigate(RegionNames.EditVendorRegion, new Uri("VendorDetailView", UriKind.Relative));
        }

        private void EditVendor(VendorListItem item)
        {
        }
    }
}