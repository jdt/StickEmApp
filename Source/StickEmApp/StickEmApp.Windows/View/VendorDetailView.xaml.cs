using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using StickEmApp.Windows.Infrastructure;
using StickEmApp.Windows.Infrastructure.Behaviors;
using StickEmApp.Windows.ViewModel;

namespace StickEmApp.Windows.View
{
    /// <summary>
    /// Interaction logic for VendorDetailView.xaml
    /// </summary>
    [ViewExport("VendorDetailView", RegionName = RegionNames.EditVendorRegion)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class VendorDetailView : UserControl
    {
        public VendorDetailView()
        {
            InitializeComponent();

            IsVisibleChanged += VendorDetailView_IsVisibleChanged;
        }

        private void VendorDetailView_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                //https://social.msdn.microsoft.com/Forums/vstudio/en-US/849393da-a04d-4f84-860c-da4f36471984/set-focus-on-textbox?forum=wpf
                //the focus gets set, but is lost through the event handling. this little hack sets the focus after everything has finished handling
                Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, (System.Threading.ThreadStart)delegate
                {
                    VendorName.Focus();
                });
            }
        }

        [Import]
        public VendorDetailViewModel ViewModel
        {
            set
            {
                DataContext = value;
            }
        }

        private void NumericTextBoxKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            SelectAll(sender as TextBox);
        }

        private void NumericTextBoxMouseFocus(object sender, MouseEventArgs e)
        {
            SelectAll(sender as TextBox);
        }

        private static void SelectAll(TextBox textBox)
        {
            textBox.SelectAll();
        }
    }
}
