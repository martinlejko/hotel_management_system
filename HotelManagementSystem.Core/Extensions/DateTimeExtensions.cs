using System;
using System.Globalization;

namespace HotelManagementSystem.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToFriendlyDateString(this DateTime date)
        {
            if (date.Date == DateTime.Today)
            {
                return "Today";
            }
            if (date.Date == DateTime.Today.AddDays(1))
            {
                return "Tomorrow";
            }
            if (date.Date == DateTime.Today.AddDays(-1))
            {
                return "Yesterday";
            }

            return date.ToString("MMMM d, yyyy", CultureInfo.InvariantCulture);
        }

        public static string ToFriendlyTimeString(this DateTime date)
        {
            return date.ToString("h:mm tt", CultureInfo.InvariantCulture);
        }

        public static string ToFriendlyDateTimeString(this DateTime date)
        {
            return $"{date.ToFriendlyDateString()} at {date.ToFriendlyTimeString()}";
        }

        public static int GetNights(this DateTime checkInDate, DateTime checkOutDate)
        {
            return (checkOutDate.Date - checkInDate.Date).Days;
        }

        public static bool IsWeekend(this DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }

        public static bool IsOverlapping(this DateTime startDate, DateTime endDate, DateTime otherStartDate, DateTime otherEndDate)
        {
            return startDate < otherEndDate && endDate > otherStartDate;
        }
    }
} 