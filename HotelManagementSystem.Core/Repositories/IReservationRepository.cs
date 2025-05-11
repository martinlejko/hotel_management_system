using HotelManagementSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelManagementSystem.Core.Repositories
{
    public interface IReservationRepository : IRepository<Reservation>
    {
        Task<IEnumerable<Reservation>> GetAllReservationsWithDetailsAsync();
        Task<IEnumerable<Reservation>> GetReservationsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<int>> GetBookedRoomIdsAsync(DateTime checkIn, DateTime checkOut, int excludeReservationId = 0);
        Task<bool> HasReservationsForRoomAsync(int roomId);
        Task<bool> HasReservationsForCustomerAsync(int customerId);
    }
} 