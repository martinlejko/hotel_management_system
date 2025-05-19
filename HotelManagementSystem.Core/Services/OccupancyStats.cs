using System;

namespace HotelManagementSystem.Core.Services
{
    /// <summary>
    /// Represents statistics about hotel room occupancy for a specific date.
    /// Provides information on room availability and occupancy rates.
    /// </summary>
    public class OccupancyStats
    {
        /// <summary>
        /// The date for which these occupancy statistics apply.
        /// </summary>
        public DateTime Date { get; set; }
        
        /// <summary>
        /// The total number of rooms in the hotel.
        /// </summary>
        public int TotalRooms { get; set; }
        
        /// <summary>
        /// The number of rooms that are currently occupied.
        /// </summary>
        public int OccupiedRooms { get; set; }
        
        /// <summary>
        /// The number of rooms that are currently available for booking.
        /// </summary>
        public int AvailableRooms { get; set; }
        
        /// <summary>
        /// The occupancy rate as a percentage (0.0 to 1.0).
        /// Calculated as OccupiedRooms / TotalRooms.
        /// </summary>
        public double OccupancyRate { get; set; }
    }
} 