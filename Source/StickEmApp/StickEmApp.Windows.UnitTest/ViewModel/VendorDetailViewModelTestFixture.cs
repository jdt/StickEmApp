using System;
using System.Collections;
using NUnit.Framework;
using Prism.Events;
using Prism.Regions;
using Rhino.Mocks;
using StickEmApp.Dal;
using StickEmApp.Entities;
using StickEmApp.Windows.Infrastructure.Events;
using StickEmApp.Windows.ViewModel;

namespace StickEmApp.Windows.UnitTest.ViewModel
{
    [TestFixture]
    public class VendorDetailViewModelTestFixture : UnitOfWorkAwareTestFixture
    {
        private IVendorRepository _vendorRepository;
        private IEventAggregator _eventAggregator;
        
        private VendorDetailViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _vendorRepository = MockRepository.GenerateMock<IVendorRepository>();
            _eventAggregator = MockRepository.GenerateMock<IEventAggregator>();

            _viewModel = new VendorDetailViewModel(_vendorRepository, _eventAggregator);
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

            var returnedEvent = MockRepository.GenerateMock<VendorChangedEvent>();
            returnedEvent.Expect(p => p.Publish(generatedGuid));

            _eventAggregator.Expect(ea => ea.GetEvent<VendorChangedEvent>()).Return(returnedEvent);

            //act
            _viewModel.VendorName = "foo";
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
            _viewModel.HasFinished = true;

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
            Assert.That(savedVendor.Status, Is.EqualTo(VendorStatus.Finished));

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
                Status = VendorStatus.Finished,
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
            Assert.That(_viewModel.VendorName, Is.EqualTo("foo"));
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
            Assert.That(_viewModel.HasFinished, Is.True);
        }

        [TestCaseSource("UpdatingFieldsShouldRecalculateTotalsData")]
        public void UpdatingFieldsShouldRecalculateTotals(Action<VendorDetailViewModel> action)
        {
            //arrange
            var vendorGuid = Guid.NewGuid();

            var context = new NavigationContext(null, null);
            context.Parameters.Add("vendorId", vendorGuid);

            var vendorToDisplay = MockRepository.GenerateMock<Vendor>();
            vendorToDisplay.Expect(p => p.AmountReturned).Return(new AmountReturned()).Repeat.Any();
            vendorToDisplay.Expect(p => p.ChangeReceived).Return(new Money(0)).Repeat.Any();

            _vendorRepository.Expect(repo => repo.Get(vendorGuid)).Return(vendorToDisplay);

            //initial loading
            vendorToDisplay.Expect(p => p.CalculateTotalAmountRequired()).Return(new Money(0)).Repeat.Once();
            vendorToDisplay.Expect(p => p.CalculateTotalAmountReturned()).Return(new Money(0)).Repeat.Once();
            vendorToDisplay.Expect(p => p.CalculateSalesResult()).Return(new SalesResult()).Repeat.Once();
            
            //update
            vendorToDisplay.Expect(p => p.CalculateTotalAmountRequired()).Return(new Money(50));
            vendorToDisplay.Expect(p => p.CalculateTotalAmountReturned()).Return(new Money(30));
            vendorToDisplay.Expect(p => p.CalculateSalesResult()).Return(new SalesResult{Difference = new Money(20)});
            
            //act
            _viewModel.OnNavigatedTo(context);
            action(_viewModel); 

            //assert
            Assert.That(_viewModel.TotalAmountRequired, Is.EqualTo(new Money(50)));
            Assert.That(_viewModel.TotalAmountReturned, Is.EqualTo(new Money(30)));
            Assert.That(_viewModel.TotalDifference, Is.EqualTo(new Money(20)));
        }

        public static IEnumerable UpdatingFieldsShouldRecalculateTotalsData
        {

            get
            {
                Action<VendorDetailViewModel> received = b => { b.NumberOfStickersReceived = 5; };
                Action<VendorDetailViewModel> returned = b => { b.NumberOfStickersReturned = 5; };
                Action<VendorDetailViewModel> change = b => { b.ChangeReceived = 5; };
                Action<VendorDetailViewModel> fiveHundreds = b => { b.FiveHundreds = 5; };
                Action<VendorDetailViewModel> twoHundreds = b => { b.TwoHundreds = 5; };
                Action<VendorDetailViewModel> hundreds = b => { b.Hundreds = 5; };
                Action<VendorDetailViewModel> fifties = b => { b.Fifties = 5; };
                Action<VendorDetailViewModel> twenties = b => { b.Twenties = 5; };
                Action<VendorDetailViewModel> tens = b => { b.Tens = 5; };
                Action<VendorDetailViewModel> fives = b => { b.Fives = 5; };
                Action<VendorDetailViewModel> twos = b => { b.Twos = 5; };
                Action<VendorDetailViewModel> ones = b => { b.Ones = 5; };
                Action<VendorDetailViewModel> fiftyCents = b => { b.FiftyCents = 5; };
                Action<VendorDetailViewModel> twentyCents = b => { b.TwentyCents = 5; };
                Action<VendorDetailViewModel> tenCents = b => { b.TenCents = 5; };
                Action<VendorDetailViewModel> fiveCents = b => { b.FiveCents = 5; };
                Action<VendorDetailViewModel> twoCents = b => { b.TwoCents = 5; };
                Action<VendorDetailViewModel> oneCents = b => { b.OneCents = 5; };

                yield return received;
                yield return returned;
                yield return change;
                yield return fiveHundreds;
                yield return twoHundreds;
                yield return hundreds;
                yield return fifties;
                yield return twenties;
                yield return tens;
                yield return fives;
                yield return twos;
                yield return ones;
                yield return fiftyCents;
                yield return twentyCents;
                yield return tenCents;
                yield return fiveCents;
                yield return twoCents;
                yield return oneCents;
            }
        }  
    }
}