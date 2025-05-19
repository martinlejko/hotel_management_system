using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelManagementSystem.Core.Services
{
    /// <summary>
    /// Service interface for retrieving hotel statistics and occupancy data.
    /// Provides methods to analyze hotel room occupancy rates over time.
    /// </summary>
    public interface IHotelStatsService
    {
        /// <summary>
        /// Gets the current occupancy statistics for the hotel.
        /// </summary>
        /// <returns>The occupancy statistics for the current date.</returns>
        Task<OccupancyStats> GetCurrentOccupancyAsync();
        
        /// <summary>
        /// Gets the occupancy statistics for a specific date range.
        /// </summary>
        /// <param name="startDate">The start date for the period to analyze.</param>
        /// <param name="endDate">The end date for the period to analyze.</param>
        /// <returns>The occupancy statistics for the specified date range.</returns>
        Task<OccupancyStats> GetOccupancyStatsAsync(DateTime startDate, DateTime endDate);
        
        /// <summary>
        /// Gets the daily occupancy statistics for each day within a date range.
        /// </summary>
        /// <param name="startDate">The start date for the period to analyze.</param>
        /// <param name="endDate">The end date for the period to analyze.</param>
        /// <returns>A collection of daily occupancy statistics within the specified date range.</returns>
        Task<IEnumerable<OccupancyStats>> GetOccupancyStatsForDateRangeAsync(DateTime startDate, DateTime endDate);
    }
} 