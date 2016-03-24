using System;

namespace StickEmApp.Entities
{
    public class Vendor
    {
        public Vendor()
        {
            Id = Guid.NewGuid();
        }

        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
    }
}
