// BooleanToOuiNonConverter.cs
using System;
using System.Globalization;
using System.Windows.Data;

namespace CiteU.Converters
{
    public class BooleanToOuiNonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool booleanValue)
            {
                return booleanValue ? "Oui" : "Non";
            }
            return "Non";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}