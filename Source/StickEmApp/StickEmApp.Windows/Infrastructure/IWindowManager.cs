using System;

namespace StickEmApp.Windows.Infrastructure
{
    public interface IWindowManager
    {
        void DisplayAddVendor();
        void DisplayEditVendor(Guid vendorId);
    }
}