using System.Windows;
using NUnit.Framework;
using Prism.Events;
using Rhino.Mocks;
using StickEmApp.Dal;
using StickEmApp.Entities;
using StickEmApp.Windows.ViewModel;

namespace StickEmApp.Windows.UnitTest.ViewModel
{
    [TestFixture]
    public class VendorViewModelTestFixture : UnitOfWorkAwareTestFixture
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
        public void SaveChangesShouldCreateVendorAndCloseWindow()
        {
            //arrange
            var executingView = new DependencyObject();

            _vendorRepository.Expect(p => p.Save(Arg<Vendor>.Matches(
                    x => x.Name == "foo"
                )));

            //act
            _viewModel.Name = "foo";
            _viewModel.SaveChangesCommand.Execute(executingView);

            //assert
            _vendorRepository.VerifyAllExpectations();
        }
    }
}