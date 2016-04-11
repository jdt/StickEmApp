using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using StickEmApp.Entities;

namespace StickEmApp.Windows.ViewModel.Converters
{
    public class VendorStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var statusValue = value as VendorStatus?;
            if (statusValue.HasValue == false)
                return string.Empty;

            switch (statusValue.Value)
            {
                case VendorStatus.Finished:
                    return Application.Current.FindResource("VendorStatusFinished");
                case VendorStatus.Working:
                    return Application.Current.FindResource("VendorStatusWorking");
                case VendorStatus.Removed:
                    return string.Empty;
                default:
                    return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}