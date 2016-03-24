using StickEmApp.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace StickEmApp.Dal
{
    [Export(typeof(IVendorRepository))]
    public class VendorRepository : IVendorRepository
    {
        public Vendor Get(Guid id)
        {
            return UnitOfWorkManager.Session.Get<Vendor>(id);
        }

        public void Save(Vendor obj)
        {
            UnitOfWorkManager.Session.SaveOrUpdate(obj);
        }

        public IReadOnlyCollection<Vendor> SelectVendors()
        {
            return UnitOfWorkManager.Session.QueryOver<Vendor>().List().ToArray();
        }
    }
}
