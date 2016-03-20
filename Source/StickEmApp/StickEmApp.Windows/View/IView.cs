namespace StickEmApp.Windows.View
{
    public delegate void ViewClosedEventHandler();

    public interface IView
    {
        event ViewClosedEventHandler ViewClosed;
        void Display();
    }
}