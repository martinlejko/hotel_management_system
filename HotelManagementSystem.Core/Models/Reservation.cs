using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagementSystem.Core.Models
{
    /// <summary>
    /// Represents a hotel room reservation made by a customer.
    /// Tracks check-in/check-out dates, status, and associated customer and room.
    /// </summary>
    public class Reservation
    {
        /// <summary>
        /// The unique identifier for the reservation.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The unique identifier of the customer who made this reservation.
        /// </summary>
        public int CustomerId { get; set; }
        
        /// <summary>
        /// The unique identifier of the room that is reserved.
        /// </summary>
        public int RoomId { get; set; }

        /// <summary>
        /// The date when the customer will check in to the hotel.
        /// </summary>
        [Required]
        public DateTime CheckInDate { get; set; }

        /// <summary>
        /// The date when the customer will check out from the hotel.
        /// </summary>
        [Required]
        public DateTime CheckOutDate { get; set; }

        /// <summary>
        /// The current status of the reservation (Pending, Confirmed, CheckedIn, CheckedOut, Cancelled).
        /// </summary>
        [Required]
        public ReservationStatus Status { get; set; } = ReservationStatus.Confirmed;

        /// <summary>
        /// The total price for the entire stay.
        /// </summary>
        [Required]
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// The date and time when the reservation was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// Any special requests or notes for this reservation. Optional field.
        /// </summary>
        [MaxLength(500)]
        public string? SpecialRequests { get; set; }

        /// <summary>
        /// The customer who made this reservation. Navigation property.
        /// </summary>
        public virtual Customer? Customer { get; set; }
        
        /// <summary>
        /// The room that is reserved. Navigation property.
        /// </summary>
        public virtual Room? Room { get; set; }

        /// <summary>
        /// Gets the duration of the stay in days. Calculated from check-in and check-out dates.
        /// </summary>
        [NotMapped]
        public int DurationInDays => (CheckOutDate - CheckInDate).Days;

        /// <summary>
        /// Calculates the total price for the stay based on room price and duration.
        /// </summary>
        public void CalculateTotalPrice()
        {
            if (Room != null && DurationInDays > 0)
            {
                TotalPrice = Room.PricePerNight * DurationInDays;
            }
        }
    }

    /// <summary>
    /// Defines the possible statuses of a reservation in the system.
    /// </summary>
    public enum ReservationStatus
    {
        Pending,
        Confirmed,
        CheckedIn,
        CheckedOut,
        Cancelled
    }
} 