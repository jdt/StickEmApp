using System.ComponentModel.Composition;
using System.Windows.Controls;
using StickEmApp.Windows.Infrastructure;
using StickEmApp.Windows.Infrastructure.Behaviors;
using StickEmApp.Windows.ViewModel;

namespace StickEmApp.Windows.View
{
    /// <summary>
    /// Interaction logic for StickerSalesPeriodDetailView.xaml
    /// </summary>
    [ViewExport("StickerSalesPeriodDetailView", RegionName = RegionNames.EditVendorRegion)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class StickerSalesPeriodDetailView : UserControl
    {
        public StickerSalesPeriodDetailView()
        {
            InitializeComponent();
        }

        [Import]
        public StickerSalesPeriodDetailViewModel ViewModel
        {
            set { DataContext = value; }
        }
    }
}
