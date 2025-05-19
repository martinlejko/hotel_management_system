using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace HotelManagementSystem.App.Converters
{
    /// <summary>
    /// Converter that converts between nullable <see cref="DateTime"/> and <see cref="DateTimeOffset"/> values.
    /// Used for binding nullable DateTime properties to UI elements that work with DateTimeOffset.
    /// </summary>
    public class DateTimeOffsetToNullableDateTimeConverter : IValueConverter
    {
        /// <summary>
        /// Converts a <see cref="DateTime"/> to a <see cref="DateTimeOffset"/>.
        /// </summary>
        /// <param name="value">The DateTime value to convert.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">Additional parameter for the converter to handle. Not used.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A DateTimeOffset equivalent to the provided DateTime, or null if conversion is not possible.</returns>
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is DateTime dateTime)
            {
                return new DateTimeOffset(dateTime);
            }
            
            return null;
        }

        /// <summary>
        /// Converts a <see cref="DateTimeOffset"/> back to a nullable <see cref="DateTime"/>.
        /// </summary>
        /// <param name="value">The DateTimeOffset value to convert.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">Additional parameter for the converter to handle. Not used.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>The DateTime component of the DateTimeOffset, or null if conversion is not possible.</returns>
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is DateTimeOffset dateTimeOffset)
            {
                return dateTimeOffset.DateTime;
            }
            
            return null;
        }
    }
} 