using System;
using System.Windows.Input;

namespace StickEmApp.Windows.ViewModel
{
    public class VendorViewModel : ViewModelBase
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public ICommand SaveChangesCommand { get { return new Command(i => SaveChanges()); } }

        private void SaveChanges()
        {

        }
    }
}