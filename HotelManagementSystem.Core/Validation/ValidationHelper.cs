using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace HotelManagementSystem.Core.Validation
{
    public static class ValidationHelper
    {
        // Email pattern as per RFC 5322
        private static readonly Regex EmailPattern = new(
            @"^[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?$", 
            RegexOptions.Compiled);
        
        // Phone number pattern - accepts formats:
        // +123456789, (123) 456-7890, 123-456-7890, 123.456.7890, 123 456 7890
        private static readonly Regex PhonePattern = new(
            @"^(\+\d{1,3})?[\s.-]?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$", 
            RegexOptions.Compiled);
        
        /// <summary>
        /// Validates if a string is a properly formatted email address
        /// </summary>
        public static bool IsValidEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;
                
            return EmailPattern.IsMatch(email);
        }
        
        /// <summary>
        /// Validates if a string is a properly formatted phone number
        /// </summary>
        public static bool IsValidPhoneNumber(string? phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;
                
            return PhonePattern.IsMatch(phoneNumber);
        }
        
        /// <summary>
        /// Validates if a string contains only digits
        /// </summary>
        public static bool ContainsOnlyDigits(string? value)
        {
            if (string.IsNullOrEmpty(value))
                return false;
                
            return value.All(char.IsDigit);
        }
    }
} 