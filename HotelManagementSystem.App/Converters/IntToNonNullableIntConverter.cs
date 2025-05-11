using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace HotelManagementSystem.App.Converters
{
    public class IntToNonNullableIntConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is int intValue)
            {
                return intValue;
            }
            
            return parameter is int minValue ? minValue : 1;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is int intValue)
            {
                return intValue;
            }
            
            return parameter is int minValue ? minValue : 1;
        }
    }
} 