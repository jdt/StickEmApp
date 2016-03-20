using System.Windows;

namespace StickEmApp.Windows.View
{
    public class ViewManager : IViewManager
    {
        public IView VendorView()
        {
            return new VendorView();
        }

        public void Close(DependencyObject view)
        {
            Window.GetWindow(view).Close();
        }
    }
}