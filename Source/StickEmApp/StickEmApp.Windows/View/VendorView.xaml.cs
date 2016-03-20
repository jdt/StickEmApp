namespace StickEmApp.Windows.View
{
    /// <summary>
    /// Interaction logic for VendorView.xaml
    /// </summary>
    public partial class VendorView : IView
    {
        public VendorView()
        {
            InitializeComponent();

            Closed += VendorView_Closed;
        }

        private void VendorView_Closed(object sender, System.EventArgs e)
        {
            if (ViewClosed != null)
                ViewClosed();
        }

        public void Display()
        {
            ShowDialog();
        }

        public event ViewClosedEventHandler ViewClosed;
    }
}
