using System.Collections.Generic;
using System.ComponentModel.Composition;
using StickEmApp.Entities;
using StickEmApp.Windows.ViewModel;

namespace StickEmApp.Windows.Builders
{
    [Export(typeof(IVendorListItemBuilder))]
    public class VendorListItemBuilder : IVendorListItemBuilder
    {
        public IReadOnlyCollection<VendorListItem> BuildFrom(IReadOnlyCollection<Vendor> vendorList)
        {
            var result = new List<VendorListItem>();
            foreach (var vendor in vendorList)
            {
                result.Add(new VendorListItem(vendor.Id, vendor.Name));
            }
            return result;
        }
    }
}