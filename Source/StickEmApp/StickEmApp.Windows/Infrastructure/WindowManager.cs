using System;
using System.ComponentModel.Composition;
using System.Windows;
using Microsoft.Win32;
using Prism.Regions;
using StickEmApp.Service;

namespace StickEmApp.Windows.Infrastructure
{
    [Export(typeof(IWindowManager))]
    public class WindowManager : IWindowManager
    {
        private readonly IRegionManager _regionManager;
        private readonly IResourceManager _resourceManager;

        [ImportingConstructor]
        public WindowManager(IRegionManager regionManager, IResourceManager resourceManager)
        {
            _regionManager = regionManager;
            _resourceManager = resourceManager;
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

        public void DisplaySummary()
        {
            _regionManager.RequestNavigate(RegionNames.EditVendorRegion, new Uri("SummaryView", UriKind.Relative));
        }

        public void DisplayEditStickerSalesPeriod()
        {
            _regionManager.RequestNavigate(RegionNames.EditVendorRegion, new Uri("StickerSalesPeriodDetailView", UriKind.Relative));
        }

        public string DisplayFileSelection()
        {
            var saveFileDialog = new SaveFileDialog
            {
                AddExtension = true,
                CheckPathExists = true,
                DefaultExt = "xlsx",
                OverwritePrompt = true,
                ValidateNames = true,
                Filter = string.Format("{0} | *.xlsx", _resourceManager.GetString("ExcelWorkbook"))
            };

            var result = saveFileDialog.ShowDialog();
            if (result.HasValue == false || result.Value == false)
                return null;
            return saveFileDialog.FileName;
        }

        public void DisplayInformation(string message)
        {
            MessageBox.Show(message, _resourceManager.GetString("Message"), MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}