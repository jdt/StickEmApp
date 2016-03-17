namespace StickEmApp.Windows.View
{
    public class ViewFactory : IViewFactory
    {
        public void DisplayVendorView()
        {
            new VendorView().Display();
        }
    }
}