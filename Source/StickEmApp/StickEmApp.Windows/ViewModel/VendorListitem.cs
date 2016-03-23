using Prism.Mvvm;

namespace StickEmApp.Windows.ViewModel
{
    public class VendorListItem : BindableBase
    {
        public VendorListItem(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
