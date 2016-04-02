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
        private int _numberOfStickersReceived;
        private int _numberOfStickersReturned;
        private Money _changeReceived;
        private int  _fiveHundreds;
        private int  _twoHundreds;
        private int  _hundreds;
        private int  _fifties;
        private int  _twenties;
        private int  _tens;
        private int  _fives;
        private int  _twos;
        private int  _ones;
        private int  _fiftyCents;
        private int  _twentyCents;
        private int  _tenCents;
        private int  _fiveCents;
        private int  _twoCents;
        private int  _oneCents;

        [ImportingConstructor]
        public VendorDetailViewModel(IVendorRepository vendorRepository, IEventAggregator eventAggregator)
        {
            _vendorRepository = vendorRepository;
            _eventAggregator = eventAggregator;

            Vendor = new Vendor();
        }

        private Vendor Vendor { get; set; }

        public string Name { get { return _name; } set { _name = value; OnPropertyChanged(); } }
        public int NumberOfStickersReceived { get { return _numberOfStickersReceived; } set { _numberOfStickersReceived = value; OnPropertyChanged(); } }
        public int NumberOfStickersReturned { get { return _numberOfStickersReturned; } set { _numberOfStickersReturned = value; OnPropertyChanged(); } }
        public Money ChangeReceived { get { return _changeReceived; } set { _changeReceived = value; OnPropertyChanged(); } }
        public int FiveHundreds { get { return _fiveHundreds; } set { _fiveHundreds = value; OnPropertyChanged(); } }
        public int TwoHundreds { get { return _twoHundreds; } set { _twoHundreds = value; OnPropertyChanged(); } }
        public int Hundreds { get { return _hundreds; } set { _hundreds = value; OnPropertyChanged(); } }
        public int Fifties { get { return _fifties; } set { _fifties = value; OnPropertyChanged(); } }
        public int Twenties { get { return _twenties; } set { _twenties = value; OnPropertyChanged(); } }
        public int Tens { get { return _tens; } set { _tens = value; OnPropertyChanged(); } }
        public int Fives { get { return _fives; } set { _fives = value; OnPropertyChanged(); } }
        public int Twos { get { return _twos; } set { _twos = value; OnPropertyChanged(); } }
        public int Ones { get { return _ones; } set { _ones = value; OnPropertyChanged(); } }
        public int FiftyCents { get { return _fiftyCents; } set { _fiftyCents = value; OnPropertyChanged(); } }
        public int TwentyCents { get { return _twentyCents; } set { _twentyCents = value; OnPropertyChanged(); } }
        public int TenCents { get { return _tenCents; } set { _tenCents = value; OnPropertyChanged(); } }
        public int FiveCents { get { return _fiveCents; } set { _fiveCents = value; OnPropertyChanged(); } }
        public int TwoCents { get { return _twoCents; } set { _twoCents = value; OnPropertyChanged(); } }
        public int OneCents { get { return _oneCents; } set { _oneCents = value; OnPropertyChanged(); } }

        public ICommand SaveChangesCommand { get { return new DelegateCommand(SaveChanges); } }
        private void SaveChanges()
        {
            Vendor.Name = Name;
            Vendor.NumberOfStickersReceived = NumberOfStickersReceived;
            Vendor.NumberOfStickersReturned = NumberOfStickersReturned;
            Vendor.ChangeReceived = ChangeReceived;
            Vendor.AmountReturned.FiveHundreds = FiveHundreds;
            Vendor.AmountReturned.TwoHundreds = TwoHundreds;
            Vendor.AmountReturned.Hundreds = Hundreds;
            Vendor.AmountReturned.Fifties = Fifties;
            Vendor.AmountReturned.Twenties = Twenties;
            Vendor.AmountReturned.Tens = Tens;
            Vendor.AmountReturned.Fives = Fives;
            Vendor.AmountReturned.Twos = Twos;
            Vendor.AmountReturned.Ones = Ones;
            Vendor.AmountReturned.FiftyCents = FiftyCents;
            Vendor.AmountReturned.TwentyCents = TwentyCents;
            Vendor.AmountReturned.TenCents = TenCents;
            Vendor.AmountReturned.FiveCents = FiveCents;
            Vendor.AmountReturned.TwoCents = TwoCents;
            Vendor.AmountReturned.OneCents = OneCents;

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
            NumberOfStickersReceived = Vendor.NumberOfStickersReceived;
            NumberOfStickersReturned = Vendor.NumberOfStickersReturned;
            ChangeReceived = Vendor.ChangeReceived;
            FiveHundreds = Vendor.AmountReturned.FiveHundreds;
            TwoHundreds = Vendor.AmountReturned.TwoHundreds;
            Hundreds = Vendor.AmountReturned.Hundreds;
            Fifties = Vendor.AmountReturned.Fifties;
            Twenties = Vendor.AmountReturned.Twenties;
            Tens = Vendor.AmountReturned.Tens;
            Fives = Vendor.AmountReturned.Fives;
            Twos = Vendor.AmountReturned.Twos;
            Ones = Vendor.AmountReturned.Ones;
            FiftyCents = Vendor.AmountReturned.FiftyCents;
            TwentyCents = Vendor.AmountReturned.TwentyCents;
            TenCents = Vendor.AmountReturned.TenCents;
            FiveCents = Vendor.AmountReturned.FiveCents;
            TwoCents = Vendor.AmountReturned.TwoCents;
            OneCents = Vendor.AmountReturned.OneCents;
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