using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using StickEmApp.Dal;
using StickEmApp.Entities;
using StickEmApp.Windows.Infrastructure.Events;

namespace StickEmApp.Windows.ViewModel
{
    [Export(typeof(VendorDetailViewModel))]
    public class VendorDetailViewModel : BindableBase, INavigationAware
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly IEventAggregator _eventAggregator;
        private string _name;

        [ImportingConstructor]
        public VendorDetailViewModel(IVendorRepository vendorRepository, IEventAggregator eventAggregator)
        {
            _vendorRepository = vendorRepository;
            _eventAggregator = eventAggregator;

            Vendor = new Vendor();
        }

        private Vendor Vendor { get; set; }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveChangesCommand { get { return new DelegateCommand(SaveChanges); } }

        private void SaveChanges()
        {
            Vendor.Name = Name;

            using (new UnitOfWork())
            {
                _vendorRepository.Save(Vendor);
            }

            _eventAggregator.GetEvent<VendorUpdatedEvent>().Publish(Vendor.Id);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.Any())
            {
                using (new UnitOfWork())
                {
                    Vendor = _vendorRepository.Get(new Guid(navigationContext.Parameters["vendorId"].ToString()));
                }
            }
            else
            {
                Vendor = new Vendor();
            }
            
            Name = Vendor.Name;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
    }
}