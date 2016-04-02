using FluentNHibernate.Mapping;
using StickEmApp.Dal.UserTypes;
using StickEmApp.Entities;

namespace StickEmApp.Dal.Mapping
{
    public class VendorMapping : ClassMap<Vendor>
    {
        public VendorMapping()
        {
            Id(x => x.Id).GeneratedBy.Assigned();

            Map(x => x.Name);
            Map(x => x.Status);
            Map(x => x.ChangeReceived).CustomType<MoneyUserType>();
            Map(x => x.NumberOfStickersReceived);
            Map(x => x.NumberOfStickersReturned);

            Component(x => x.AmountReturned, amount =>
            {
                amount.Map(x => x.FiveHundreds);
                amount.Map(x => x.TwoHundreds);
                amount.Map(x => x.Hundreds);
                amount.Map(x => x.Fifties);
                amount.Map(x => x.Twenties);
                amount.Map(x => x.Tens);
                amount.Map(x => x.Fives);
                amount.Map(x => x.Twos);
                amount.Map(x => x.Ones);
                amount.Map(x => x.FiftyCents);
                amount.Map(x => x.TwentyCents);
                amount.Map(x => x.TenCents);
                amount.Map(x => x.FiveCents);
                amount.Map(x => x.TwoCents);
                amount.Map(x => x.OneCents);
            });
        }
    }
}