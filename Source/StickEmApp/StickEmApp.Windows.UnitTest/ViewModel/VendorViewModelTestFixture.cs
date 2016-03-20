using System.Windows;
using NUnit.Framework;
using Rhino.Mocks;
using StickEmApp.Dal;
using StickEmApp.Entities;
using StickEmApp.Windows.View;
using StickEmApp.Windows.ViewModel;

namespace StickEmApp.Windows.UnitTest.ViewModel
{
    [TestFixture]
    public class VendorViewModelTestFixture : UnitOfWorkAwareTestFixture
    {
        private IVendorRepository _vendorRepository;
        private IViewManager _viewManager;

        private VendorViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _vendorRepository = MockRepository.GenerateMock<IVendorRepository>();
            _viewManager = MockRepository.GenerateMock<IViewManager>();

            _viewModel = new VendorViewModel(_vendorRepository, _viewManager);
        }

        [Test]
        public void SaveChangesShouldCreateVendorAndCloseWindow()
        {
            //arrange
            var executingView = new DependencyObject();

            _vendorRepository.Expect(p => p.Save(Arg<Vendor>.Matches(
                    x => x.Name == "foo"
                )));

            _viewManager.Expect(p => p.Close(executingView));

            //act
            _viewModel.Name = "foo";
            _viewModel.SaveChangesCommand.Execute(executingView);

            //assert
            _vendorRepository.VerifyAllExpectations();
            _viewManager.VerifyAllExpectations();
        }
    }
}