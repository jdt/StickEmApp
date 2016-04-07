using System;
using System.ComponentModel.Composition;
using Prism.Commands;
using Prism.Mvvm;
using StickEmApp.Dal;
using StickEmApp.Entities;
using StickEmApp.Windows.Infrastructure.Events;

namespace StickEmApp.Windows.ViewModel
{
    [Export]
    public class StickerSalesPeriodDetailViewModel : BindableBase
    {
        private readonly IStickerSalesPeriodRepository _stickerSalesPeriodRepository;
        private readonly IEventBus _eventBus;
        private int _numberOfStickersToSell;

        private DelegateCommand _saveChangesCommand;

        [ImportingConstructor]
        public StickerSalesPeriodDetailViewModel(IStickerSalesPeriodRepository stickerSalesPeriodRepository, IEventBus eventBus)
        {
            _stickerSalesPeriodRepository = stickerSalesPeriodRepository;
            _eventBus = eventBus;

            using (new UnitOfWork())
            {
                NumberOfStickersToSell = _stickerSalesPeriodRepository.Get().NumberOfStickersToSell;
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

        public DelegateCommand SaveChangesCommand
        {
            get
            {
                if(_saveChangesCommand == null)
                    _saveChangesCommand = new DelegateCommand(SaveChanges);

                return _saveChangesCommand;
            }
        }

        private void SaveChanges()
        {
            StickerSalesPeriod period;
            using (new UnitOfWork())
            {
                period = _stickerSalesPeriodRepository.Get();
                period.NumberOfStickersToSell = NumberOfStickersToSell;
                _stickerSalesPeriodRepository.Save(period);
            }

            _eventBus.Publish<StickerSalesPeriodChangedEvent, Guid>(period.Id);
        }
    }
}