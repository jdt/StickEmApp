using System.ComponentModel.Composition;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using StickEmApp.Dal;
using StickEmApp.Entities;
using StickEmApp.Windows.Infrastructure.Events;

namespace StickEmApp.Windows.ViewModel
{
    [Export(typeof(VendorDetailViewModel))]
    public class VendorDetailViewModel : BindableBase
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly IEventAggregator _eventAggregator;
        private string _name;

        [ImportingConstructor]
        public VendorDetailViewModel(IVendorRepository vendorRepository, IEventAggregator eventAggregator)
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
                OnPropertyChanged();
            }
        }

        public ICommand SaveChangesCommand { get { return new DelegateCommand(SaveChanges); } }

        private void SaveChanges()
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