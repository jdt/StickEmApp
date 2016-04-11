using System;
using System.Collections.Generic;

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
                NumberOfStickersSold = 0,
                NumberOfStickersRemaining = NumberOfStickersToSell
            };
            
            foreach (var vendor in vendors)
            {
                var vendorResult = vendor.CalculateSalesResult();

                if (vendor.Status == VendorStatus.Finished)
                {
                    status.NumberOfStickersSold += vendorResult.NumberOfStickersSold;
                    status.NumberOfStickersRemaining -= vendorResult.NumberOfStickersSold;
                }
                else
                {
                    status.NumberOfStickersWithVendors += vendorResult.NumberOfStickersReceived;
                    status.NumberOfStickersRemaining -= vendorResult.NumberOfStickersReceived;
                }
            }

            status.SalesTotal = status.NumberOfStickersSold * Sticker.Price;

            return status;
        }
    }
}