﻿using NUnit.Framework;
using StickEmApp.Dal;
using StickEmApp.Entities;

namespace StickEmApp.UnitTest.Dal
{
    [TestFixture]
    public class StickerSalesPeriodRepositoryTestFixture : UnitOfWorkAwareTestFixture
    {
        private StickerSalesPeriodRepository _repo;

        [SetUp]
        public void SetUp()
        {
            _repo = new StickerSalesPeriodRepository();
        }

        [Test]
        public void TestGet()
        {
            var period = new StickerSalesPeriod
            {
                NumberOfStickersToSell = 55
            };

            Session.Save(period);

            RenewSession();

            var savedVendor = _repo.Get();

            Assert.That(savedVendor.NumberOfStickersToSell, Is.EqualTo(55));
        }
    }
}