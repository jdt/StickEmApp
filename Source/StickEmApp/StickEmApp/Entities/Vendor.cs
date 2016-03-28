using System;

namespace StickEmApp.Entities
{
    public class Vendor
    {
        public Vendor()
        {
            Id = Guid.NewGuid();
            Status = VendorStatus.Working;
        }

        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual VendorStatus Status { get; set; }

        public virtual void Remove()
        {
            Status = VendorStatus.Removed;
        }
    }
}
