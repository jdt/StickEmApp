using System.ComponentModel.Composition;
using StickEmApp.Entities;

namespace StickEmApp.Dal
{
    [Export(typeof(IStickerSalesPeriodRepository))]
    public class StickerSalesPeriodRepository : IStickerSalesPeriodRepository
    {
        public StickerSalesPeriod Get()
        {
            return UnitOfWorkManager.Session.QueryOver<StickerSalesPeriod>().SingleOrDefault();
        }

        public void Save(StickerSalesPeriod period)
        {
            UnitOfWorkManager.Session.Save(period);
        }
    }
}