using System;
using System.Collections.Generic;
using System.Linq;

namespace StickEmApp.Entities
{
    public class StickerSalesPeriod
    {
        public virtual Guid Id { get; set; }
        public virtual int NumberOfStickersToSell { get; set; }

        public virtual SalesPeriodStatus CalculateStatus(IReadOnlyCollection<Vendor> vendors)
        {
            var status = new SalesPeriodStatus
            {
                NumberOfStickersToSell = NumberOfStickersToSell,
                NumberOfStickersSold = 0
            };
            
            foreach (var vendorResult in vendors.Select(vendor => vendor.CalculateSalesResult()))
            {
                status.NumberOfStickersSold += vendorResult.NumberOfStickersSold;
            }

            status.SalesTotal = status.NumberOfStickersSold * Sticker.Price;

            return status;
        }
    }
}