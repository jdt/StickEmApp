namespace StickEmApp.Windows.ViewModel
{
    public class VendorViewModel : ViewModelBase
    {
        private string _name;

        public VendorViewModel(string name)
        {
            Name = name;
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }
    }
}
