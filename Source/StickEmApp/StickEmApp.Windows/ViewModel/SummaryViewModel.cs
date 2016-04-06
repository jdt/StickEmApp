using System.ComponentModel.Composition;
using Prism.Mvvm;
using StickEmApp.Dal;

namespace StickEmApp.Windows.ViewModel
{
    [Export(typeof(SummaryViewModel))]
    public class SummaryViewModel : BindableBase
    {
        private int _numberOfStickersToSell;

        [ImportingConstructor]
        public SummaryViewModel(IStickerSalesPeriodRepository stickerSalesPeriodRepository)
        {
            using (new UnitOfWork())
            {
                NumberOfStickersToSell = stickerSalesPeriodRepository.Get().NumberOfStickersToSell;
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
    }
}