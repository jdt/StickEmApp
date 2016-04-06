using System;

namespace StickEmApp.Entities
{
    public class StickerSalesPeriod
    {
        public virtual Guid Id { get; set; }
        public virtual int NumberOfStickersToSell { get; set; }
    }
}