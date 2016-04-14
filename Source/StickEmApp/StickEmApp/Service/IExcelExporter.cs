using System.Collections.Generic;
using StickEmApp.Entities;

namespace StickEmApp.Service
{
    public interface IExcelExporter
    {
        void Export(IReadOnlyCollection<Vendor> vendors, string destination);
    }
}