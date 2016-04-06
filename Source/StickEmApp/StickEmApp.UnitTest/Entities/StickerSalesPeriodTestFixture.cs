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
        public void NumberOfStickersSoldInPeriodShouldBeTotalOfSoldByAllVendorsInThePeriod()
        {
            //arrange
            var vendor1 = MockRepository.GenerateMock<Vendor>();
            var vendor2 = MockRepository.GenerateMock<Vendor>();

            vendor1.Expect(v => v.CalculateSalesResult()).Return(new SalesResult {NumberOfStickersSold = 5});
            vendor2.Expect(v => v.CalculateSalesResult()).Return(new SalesResult {NumberOfStickersSold = 7});

            //act
            var result = _period.CalculateStatus(new[] {vendor1, vendor2});

            //assert
            Assert.That(result.NumberOfStickersSold, Is.EqualTo(12));
        }

        [Test]
        public void SalesTotalShouldBeStickerPriceTimesTotalNumberOfStickersSold()
        {
            //arrange
            var vendor = MockRepository.GenerateMock<Vendor>();
            vendor.Expect(v => v.CalculateSalesResult()).Return(new SalesResult { NumberOfStickersSold = 3 });

            //act
            var result = _period.CalculateStatus(new[] { vendor });

            //assert
            Assert.That(result.SalesTotal, Is.EqualTo(new Money(15)));
        }
    }
}