using System;
using Prism.Mvvm;
using StickEmApp.Entities;

namespace StickEmApp.Windows.ViewModel
{
    public class VendorListItem : BindableBase
    {
        public VendorListItem(Guid id, string name)
        {
            Id = id;
            Name = name;

            NumberOfStickersReceived = 0;
            AmountRequired = new Money(0);
            AmountReturned = new Money(0);
            Difference = new Money(0);
            Status = VendorStatus.Working;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public int NumberOfStickersReceived { get; set; }
        public Money AmountRequired { get; set; }
        public Money AmountReturned { get; set; }
        public Money Difference { get; set; }
        public VendorStatus Status { get; set; }
    }
}
