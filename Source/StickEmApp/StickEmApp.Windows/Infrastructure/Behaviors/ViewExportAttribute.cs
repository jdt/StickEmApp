using System;
using System.ComponentModel.Composition;

namespace StickEmApp.Windows.Infrastructure.Behaviors
{
    [AttributeUsage(AttributeTargets.Class)]
    [MetadataAttribute]
    public sealed class ViewExportAttribute : ExportAttribute, IViewRegionRegistration
    {
        public ViewExportAttribute()
            : base(typeof(object))
        { }

        public ViewExportAttribute(string viewName)
            : base(viewName, typeof(object))
        { }

        public string ViewName { get { return ContractName; } }

        public string RegionName { get; set; }
    }
}