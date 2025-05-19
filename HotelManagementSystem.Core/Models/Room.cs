using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelManagementSystem.Core.Models
{
    /// <summary>
    /// Represents a hotel room with its properties and availability status.
    /// </summary>
    public class Room
    {
        /// <summary>
        /// The unique identifier for the room.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The room number identifier. Required field with maximum length of 20 characters.
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string RoomNumber { get; set; } = string.Empty;

        /// <summary>
        /// The type of room (Single, Double, Twin, Suite, etc.).
        /// </summary>
        [Required]
        public RoomType Type { get; set; }

        /// <summary>
        /// The maximum number of guests the room can accommodate.
        /// </summary>
        [Required]
        public int Capacity { get; set; }

        /// <summary>
        /// The price per night for staying in this room.
        /// </summary>
        [Required]
        public decimal PricePerNight { get; set; }

        /// <summary>
        /// A detailed description of the room and its features. Optional field.
        /// </summary>
        [MaxLength(500)]
        public string? Description { get; set; }

        /// <summary>
        /// Indicates whether the room is currently available for booking.
        /// </summary>
        public bool IsAvailable { get; set; } = true;

        /// <summary>
        /// Collection of reservations associated with this room.
        /// </summary>
        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }

    /// <summary>
    /// Defines the different types of rooms available in the hotel.
    /// </summary>
    public enum RoomType
    {
        Single,
        Double,
        Twin,
        Suite,
        Deluxe,
        Family
    }
} 