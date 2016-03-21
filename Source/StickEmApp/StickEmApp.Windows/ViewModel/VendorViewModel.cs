using System.Windows.Input;
using StickEmApp.Dal;
using StickEmApp.Entities;

namespace StickEmApp.Windows.ViewModel
{
    public class VendorViewModel : ViewModelBase
    {
        private readonly IVendorRepository _vendorRepository;
        private string _name;

        public VendorViewModel(IVendorRepository vendorRepository)
        {
            _vendorRepository = vendorRepository;
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
        }
    }
}