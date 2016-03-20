using System.Windows;

namespace StickEmApp.Windows.View
{
    public interface IViewManager
    {
        IView VendorView();
        void Close(DependencyObject view);
    }
}