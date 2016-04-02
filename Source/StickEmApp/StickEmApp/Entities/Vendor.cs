using System;

namespace StickEmApp.Entities
{
    public class Vendor
    {
        public Vendor()
        {
            Id = Guid.NewGuid();

            NumberOfStickersReceived = 0;
            NumberOfStickersReturned = 0;
            ChangeReceived = new Money(0);
            AmountReturned = new AmountReturned();

            Status = VendorStatus.Working;
        }

        public virtual Guid Id { get; set; }

        public virtual string Name { get; set; }
        public virtual int NumberOfStickersReceived { get; set; }
        public virtual int NumberOfStickersReturned { get; set; }
        public virtual Money ChangeReceived { get; set; }
        public virtual AmountReturned AmountReturned { get; set; }
        
        public virtual VendorStatus Status { get; set; }

        public virtual void Remove()
        {
            Status = VendorStatus.Removed;
        }
    }
}
