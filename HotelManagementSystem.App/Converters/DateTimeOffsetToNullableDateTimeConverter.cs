using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace HotelManagementSystem.App.Converters
{
    public class DateTimeOffsetToNullableDateTimeConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            // Converting from DateTime? to DateTimeOffset for the UI
            if (value is DateTime dateTime)
            {
                return new DateTimeOffset(dateTime);
            }
            
            return null;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            // Converting from DateTimeOffset to DateTime? when saving
            if (value is DateTimeOffset dateTimeOffset)
            {
                return dateTimeOffset.DateTime;
            }
            
            return null;
        }
    }
} 