using HotelManagementSystem.Core.Data;
using HotelManagementSystem.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagementSystem.Core.Repositories
{
    /// <summary>
    /// Repository implementation for Reservation entity operations.
    /// Inherits from the generic <see cref="Repository{T}"/> class and implements the <see cref="IReservationRepository"/> interface.
    /// </summary>
    public class ReservationRepository : Repository<Reservation>, IReservationRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReservationRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public ReservationRepository(HotelDbContext context) : base(context)
        {
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Reservation>> GetAllReservationsWithDetailsAsync()
        {
            return await _context.Reservations
                .Include(r => r.Customer)
                .Include(r => r.Room)
                .OrderByDescending(r => r.CheckInDate)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Reservation>> GetReservationsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Reservations
                .Include(r => r.Customer)
                .Include(r => r.Room)
                .Where(r => 
                    (r.CheckInDate >= startDate && r.CheckInDate <= endDate) || 
                    (r.CheckOutDate >= startDate && r.CheckOutDate <= endDate) ||
                    (r.CheckInDate <= startDate && r.CheckOutDate >= endDate))
                .OrderByDescending(r => r.CheckInDate)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<int>> GetBookedRoomIdsAsync(DateTime checkIn, DateTime checkOut, int excludeReservationId = 0)
        {
            return await _context.Reservations
                .Where(r => 
                    r.Id != excludeReservationId &&
                    (r.Status == ReservationStatus.Confirmed || r.Status == ReservationStatus.CheckedIn) &&
                    r.CheckInDate < checkOut && r.CheckOutDate > checkIn)
                .Select(r => r.RoomId)
                .Distinct()
                .ToListAsync();
        }
        
        /// <inheritdoc/>
        public async Task<bool> HasReservationsForRoomAsync(int roomId)
        {
            return await _context.Reservations
                .AnyAsync(r => r.RoomId == roomId);
        }
        
        /// <inheritdoc/>
        public async Task<bool> HasReservationsForCustomerAsync(int customerId)
        {
            return await _context.Reservations
                .AnyAsync(r => r.CustomerId == customerId);
        }
    }
} 