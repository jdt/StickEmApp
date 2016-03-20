using StickEmApp.Windows.View;

namespace StickEmApp.Windows.UnitTest.ViewModel
{
    public class TestableIView : IView
    {
        public TestableIView()
        {
            WasDisplayed = false;
        }

        public event ViewClosedEventHandler ViewClosed;

        public void Display()
        {
            WasDisplayed = true;
        }

        public void DoClose()
        {
            if (ViewClosed != null)
                ViewClosed();
        }

        public bool WasDisplayed { get; private set; }
    }
}