using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelManagementSystem.Core.Models
{
    /// <summary>
    /// Represents a customer in the hotel management system.
    /// Stores personal information and tracks the customer's reservations.
    /// </summary>
    public class Customer
    {
        /// <summary>
        /// The unique identifier for the customer.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The customer's first name. Required field with maximum length of 100 characters.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// The customer's last name. Required field with maximum length of 100 characters.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// The customer's email address. Required field, must be a valid email format.
        /// </summary>
        [Required]
        [MaxLength(255)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// The customer's phone number. Optional field with maximum length of 20 characters.
        /// </summary>
        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// The customer's address. Optional field with maximum length of 255 characters.
        /// </summary>
        [MaxLength(255)]
        public string? Address { get; set; }

        /// <summary>
        /// The customer's date of birth. Optional field.
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// The date and time when the customer record was created. Defaults to current date and time.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// Collection of reservations made by this customer.
        /// </summary>
        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

        /// <summary>
        /// Gets the full name of the customer by combining first and last name.
        /// </summary>
        public string FullName => $"{FirstName} {LastName}";
    }
} 