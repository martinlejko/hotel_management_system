using HotelManagementSystem.Core.Models;
using HotelManagementSystem.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagementSystem.Core.Services
{
    /// <summary>
    /// Service that provides hotel statistics and occupancy data.
    /// Implements the <see cref="IHotelStatsService"/> interface.
    /// </summary>
    public class HotelStatsService : IHotelStatsService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IReservationRepository _reservationRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="HotelStatsService"/> class.
        /// </summary>
        /// <param name="roomRepository">The repository for accessing room data.</param>
        /// <param name="reservationRepository">The repository for accessing reservation data.</param>
        public HotelStatsService(IRoomRepository roomRepository, IReservationRepository reservationRepository)
        {
            _roomRepository = roomRepository;
            _reservationRepository = reservationRepository;
        }

        /// <inheritdoc/>
        public async Task<OccupancyStats> GetCurrentOccupancyAsync()
        {
            var today = DateTime.Today;
            return await GetOccupancyStatsAsync(today, today.AddDays(1));
        }

        /// <inheritdoc/>
        public async Task<OccupancyStats> GetOccupancyStatsAsync(DateTime startDate, DateTime endDate)
        {
            var allRooms = await _roomRepository.GetAllAsync();
            var allReservations = await _reservationRepository.GetReservationsByDateRangeAsync(startDate, endDate);
            
            var totalRooms = allRooms.Count();
            var occupiedRooms = allReservations
                .Where(r => r.Status == ReservationStatus.Confirmed || r.Status == ReservationStatus.CheckedIn)
                .Select(r => r.RoomId)
                .Distinct()
                .Count();
            
            var availableRooms = totalRooms - occupiedRooms;
            
            return new OccupancyStats
            {
                Date = startDate,
                TotalRooms = totalRooms,
                OccupiedRooms = occupiedRooms,
                AvailableRooms = availableRooms,
                OccupancyRate = totalRooms > 0 ? (double)occupiedRooms / totalRooms : 0
            };
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<OccupancyStats>> GetOccupancyStatsForDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var result = new List<OccupancyStats>();
            
            for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
            {
                var stats = await GetOccupancyStatsAsync(date, date.AddDays(1));
                result.Add(stats);
            }
            
            return result;
        }
    }
} 