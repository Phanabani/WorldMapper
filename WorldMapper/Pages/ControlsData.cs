using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WorldMapper.Pages
{
    public class ControlsData
    {
        public int PlayerPositionAddress { get; set; }
        public int CameraPositionAddress { get; set; }
        public int CameraRotationAddress { get; set; }
        public float FieldOfView { get; set; } = 60;
    }

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
