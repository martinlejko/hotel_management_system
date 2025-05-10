using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace HotelManagementSystem.App.Converters
{
    public class IntToNonNullableIntConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            // If value is null or not an int, return the minimum value (typically 1)
            if (value is int intValue)
            {
                return intValue;
            }
            
            // Default minimum value (can be parameterized)
            return parameter is int minValue ? minValue : 1;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            // If value is null, return the minimum value (typically 1)
            if (value is int intValue)
            {
                return intValue;
            }
            
            // Default minimum value (can be parameterized)
            return parameter is int minValue ? minValue : 1;
        }
    }
} 