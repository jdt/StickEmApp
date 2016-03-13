using FluentNHibernate.Mapping;
using StickEmApp.Entities;

namespace StickEmApp.Dal.Mapping
{
    public class VendorMapping : ClassMap<Vendor>
    {
        public VendorMapping()
        {
            Id(x => x.Id);
            Map(x => x.Name);
        }
    }
}