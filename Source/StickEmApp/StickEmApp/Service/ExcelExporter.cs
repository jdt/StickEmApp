using System.Collections.Generic;
using System.ComponentModel.Composition;
using StickEmApp.Entities;

namespace StickEmApp.Service
{
    [Export(typeof(IExcelExporter))]
    public class ExcelExporter : IExcelExporter
    {
        public void Export(IReadOnlyCollection<Vendor> vendors, string destination)
        {
            throw new System.NotImplementedException();
        }
    }
}