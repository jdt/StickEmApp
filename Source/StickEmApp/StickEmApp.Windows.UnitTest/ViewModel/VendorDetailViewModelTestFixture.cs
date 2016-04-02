using System;
using NUnit.Framework;
using Prism.Events;
using Rhino.Mocks;
using StickEmApp.Dal;
using StickEmApp.Entities;
using StickEmApp.Windows.Infrastructure.Events;

namespace StickEmApp.Windows.UnitTest.ViewModel
{
    [TestFixture]
    public class VendorDetailViewModelTestFixture : UnitOfWorkAwareTestFixture
    {
        private IVendorRepository _vendorRepository;
        private IEventAggregator _eventAggregator;

        private Windows.ViewModel.VendorDetailViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _vendorRepository = MockRepository.GenerateMock<IVendorRepository>();
            _eventAggregator = MockRepository.GenerateMock<IEventAggregator>();
            
            _viewModel = new Windows.ViewModel.VendorDetailViewModel(_vendorRepository, _eventAggregator);
        }

        [Test]
        public void SaveChangesShouldCreateVendorAndRaiseVendorUpdatedEvent()
        {
            //arrange
            var generatedGuid = Guid.NewGuid();

            _vendorRepository.Expect(p => p.Save(Arg<Vendor>.Matches(
                x => x.Name == "foo"
                ))).WhenCalled(m => (m.Arguments[0] as Vendor).Id = generatedGuid);
            
            var returnedEvent = MockRepository.GenerateMock<VendorUpdatedEvent>();
            returnedEvent.Expect(p => p.Publish(generatedGuid));

            _eventAggregator.Expect(ea => ea.GetEvent<VendorUpdatedEvent>()).Return(returnedEvent);

            //act
            _viewModel.Name = "foo";
            _viewModel.SaveChangesCommand.Execute(null);

            //assert
            _vendorRepository.VerifyAllExpectations();
            _eventAggregator.VerifyAllExpectations();
            returnedEvent.VerifyAllExpectations();
        }
    }
}