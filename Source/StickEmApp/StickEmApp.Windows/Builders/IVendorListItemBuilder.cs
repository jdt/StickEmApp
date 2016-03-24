using System.Collections.Generic;
using StickEmApp.Entities;
using StickEmApp.Windows.ViewModel;

namespace StickEmApp.Windows.Builders
{
    public interface IVendorListItemBuilder
    {
        IReadOnlyCollection<VendorListItem> BuildFrom(IReadOnlyCollection<Vendor> vendorList);
    }
}