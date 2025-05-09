using HotelManagementSystem.Core.Models;
using HotelManagementSystem.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagementSystem.Core.Services
{
    public class HotelStatsService : IHotelStatsService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IReservationRepository _reservationRepository;

        public HotelStatsService(IRoomRepository roomRepository, IReservationRepository reservationRepository)
        {
            _roomRepository = roomRepository;
            _reservationRepository = reservationRepository;
        }

        public async Task<OccupancyStats> GetCurrentOccupancyAsync()
        {
            var today = DateTime.Today;
            return await GetOccupancyStatsAsync(today, today.AddDays(1));
        }

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

        public async Task<RevenueStats> GetRevenueStatsAsync(DateTime startDate, DateTime endDate)
        {
            var reservations = await _reservationRepository.GetReservationsByDateRangeAsync(startDate, endDate);
            
            var totalRevenue = reservations.Sum(r => r.TotalPrice);
            var confirmedRevenue = reservations
                .Where(r => r.Status == ReservationStatus.Confirmed || r.Status == ReservationStatus.CheckedIn || r.Status == ReservationStatus.CheckedOut)
                .Sum(r => r.TotalPrice);
            
            return new RevenueStats
            {
                StartDate = startDate,
                EndDate = endDate,
                TotalRevenue = totalRevenue,
                ConfirmedRevenue = confirmedRevenue
            };
        }
    }

    public interface IHotelStatsService
    {
        Task<OccupancyStats> GetCurrentOccupancyAsync();
        Task<OccupancyStats> GetOccupancyStatsAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<OccupancyStats>> GetOccupancyStatsForDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<RevenueStats> GetRevenueStatsAsync(DateTime startDate, DateTime endDate);
    }

    public class OccupancyStats
    {
        public DateTime Date { get; set; }
        public int TotalRooms { get; set; }
        public int OccupiedRooms { get; set; }
        public int AvailableRooms { get; set; }
        public double OccupancyRate { get; set; }
    }

    public class RevenueStats
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal ConfirmedRevenue { get; set; }
    }
} 