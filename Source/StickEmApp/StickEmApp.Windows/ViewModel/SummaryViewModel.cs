using System.ComponentModel.Composition;
using Prism.Mvvm;
using StickEmApp.Dal;

namespace StickEmApp.Windows.ViewModel
{
    [Export(typeof(SummaryViewModel))]
    public class SummaryViewModel : BindableBase
    {
        private int _numberOfStickersToSell;
        private int _numberOfStickersSold;
        private decimal _salesTotal;

        [ImportingConstructor]
        public SummaryViewModel(IStickerSalesPeriodRepository stickerSalesPeriodRepository, IVendorRepository vendorRepository)
        {
            using (new UnitOfWork())
            {
                var period = stickerSalesPeriodRepository.Get();

                var vendors = vendorRepository.SelectVendors();
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