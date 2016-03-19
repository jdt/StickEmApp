namespace StickEmApp.Windows.ViewModel
{
    public class VendorListItemViewModel : ViewModelBase
    {
        public VendorListItemViewModel(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
