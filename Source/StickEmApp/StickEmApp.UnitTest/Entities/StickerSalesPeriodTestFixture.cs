using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using StickEmApp.Entities;

namespace StickEmApp.UnitTest.Entities
{
    [TestFixture]
    public class StickerSalesPeriodTestFixture
    {
        private StickerSalesPeriod _period;

        [SetUp]
        public void SetUp()
        {
            _period = new StickerSalesPeriod();
        }

        [Test]
        public void NumberOfStickersToSellShouldBeNumberOfStickersToSell()
        {
            //arrange
            _period.NumberOfStickersToSell = 33;

            //act
            var result = _period.CalculateStatus(new List<Vendor>());

            //assert
            Assert.That(result.NumberOfStickersToSell, Is.EqualTo(33));
        }

        [Test]
        public void NumberOfStickersSoldInPeriodShouldBeTotalOfSoldByAllFinishedVendorsInThePeriod()
        {
            //arrange
            var vendor1 = MockRepository.GenerateMock<Vendor>();
            var vendor2 = MockRepository.GenerateMock<Vendor>();
            var workingVendor = MockRepository.GenerateMock<Vendor>();

            vendor1.Expect(v => v.CalculateSalesResult()).Return(new SalesResult {NumberOfStickersSold = 5});
            vendor1.Expect(v => v.Status).Return(VendorStatus.Finished);

            vendor2.Expect(v => v.CalculateSalesResult()).Return(new SalesResult { NumberOfStickersSold = 7 });
            vendor2.Expect(v => v.Status).Return(VendorStatus.Finished);

            workingVendor.Expect(v => v.CalculateSalesResult()).Return(new SalesResult { NumberOfStickersSold = 7 });
            workingVendor.Expect(v => v.Status).Return(VendorStatus.Working);

            //act
            var result = _period.CalculateStatus(new[] {vendor1, vendor2, workingVendor});

            //assert
            Assert.That(result.NumberOfStickersSold, Is.EqualTo(12));
        }

        [Test]
        public void SalesTotalShouldBeStickerPriceTimesTotalNumberOfStickersSoldByFinishedVendors()
        {
            //arrange
            var finished = MockRepository.GenerateMock<Vendor>();
            finished.Expect(v => v.Status).Return(VendorStatus.Finished);
            finished.Expect(v => v.CalculateSalesResult()).Return(new SalesResult { NumberOfStickersSold = 3 });

            var working = MockRepository.GenerateMock<Vendor>();
            working.Expect(v => v.Status).Return(VendorStatus.Working);
            working.Expect(v => v.CalculateSalesResult()).Return(new SalesResult {NumberOfStickersSold = 5});

            //act
            var result = _period.CalculateStatus(new[] { finished, working });

            //assert
            Assert.That(result.SalesTotal, Is.EqualTo(new Money(15)));
        }

        [Test]
        public void NumberOfStickersWithVendorsShouldBeTotalOfStickersWithAllWorkingVendorsInThePeriod()
        {
            //arrange
            var vendor1 = MockRepository.GenerateMock<Vendor>();
            var vendor2 = MockRepository.GenerateMock<Vendor>();
            var finishedVendor = MockRepository.GenerateMock<Vendor>();

            vendor1.Expect(v => v.CalculateSalesResult()).Return(new SalesResult { NumberOfStickersReceived = 5 });
            vendor1.Expect(v => v.Status).Return(VendorStatus.Working);

            vendor2.Expect(v => v.CalculateSalesResult()).Return(new SalesResult { NumberOfStickersReceived = 7 });
            vendor2.Expect(v => v.Status).Return(VendorStatus.Working);

            finishedVendor.Expect(v => v.CalculateSalesResult()).Return(new SalesResult { NumberOfStickersSold = 7 });
            finishedVendor.Expect(v => v.Status).Return(VendorStatus.Finished);

            //act
            var result = _period.CalculateStatus(new[] { vendor1, vendor2, finishedVendor });

            //assert
            Assert.That(result.NumberOfStickersWithVendors, Is.EqualTo(12));
        }

        [Test]
        public void NumberOfStickersRemainingShouldBeTotalNumberOfStickersMinusStickersSoldByFinishedVendorsAndStickersWithWorkingVendors()
        {
            //arrange
            var workingVendor1 = MockRepository.GenerateMock<Vendor>();
            var workingVendor2 = MockRepository.GenerateMock<Vendor>();
            var finishedVendor1 = MockRepository.GenerateMock<Vendor>();
            var finishedVendor2 = MockRepository.GenerateMock<Vendor>();

            workingVendor1.Expect(v => v.CalculateSalesResult()).Return(new SalesResult { NumberOfStickersReceived = 5 });
            workingVendor1.Expect(v => v.Status).Return(VendorStatus.Working);

            workingVendor2.Expect(v => v.CalculateSalesResult()).Return(new SalesResult { NumberOfStickersReceived = 7 });
            workingVendor2.Expect(v => v.Status).Return(VendorStatus.Working);

            finishedVendor1.Expect(v => v.CalculateSalesResult()).Return(new SalesResult { NumberOfStickersSold = 13 });
            finishedVendor1.Expect(v => v.Status).Return(VendorStatus.Finished);

            finishedVendor2.Expect(v => v.CalculateSalesResult()).Return(new SalesResult { NumberOfStickersSold = 17 });
            finishedVendor2.Expect(v => v.Status).Return(VendorStatus.Finished);

            _period.NumberOfStickersToSell = 100;

            //act
            var result = _period.CalculateStatus(new[] { workingVendor1, workingVendor2, finishedVendor1, finishedVendor2 });

            //assert
            Assert.That(result.NumberOfStickersRemaining, Is.EqualTo(58));
        }
    }
}