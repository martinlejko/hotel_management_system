using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelManagementSystem.Core.Models
{
    public class Room
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string RoomNumber { get; set; } = string.Empty;

        [Required]
        public RoomType Type { get; set; }

        [Required]
        public int Capacity { get; set; }

        [Required]
        public decimal PricePerNight { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public bool IsAvailable { get; set; } = true;

        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }

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