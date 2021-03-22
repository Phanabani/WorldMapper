using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WorldMapper.Converters
{
    [ValueConversion(typeof(double), typeof(float))]
    public class FloatRoundingConverter : IValueConverter
    {
        public int Digits { get; set; } = 0;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // This is the method that converts to the property
            var valueDouble = value as double? ?? default(double);
            var round = (float) Math.Round(valueDouble, Digits);
            return round;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // This is the method that converts to the visual representation
            return value as float? ?? default(float);
        }
    }
}
