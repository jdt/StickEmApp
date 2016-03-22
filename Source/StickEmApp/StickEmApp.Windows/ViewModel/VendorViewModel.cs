using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Input;
using Prism.Events;
using Prism.Regions;
using StickEmApp.Dal;
using StickEmApp.Entities;
using StickEmApp.Windows.Infrastructure;
using StickEmApp.Windows.Infrastructure.Events;

namespace StickEmApp.Windows.ViewModel
{
    [Export(typeof(VendorViewModel))]
    public class VendorViewModel : ViewModelBase
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly IEventAggregator _eventAggregator;
        private string _name;

        [ImportingConstructor]
        public VendorViewModel(IVendorRepository vendorRepository, IEventAggregator eventAggregator)
        {
            _vendorRepository = vendorRepository;
            _eventAggregator = eventAggregator;
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public ICommand SaveChangesCommand { get { return new Command(SaveChanges); } }

        private void SaveChanges(object view)
        {
            var vendor = new Vendor
            {
                Name = Name
            };

            using (new UnitOfWork())
            {
                _vendorRepository.Save(vendor);
            }

            _eventAggregator.GetEvent<VendorUpdatedEvent>().Publish(vendor.Id);
        }
    }
}