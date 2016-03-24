using System;
using Prism.Mvvm;

namespace StickEmApp.Windows.ViewModel
{
    public class VendorListItem : BindableBase
    {
        public VendorListItem(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
