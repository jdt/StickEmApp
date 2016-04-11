using System;
using System.Globalization;
using System.Windows.Data;
using StickEmApp.Entities;

namespace StickEmApp.Windows.ViewModel.Converters
{
    public class MoneyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var money = value as Money;
            if (money == null)
                return string.Empty;

            return "€" + money.Value.ToString("F2");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}