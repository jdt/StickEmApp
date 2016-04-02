using System;
using NUnit.Framework;
using Prism.Events;
using Prism.Regions;
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
        public void SaveChangesShouldSaveVendorAndRaiseVendorUpdatedEvent()
        {
            //arrange
            var generatedGuid = Guid.NewGuid();
            Vendor savedVendor = null;

            _vendorRepository.Expect(p => p.Save(Arg<Vendor>.Is.Anything)).WhenCalled(m =>
            {
                savedVendor = (m.Arguments[0] as Vendor); 
                savedVendor.Id = generatedGuid;
            });

            var returnedEvent = MockRepository.GenerateMock<VendorUpdatedEvent>();
            returnedEvent.Expect(p => p.Publish(generatedGuid));

            _eventAggregator.Expect(ea => ea.GetEvent<VendorUpdatedEvent>()).Return(returnedEvent);

            //act
            _viewModel.Name = "foo";
            _viewModel.ChangeReceived = 55.75m;
            _viewModel.NumberOfStickersReceived = 15;
            _viewModel.NumberOfStickersReturned = 10;
            _viewModel.FiveHundreds = 500;
            _viewModel.TwoHundreds = 200;
            _viewModel.Hundreds = 100;
            _viewModel.Fifties = 50;
            _viewModel.Twenties = 20;
            _viewModel.Tens = 10;
            _viewModel.Fives = 5;
            _viewModel.Twos = 2;
            _viewModel.Ones = 1;
            _viewModel.FiftyCents = 5010;
            _viewModel.TwentyCents = 2010;
            _viewModel.TenCents = 1010;
            _viewModel.FiveCents = 510;
            _viewModel.TwoCents = 210;
            _viewModel.OneCents = 110;

            _viewModel.SaveChangesCommand.Execute(null);

            //assert
            Assert.That(savedVendor.Name, Is.EqualTo("foo"));
            Assert.That(savedVendor.ChangeReceived, Is.EqualTo(new Money(55.75m)));
            Assert.That(savedVendor.NumberOfStickersReceived, Is.EqualTo(15));
            Assert.That(savedVendor.NumberOfStickersReturned, Is.EqualTo(10));
            Assert.That(savedVendor.AmountReturned.FiveHundreds, Is.EqualTo(500));
            Assert.That(savedVendor.AmountReturned.TwoHundreds, Is.EqualTo(200));
            Assert.That(savedVendor.AmountReturned.Hundreds, Is.EqualTo(100));
            Assert.That(savedVendor.AmountReturned.Fifties, Is.EqualTo(50));
            Assert.That(savedVendor.AmountReturned.Twenties, Is.EqualTo(20));
            Assert.That(savedVendor.AmountReturned.Tens, Is.EqualTo(10));
            Assert.That(savedVendor.AmountReturned.Fives, Is.EqualTo(5));
            Assert.That(savedVendor.AmountReturned.Twos, Is.EqualTo(2));
            Assert.That(savedVendor.AmountReturned.Ones, Is.EqualTo(1));
            Assert.That(savedVendor.AmountReturned.FiftyCents, Is.EqualTo(5010));
            Assert.That(savedVendor.AmountReturned.TwentyCents, Is.EqualTo(2010));
            Assert.That(savedVendor.AmountReturned.TenCents, Is.EqualTo(1010));
            Assert.That(savedVendor.AmountReturned.FiveCents, Is.EqualTo(510));
            Assert.That(savedVendor.AmountReturned.TwoCents, Is.EqualTo(210));
            Assert.That(savedVendor.AmountReturned.OneCents, Is.EqualTo(110));

            _vendorRepository.VerifyAllExpectations();
            _eventAggregator.VerifyAllExpectations();
            returnedEvent.VerifyAllExpectations();
        }

        [Test]
        public void NavigatingToViewWithVendorIdShouldDisplayVendor()
        {
            //arrange
            var vendorGuid = Guid.NewGuid();

            var context = new NavigationContext(null, null);
            context.Parameters.Add("vendorId", vendorGuid);

            var vendorToDisplay = new Vendor
            {
                Name = "foo",
                ChangeReceived = new Money(55.75m),
                NumberOfStickersReceived = 15,
                NumberOfStickersReturned = 10,
                AmountReturned = new AmountReturned
                {
                    FiveHundreds = 500,
                    TwoHundreds = 200,
                    Hundreds = 100,
                    Fifties = 50,
                    Twenties = 20,
                    Tens = 10,
                    Fives = 5,
                    Twos = 2,
                    Ones = 1,
                    FiftyCents = 5010,
                    TwentyCents = 2010,
                    TenCents = 1010,
                    FiveCents = 510,
                    TwoCents = 210,
                    OneCents = 110
                }
            };
            _vendorRepository.Expect(repo => repo.Get(vendorGuid)).Return(vendorToDisplay);

            //act
            _viewModel.OnNavigatedTo(context);

            //assert
            Assert.That(_viewModel.Name, Is.EqualTo("foo"));
            Assert.That(_viewModel.ChangeReceived, Is.EqualTo(55.75m));
            Assert.That(_viewModel.NumberOfStickersReceived, Is.EqualTo(15));
            Assert.That(_viewModel.NumberOfStickersReturned, Is.EqualTo(10));
            Assert.That(_viewModel.FiveHundreds, Is.EqualTo(500));
            Assert.That(_viewModel.TwoHundreds, Is.EqualTo(200));
            Assert.That(_viewModel.Hundreds, Is.EqualTo(100));
            Assert.That(_viewModel.Fifties, Is.EqualTo(50));
            Assert.That(_viewModel.Twenties, Is.EqualTo(20));
            Assert.That(_viewModel.Tens, Is.EqualTo(10));
            Assert.That(_viewModel.Fives, Is.EqualTo(5));
            Assert.That(_viewModel.Twos, Is.EqualTo(2));
            Assert.That(_viewModel.Ones, Is.EqualTo(1));
            Assert.That(_viewModel.FiftyCents, Is.EqualTo(5010));
            Assert.That(_viewModel.TwentyCents, Is.EqualTo(2010));
            Assert.That(_viewModel.TenCents, Is.EqualTo(1010));
            Assert.That(_viewModel.FiveCents, Is.EqualTo(510));
            Assert.That(_viewModel.TwoCents, Is.EqualTo(210));
            Assert.That(_viewModel.OneCents, Is.EqualTo(110));
        }
    }
}