﻿namespace StickEmApp.Entities
{
    public class SalesPeriodStatus
    {
        public int NumberOfStickersToSell { get; set; }
        public int NumberOfStickersSold { get; set; }
        public int NumberOfStickersWithVendors { get; set; }
        public int NumberOfStickersRemaining { get; set; }
        public Money SalesTotal { get; set; }
    }
}