using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using StickEmApp.Dal;
using StickEmApp.Entities;
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

        private bool _canVendorBeEdited;

        private DelegateCommand _addCommand;
        private DelegateCommand<VendorListItem> _editCommand;
        private DelegateCommand<VendorListItem> _removeCommand;

        private ObservableCollection<VendorListItem> _vendorList;

        [ImportingConstructor]
        public VendorListViewModel(IVendorRepository vendorRepository, IVendorListItemBuilder listItemBuilder, IRegionManager regionManager, IEventBus eventBus)
        {
            _vendorRepository = vendorRepository;
            _listItemBuilder = listItemBuilder;
            _regionManager = regionManager;
            _eventBus = eventBus;

            _canVendorBeEdited = true;

            _eventBus.On<VendorUpdatedEvent, Guid>(VendorChanged);
            _eventBus.On<VendorRemovedEvent, Guid>(VendorChanged);
        }

        private void VendorChanged(Guid id)
        {
            LoadData(); 
            AllowVendorEditing(true);
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

        public DelegateCommand AddVendorCommand
        {
            get
            {
                if (_addCommand == null)
                    _addCommand = new DelegateCommand(AddVendor, CanVendorBeEdited);

                return _addCommand;
            }
        }

        public DelegateCommand<VendorListItem> EditVendorCommand
        {
            get
            {
                if(_editCommand == null)
                    _editCommand = new DelegateCommand<VendorListItem>(EditVendor, CanVendorBeEdited);

                return _editCommand;
            }
        }
        
        public DelegateCommand<VendorListItem> RemoveVendorCommand
        {
            get
            {
                if(_removeCommand == null)
                    _removeCommand = new DelegateCommand<VendorListItem>(RemoveVendor, CanVendorBeEdited);

                return _removeCommand;
            }
        }

        private void AddVendor()
        {
            _regionManager.RequestNavigate(RegionNames.EditVendorRegion, new Uri("VendorDetailView", UriKind.Relative));
            AllowVendorEditing(false);
        }

        private void EditVendor(VendorListItem item)
        {
            _regionManager.RequestNavigate(RegionNames.EditVendorRegion, new Uri(string.Format("VendorDetailView?vendorId={0}", item.Id), UriKind.Relative));
            AllowVendorEditing(false);
        }

        private void RemoveVendor(VendorListItem item)
        {
            Vendor vendorToRemove;
            using (new UnitOfWork())
            {
                vendorToRemove = _vendorRepository.Get(item.Id);
                vendorToRemove.Remove();

                _vendorRepository.Save(vendorToRemove);
            }

            _eventBus.Publish<VendorRemovedEvent, Guid>(vendorToRemove.Id);
        }

        private bool CanVendorBeEdited()
        {
            return _canVendorBeEdited;
        }

        private bool CanVendorBeEdited(VendorListItem item)
        {
            return _canVendorBeEdited;
        }

        private void AllowVendorEditing(bool allowEditing)
        {
            _canVendorBeEdited = allowEditing;
            AddVendorCommand.RaiseCanExecuteChanged();
            EditVendorCommand.RaiseCanExecuteChanged();
            RemoveVendorCommand.RaiseCanExecuteChanged();
        }
    }
}