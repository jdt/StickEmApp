﻿using System;
using System.ComponentModel.Composition;
using Prism.Regions;

namespace StickEmApp.Windows.Infrastructure
{
    [Export(typeof(IWindowManager))]
    public class WindowManager : IWindowManager
    {
        private readonly IRegionManager _regionManager;

        [ImportingConstructor]
        public WindowManager(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void DisplayAddVendor()
        {
            _regionManager.RequestNavigate(RegionNames.EditVendorRegion, new Uri("VendorDetailView", UriKind.Relative));
        }

        public void DisplayEditVendor(Guid vendorId)
        {
            var parameters = new NavigationParameters
            {
                {"vendorId", vendorId}
            };
            _regionManager.RequestNavigate(RegionNames.EditVendorRegion, new Uri("VendorDetailView", UriKind.Relative), parameters);
        }
    }
}