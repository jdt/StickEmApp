using System;
using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;
using Prism.Regions;

namespace StickEmApp.Windows.Infrastructure.Behaviors
{
    [Export(typeof(AutoPopulateExportedViewsBehavior))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AutoPopulateExportedViewsBehavior : RegionBehavior, IPartImportsSatisfiedNotification
    {
        protected override void OnAttach()
        {
            AddRegisteredViews();
        }

        public void OnImportsSatisfied()
        {
            AddRegisteredViews();
        }

        private void AddRegisteredViews()
        {
            if (Region != null)
            {
                foreach (var viewEntry in RegisteredViews)
                {
                    if (viewEntry.Metadata.RegionName == Region.Name)
                    {
                        var view = viewEntry.Value;

                        if (!Region.Views.Contains(view))
                        {
                            Region.Add(view);
                        }
                    }
                }
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Justification = "MEF injected values"), ImportMany(AllowRecomposition = true)]
        public Lazy<object, IViewRegionRegistration>[] RegisteredViews { get; set; }
    }
}