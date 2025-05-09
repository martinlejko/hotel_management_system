using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace HotelManagementSystem.Core.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string? value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static bool IsNullOrWhiteSpace(this string? value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static string? Truncate(this string? value, int maxLength, string suffix = "...")
        {
            if (value == null || value.Length <= maxLength)
            {
                return value;
            }

            return value.Substring(0, maxLength) + suffix;
        }

        public static string? ToTitleCase(this string? value)
        {
            if (value.IsNullOrEmpty())
            {
                return value;
            }

            var textInfo = CultureInfo.CurrentCulture.TextInfo;
            return textInfo.ToTitleCase(value!.ToLower());
        }

        public static string? RemoveSpaces(this string? value)
        {
            if (value.IsNullOrEmpty())
            {
                return value;
            }

            return value!.Replace(" ", string.Empty);
        }

        public static string? ToSafeFileName(this string? value)
        {
            if (value.IsNullOrEmpty())
            {
                return value;
            }

            var invalidChars = System.IO.Path.GetInvalidFileNameChars();
            return new string(value!.Where(c => !invalidChars.Contains(c)).ToArray());
        }
    }
} 