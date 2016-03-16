using System.IO;
using System.Reflection;
using StickEmApp.Dal;
using StickEmApp.Entities;

namespace StickEmApp.Sandbox
{
    public class Program
    {
        static void Main(string[] args)
        {
            var dbFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "data.db");
            UnitOfWorkManager.Initialize(dbFile, DatabaseFileMode.Overwrite);

            var vendorRepository = new VendorRepository();

            using (new UnitOfWork())
            {
                var vendor1 = new Vendor {Name = "Vendor 1"};
                var vendor2 = new Vendor {Name = "Vendor 2"};

                vendorRepository.Save(vendor1);
                vendorRepository.Save(vendor2);
            }
        }
    }
}
