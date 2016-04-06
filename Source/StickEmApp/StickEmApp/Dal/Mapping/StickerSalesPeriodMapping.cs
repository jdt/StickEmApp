using FluentNHibernate.Mapping;
using StickEmApp.Entities;

namespace StickEmApp.Dal.Mapping
{
    public class StickerSalesPeriodMapping : ClassMap<StickerSalesPeriod>
    {
        public StickerSalesPeriodMapping()
        {
            Id(x => x.Id);
            Map(x => x.NumberOfStickersToSell);
        } 
    }
}