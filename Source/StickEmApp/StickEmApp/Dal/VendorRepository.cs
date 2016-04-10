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

        public IReadOnlyCollection<Vendor> SelectVendors(bool includeFinished)
        {
            var query = UnitOfWorkManager.Session.QueryOver<Vendor>().Where(x => x.Status != VendorStatus.Removed);
            if (includeFinished == false)
            {
                query.And(x => x.Status != VendorStatus.Finished);
            }

            return query.List().ToArray();
        }
    }
}
