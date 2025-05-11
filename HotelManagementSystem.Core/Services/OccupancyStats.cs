using System;

namespace HotelManagementSystem.Core.Services
{
    public class OccupancyStats
    {
        public DateTime Date { get; set; }
        public int TotalRooms { get; set; }
        public int OccupiedRooms { get; set; }
        public int AvailableRooms { get; set; }
        public double OccupancyRate { get; set; }
    }
} 