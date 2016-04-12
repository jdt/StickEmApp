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
                result.Add(new VendorListItem(vendor.Id, vendor.Name)
                {
                    NumberOfStickersReceived = vendor.CalculateSalesResult().NumberOfStickersReceived,
                    AmountReturned = vendor.CalculateTotalAmountReturned(),
                    AmountRequired = vendor.CalculateTotalAmountRequired(),
                    Difference = vendor.CalculateSalesResult().Difference,
                    Status = vendor.Status,
                    StartedAt = vendor.StartedAt,
                    FinishedAt = vendor.FinishedAt
                });
            }
            return result;
        }
    }
}