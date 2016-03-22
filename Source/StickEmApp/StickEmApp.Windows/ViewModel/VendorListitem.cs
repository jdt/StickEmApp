using Prism.Mvvm;

namespace StickEmApp.Windows.ViewModel
{
    public class VendorListitem : BindableBase
    {
        public VendorListitem(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
