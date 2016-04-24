using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using Prism.Commands;
using Prism.Mvvm;
using StickEmApp.Dal;
using StickEmApp.Entities;
using StickEmApp.Service;
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
        private readonly IWindowManager _windowManager;
        private readonly IEventBus _eventBus;
        private readonly IExcelExporter _exporter;
        private readonly IResourceManager _resourceManager;

        private bool _canVendorBeEdited;

        private DelegateCommand _addCommand;
        private DelegateCommand _editStickerSalesPeriod;
        private DelegateCommand<VendorListItem> _editCommand;
        private DelegateCommand<VendorListItem> _removeCommand;
        private DelegateCommand _exportToExcelCommand;

        private ObservableCollection<VendorListItem> _vendorList;
        private bool _showFinishedVendors;

        [ImportingConstructor]
        public VendorListViewModel(IVendorRepository vendorRepository, IVendorListItemBuilder listItemBuilder, IWindowManager windowManager, IEventBus eventBus, IExcelExporter exporter, IResourceManager resourceManager)
        {
            _vendorRepository = vendorRepository;
            _listItemBuilder = listItemBuilder;
            _windowManager = windowManager;
            _eventBus = eventBus;
            _exporter = exporter;
            _resourceManager = resourceManager;

            _canVendorBeEdited = true;

            ShowFinishedVendors = false;

            _eventBus.On<VendorChangedEvent, Guid>(VendorChanged);
            _eventBus.On<StickerSalesPeriodChangedEvent, Guid>(VendorChanged);
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

        public bool ShowFinishedVendors
        {
            get
            {
                return _showFinishedVendors;
            }
            set
            {
                _showFinishedVendors = value;
                OnPropertyChanged();
                LoadData();
            }
        }

        private void LoadData()
        {
            using (new UnitOfWork())
            {
                var items = _listItemBuilder.BuildFrom(_vendorRepository.SelectVendors(ShowFinishedVendors));
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

        public DelegateCommand EditStickerSalesPeriodCommand
        {
            get
            {
                if(_editStickerSalesPeriod == null)
                    _editStickerSalesPeriod = new DelegateCommand(EditStickerSalesPeriod, CanVendorBeEdited);

                return _editStickerSalesPeriod;
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

        public DelegateCommand ExportToExcelCommand
        {
            get
            {
                if(_exportToExcelCommand == null)
                    _exportToExcelCommand = new DelegateCommand(ExportToExcel, CanVendorBeEdited);

                return _exportToExcelCommand;
            }
        }

        private void AddVendor()
        {
            _windowManager.DisplayAddVendor();
            AllowVendorEditing(false);
        }

        private void EditStickerSalesPeriod()
        {
            _windowManager.DisplayEditStickerSalesPeriod();
            AllowVendorEditing(false);
        }

        private void EditVendor(VendorListItem item)
        {
            _windowManager.DisplayEditVendor(item.Id);
            AllowVendorEditing(false);
        }

        private void RemoveVendor(VendorListItem item)
        {
            var confirm = _windowManager.IsConfirmation(string.Format(_resourceManager.GetString("AreYouSureYouWantToRemoveVendorWithName"), item.Name));
            if (!confirm)
                return;

            Vendor vendorToRemove;
            using (new UnitOfWork())
            {
                vendorToRemove = _vendorRepository.Get(item.Id);
                vendorToRemove.Remove();

                _vendorRepository.Save(vendorToRemove);
            }

            _eventBus.Publish<VendorChangedEvent, Guid>(vendorToRemove.Id);
        }

        private void ExportToExcel()
        {
            var target = _windowManager.DisplayFileSelection();
            if (target == null)
                return;

            using (new UnitOfWork())
            {
                var vendors = _vendorRepository.SelectVendors(ShowFinishedVendors);
                _exporter.Export(vendors, target);

                _windowManager.DisplayInformation(string.Format(_resourceManager.GetString("DataExportedToExcelFileAt"), target));
            }
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
            EditStickerSalesPeriodCommand.RaiseCanExecuteChanged();

            if (allowEditing)
            {
                _windowManager.DisplaySummary();
            }
        }
    }
}