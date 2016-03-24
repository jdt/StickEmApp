using FluentNHibernate.Mapping;
using StickEmApp.Entities;

namespace StickEmApp.Dal.Mapping
{
    public class VendorMapping : ClassMap<Vendor>
    {
        public VendorMapping()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.Name);
        }
    }
}