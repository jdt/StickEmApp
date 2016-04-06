using StickEmApp.Entities;

namespace StickEmApp.Dal
{
    public interface IStickerSalesPeriodRepository
    {
        StickerSalesPeriod Get();
        void Save(StickerSalesPeriod period);
    }
}