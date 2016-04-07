using System;
using System.ComponentModel.Composition;
using Prism.Mvvm;
using StickEmApp.Dal;
using StickEmApp.Windows.Infrastructure.Events;

namespace StickEmApp.Windows.ViewModel
{
    [Export(typeof(SummaryViewModel))]
    public class SummaryViewModel : BindableBase
    {
        private readonly IStickerSalesPeriodRepository _stickerSalesPeriodRepository;
        private readonly IVendorRepository _vendorRepository;

        private int _numberOfStickersToSell;
        private int _numberOfStickersSold;
        private decimal _salesTotal;

        [ImportingConstructor]
        public SummaryViewModel(IStickerSalesPeriodRepository stickerSalesPeriodRepository, IVendorRepository vendorRepository, IEventBus eventBus)
        {
            _stickerSalesPeriodRepository = stickerSalesPeriodRepository;
            _vendorRepository = vendorRepository;

            eventBus.On<VendorChangedEvent, Guid>(VendorChanged);
            eventBus.On<StickerSalesPeriodChangedEvent, Guid>(VendorChanged);

            LoadData();
        }

        public void VendorChanged(Guid vendorId)
        {
            LoadData();
        }

        private void LoadData()
        {
            using (new UnitOfWork())
            {
                var period = _stickerSalesPeriodRepository.Get();

                var vendors = _vendorRepository.SelectVendors();
                var salesStatus = period.CalculateStatus(vendors);

                NumberOfStickersToSell = salesStatus.NumberOfStickersToSell;
                NumberOfStickersSold = salesStatus.NumberOfStickersSold;
                SalesTotal = salesStatus.SalesTotal.Value;
            }
        }

        public int NumberOfStickersToSell
        {
            get
            {
                return _numberOfStickersToSell;
            }
            set
            {
                _numberOfStickersToSell = value;
                OnPropertyChanged();
            }
        }

        public int NumberOfStickersSold
        {
            get
            {
                return _numberOfStickersSold;
            }
            set
            {
                _numberOfStickersSold = value;
                OnPropertyChanged();
            }
        }

        public decimal SalesTotal
        {
            get
            {
                return _salesTotal;
            }
            set
            {
                _salesTotal = value;
                OnPropertyChanged();
            }
        }
    }
}