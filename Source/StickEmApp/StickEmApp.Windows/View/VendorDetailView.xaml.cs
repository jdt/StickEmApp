using System.ComponentModel.Composition;
using System.Windows.Controls;
using System.Windows.Input;
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
        }

        [Import]
        public VendorDetailViewModel ViewModel
        {
            set { DataContext = value; }
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
