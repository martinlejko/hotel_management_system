using HotelManagementSystem.Core.Data;
using HotelManagementSystem.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagementSystem.Core.Repositories
{
    public class RoomRepository : Repository<Room>, IRoomRepository
    {
        public RoomRepository(HotelDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Room>> GetRoomsWithReservationsAsync()
        {
            return await _context.Rooms
                .Include(r => r.Reservations)
                .ThenInclude(r => r.Customer)
                .ToListAsync();
        }

        public async Task<Room?> GetRoomWithReservationsAsync(int roomId)
        {
            return await _context.Rooms
                .Include(r => r.Reservations)
                .ThenInclude(r => r.Customer)
                .FirstOrDefaultAsync(r => r.Id == roomId);
        }

        public async Task<IEnumerable<Room>> GetAvailableRoomsAsync(DateTime checkIn, DateTime checkOut)
        {
            // Get all rooms
            var allRooms = await _context.Rooms.ToListAsync();
            
            // Get rooms that have overlapping reservations with the specified date range
            var roomsWithOverlappingReservations = await _context.Reservations
                .Where(r => 
                    (r.Status == ReservationStatus.Confirmed || r.Status == ReservationStatus.CheckedIn) &&
                    ((r.CheckInDate <= checkOut && r.CheckOutDate >= checkIn)))
                .Select(r => r.RoomId)
                .Distinct()
                .ToListAsync();

            // Return rooms that don't have overlapping reservations
            return allRooms.Where(r => !roomsWithOverlappingReservations.Contains(r.Id));
        }
    }

    public interface IRoomRepository : IRepository<Room>
    {
        Task<IEnumerable<Room>> GetRoomsWithReservationsAsync();
        Task<Room?> GetRoomWithReservationsAsync(int roomId);
        Task<IEnumerable<Room>> GetAvailableRoomsAsync(DateTime checkIn, DateTime checkOut);
    }
} 