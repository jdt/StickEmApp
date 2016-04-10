using System;
using System.Collections.Generic;
using StickEmApp.Entities;

namespace StickEmApp.Dal
{
    public interface IVendorRepository
    {
        IReadOnlyCollection<Vendor> SelectVendors(bool includeFinished);
        Vendor Get(Guid id);
        void Save(Vendor obj);
    }
}