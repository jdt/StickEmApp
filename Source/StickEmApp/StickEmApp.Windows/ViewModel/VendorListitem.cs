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
            AmountRequired = 0;
            AmountReturned = 0;
            Difference = 0;
            Status = VendorStatus.Working;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public int NumberOfStickersReceived { get; set; }
        public decimal AmountRequired { get; set; }
        public decimal AmountReturned { get; set; }
        public decimal Difference { get; set; }
        public VendorStatus Status { get; set; }
    }
}
