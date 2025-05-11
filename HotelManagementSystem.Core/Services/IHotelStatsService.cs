using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelManagementSystem.Core.Services
{
    public interface IHotelStatsService
    {
        Task<OccupancyStats> GetCurrentOccupancyAsync();
        Task<OccupancyStats> GetOccupancyStatsAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<OccupancyStats>> GetOccupancyStatsForDateRangeAsync(DateTime startDate, DateTime endDate);
    }
} 