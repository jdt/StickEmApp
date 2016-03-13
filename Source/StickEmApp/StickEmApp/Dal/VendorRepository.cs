using StickEmApp.Entities;
using System;
namespace StickEmApp.Dal
{
    public class VendorRepository
    {
        public Vendor Get(Guid id)
        {
            return UnitOfWorkManager.Session.Get<Vendor>(id);
        }

        public void Save(Vendor obj)
        {
            UnitOfWorkManager.Session.SaveOrUpdate(obj);
        }
    }
}
