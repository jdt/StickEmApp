using System.ComponentModel.Composition;
using System.Windows.Controls;
using StickEmApp.Windows.Infrastructure;
using StickEmApp.Windows.Infrastructure.Behaviors;
using StickEmApp.Windows.ViewModel;

namespace StickEmApp.Windows.View
{
    /// <summary>
    /// Interaction logic for SummaryView.xaml
    /// </summary>
    [ViewExport(RegionName = RegionNames.EditVendorRegion)]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class SummaryView : UserControl
    {
        public SummaryView()
        {
            InitializeComponent();
        }

        [Import]
        SummaryViewModel ViewModel
        {
            set
            {
                DataContext = value;
            }
        }
    }
}
