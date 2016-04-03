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

        private bool _suspendUpdatesToCalculated;

        private string _name;
        private int _numberOfStickersReceived;
        private int _numberOfStickersReturned;
        private decimal _changeReceived;
        private int _fiveHundreds;
        private int _twoHundreds;
        private int _hundreds;
        private int _fifties;
        private int _twenties;
        private int _tens;
        private int _fives;
        private int _twos;
        private int _ones;
        private int _fiftyCents;
        private int _twentyCents;
        private int _tenCents;
        private int _fiveCents;
        private int _twoCents;
        private int _oneCents;

        private decimal _totalAmountReturned;
        private decimal _totalAmountRequired;
        private decimal _totalDifference;

        [ImportingConstructor]
        public VendorDetailViewModel(IVendorRepository vendorRepository, IEventAggregator eventAggregator)
        {
            _vendorRepository = vendorRepository;
            _eventAggregator = eventAggregator;

            _suspendUpdatesToCalculated = false;

            Vendor = new Vendor();
        }

        private Vendor Vendor { get; set; }

        public string Name { get { return _name; } set { _name = value; OnPropertyChanged(); } }

        public decimal TotalAmountReturned { get { return _totalAmountReturned; } set { _totalAmountReturned = value; OnPropertyChanged(); } }
        public decimal TotalAmountRequired { get { return _totalAmountRequired; } set { _totalAmountRequired = value; OnPropertyChanged(); } }
        public decimal TotalDifference { get { return _totalDifference; } set { _totalDifference = value; OnPropertyChanged(); } }

        public int NumberOfStickersReceived { get { return _numberOfStickersReceived; } set { _numberOfStickersReceived = value; OnPropertyChanged(); RecalculateTotals(); } }
        public int NumberOfStickersReturned { get { return _numberOfStickersReturned; } set { _numberOfStickersReturned = value; OnPropertyChanged(); RecalculateTotals(); } }
        public decimal ChangeReceived { get { return _changeReceived; } set { _changeReceived = value; OnPropertyChanged(); RecalculateTotals(); } }

        public int FiveHundreds { get { return _fiveHundreds; } set { _fiveHundreds = value; OnPropertyChanged(); RecalculateTotals(); } }
        public int TwoHundreds { get { return _twoHundreds; } set { _twoHundreds = value; OnPropertyChanged(); RecalculateTotals(); } }
        public int Hundreds { get { return _hundreds; } set { _hundreds = value; OnPropertyChanged(); RecalculateTotals(); } }
        public int Fifties { get { return _fifties; } set { _fifties = value; OnPropertyChanged(); RecalculateTotals(); } }
        public int Twenties { get { return _twenties; } set { _twenties = value; OnPropertyChanged(); RecalculateTotals(); } }
        public int Tens { get { return _tens; } set { _tens = value; OnPropertyChanged(); RecalculateTotals(); } }
        public int Fives { get { return _fives; } set { _fives = value; OnPropertyChanged(); RecalculateTotals(); } }
        public int Twos { get { return _twos; } set { _twos = value; OnPropertyChanged(); RecalculateTotals(); } }
        public int Ones { get { return _ones; } set { _ones = value; OnPropertyChanged(); RecalculateTotals(); } }
        public int FiftyCents { get { return _fiftyCents; } set { _fiftyCents = value; OnPropertyChanged(); RecalculateTotals(); } }
        public int TwentyCents { get { return _twentyCents; } set { _twentyCents = value; OnPropertyChanged(); RecalculateTotals(); } }
        public int TenCents { get { return _tenCents; } set { _tenCents = value; OnPropertyChanged(); RecalculateTotals(); } }
        public int FiveCents { get { return _fiveCents; } set { _fiveCents = value; OnPropertyChanged(); RecalculateTotals(); } }
        public int TwoCents { get { return _twoCents; } set { _twoCents = value; OnPropertyChanged(); RecalculateTotals(); } }
        public int OneCents { get { return _oneCents; } set { _oneCents = value; OnPropertyChanged(); RecalculateTotals(); } }

        public ICommand SaveChangesCommand { get { return new DelegateCommand(SaveChanges); } }
        private void SaveChanges()
        {
            UpdateVendor();
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

            _suspendUpdatesToCalculated = true;

            Name = Vendor.Name;
            NumberOfStickersReceived = Vendor.NumberOfStickersReceived;
            NumberOfStickersReturned = Vendor.NumberOfStickersReturned;
            ChangeReceived = Vendor.ChangeReceived.Value;
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

            _suspendUpdatesToCalculated = false;

            RecalculateTotals();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        private void RecalculateTotals()
        {
            if (_suspendUpdatesToCalculated)
                return;

            UpdateVendor();

            TotalAmountRequired = Vendor.CalculateTotalAmountRequired().Value;
            TotalAmountReturned = Vendor.CalculateTotalAmountReturned().Value;
            TotalDifference = Vendor.CalculateSalesResult().Difference.Value;
        }

        private void UpdateVendor()
        {
            Vendor.Name = Name;
            Vendor.NumberOfStickersReceived = NumberOfStickersReceived;
            Vendor.NumberOfStickersReturned = NumberOfStickersReturned;
            Vendor.ChangeReceived = new Money(ChangeReceived);
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
        }
    }
}