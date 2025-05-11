using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagementSystem.Core.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }
        
        public int RoomId { get; set; }

        [Required]
        public DateTime CheckInDate { get; set; }

        [Required]
        public DateTime CheckOutDate { get; set; }

        [Required]
        public ReservationStatus Status { get; set; } = ReservationStatus.Confirmed;

        [Required]
        public decimal TotalPrice { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [MaxLength(500)]
        public string? SpecialRequests { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual Room? Room { get; set; }

        [NotMapped]
        public int DurationInDays => (CheckOutDate - CheckInDate).Days;

        public void CalculateTotalPrice()
        {
            if (Room != null && DurationInDays > 0)
            {
                TotalPrice = Room.PricePerNight * DurationInDays;
            }
        }
    }

    public enum ReservationStatus
    {
        Pending,
        Confirmed,
        CheckedIn,
        CheckedOut,
        Cancelled
    }
} 