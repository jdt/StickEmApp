using System;
using System.Linq;
using NUnit.Framework;
using Prism.Regions;
using Rhino.Mocks;
using StickEmApp.Service;
using StickEmApp.Windows.Infrastructure;

namespace StickEmApp.Windows.UnitTest.Infrastructure
{
    [TestFixture]
    public class WindowManagerTestFixture
    {
        private IRegionManager _regionManager;
        private IResourceManager _resourceManager;

        private WindowManager _windowManager;

        [SetUp]
        public void SetUp()
        {
            _regionManager = MockRepository.GenerateMock<IRegionManager>();
            _resourceManager = MockRepository.GenerateMock<IResourceManager>();

            _windowManager = new WindowManager(_regionManager, _resourceManager);
        }

        [Test]
        public void AddVendorShouldDisplayVendorDetailViewInEditVendorRegion()
        {
            //arrange
            _regionManager.Expect(p => p.RequestNavigate(RegionNames.EditVendorRegion, new Uri("VendorDetailView", UriKind.Relative)));

            //act
            _windowManager.DisplayAddVendor();

            //assert
            _regionManager.VerifyAllExpectations();
        }

        [Test]
        public void EditVendorShouldDisplayVendorDetailViewForVendorInEditVendorRegion()
        {
            //arrange
            var vendorToEdit = new Guid("eac49554-b348-4ed5-9238-d254e3301980");

            var parameters = new NavigationParameters
            {
                {"vendorId", vendorToEdit}
            };
            _regionManager.Expect(p => p.RequestNavigate(
                Arg<string>.Is.Equal(RegionNames.EditVendorRegion), 
                Arg<Uri>.Is.Equal(new Uri("VendorDetailView", UriKind.Relative)), 
                Arg<NavigationParameters>.Matches(x => IsParametersEqual(x, parameters))
            ));

            //act
            _windowManager.DisplayEditVendor(vendorToEdit);

            //assert
            _regionManager.VerifyAllExpectations();
        }

        [Test]
        public void DisplaySummaryShouldShowSummaryInEditVendorRegion()
        {
            //arrange
            _regionManager.Expect(p => p.RequestNavigate(RegionNames.EditVendorRegion, new Uri("SummaryView", UriKind.Relative)));

            //act
            _windowManager.DisplaySummary();

            //assert
            _regionManager.VerifyAllExpectations();
        }

        [Test]
        public void DisplayEditStickerSalesPeriodShouldShowStickerSalesPeriodDetailViewInEditVendorRegion()
        {
            //arrange
            _regionManager.Expect(p => p.RequestNavigate(RegionNames.EditVendorRegion, new Uri("StickerSalesPeriodDetailView", UriKind.Relative)));

            //act
            _windowManager.DisplayEditStickerSalesPeriod();

            //assert
            _regionManager.VerifyAllExpectations();
        }

        private static bool IsParametersEqual(NavigationParameters a, NavigationParameters b)
        {
            var listA = a.ToList().OrderBy(x => x.Key);
            var listB = b.ToList().OrderBy(x => x.Key);

            if (listA.Count() != listB.Count())
                return false;

            for(var i = 0; i < listA.Count(); i++)
            {
                if (listA.ElementAt(i).Key != listB.ElementAt(i).Key)
                    return false;
                if (listA.ElementAt(i).Value.Equals(listB.ElementAt(i).Value) == false)
                    return false;
            }

            return true;
        }
    }
}