using System;
using FluentNHibernate.Testing;
using NUnit.Framework;
using StickEmApp.Dal;
using StickEmApp.Entities;

namespace StickEmApp.UnitTest.Dal.Mapping
{
    [TestFixture]
    public class VendorMappingTestFixture : UnitOfWorkAwareTestFixture
    {
        [Test]
        public void TestMap()
        {
            new PersistenceSpecification<Vendor>(UnitOfWorkManager.Session)
            .CheckProperty(c => c.Name, "foo")
            .VerifyTheMappings();
        }
    }
}