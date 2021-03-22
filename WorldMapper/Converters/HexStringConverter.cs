using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WorldMapper.Converters
{
    [ValueConversion(typeof(int), typeof(int))]
    public class HexStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var valueInt = value as int? ?? 0;
            return valueInt.ToString("X");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return int.TryParse(value as string, NumberStyles.HexNumber, culture,
                out var valueHex) ? valueHex : DependencyProperty.UnsetValue;
        }
    }
}