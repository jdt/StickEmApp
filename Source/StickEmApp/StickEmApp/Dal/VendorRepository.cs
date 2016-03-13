using StickEmApp.Entities;
using System;
using System.Collections.Generic;

namespace StickEmApp.Dal
{
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

        public IEnumerable<Vendor> SelectVendors()
        {
            return UnitOfWorkManager.Session.QueryOver<Vendor>().List();
        }
    }
}
