using System.ComponentModel.Composition;
using System.Windows.Controls;
using StickEmApp.Windows.Infrastructure;
using StickEmApp.Windows.Infrastructure.Behaviors;

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
    }
}
