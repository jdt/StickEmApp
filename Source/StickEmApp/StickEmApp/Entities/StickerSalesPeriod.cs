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
                NumberOfStickersSold = 0
            };
            
            foreach (var vendor in vendors)
            {
                var vendorResult = vendor.CalculateSalesResult();

                status.NumberOfStickersSold += vendorResult.NumberOfStickersSold;
            }

            return status;
        }
    }
}