using System;
using System.Collections.Generic;
using StickEmApp.Entities;

namespace StickEmApp.Dal
{
    public interface IVendorRepository
    {
        IEnumerable<Vendor> SelectVendors();
        Vendor Get(Guid id);
        void Save(Vendor obj);
    }
}