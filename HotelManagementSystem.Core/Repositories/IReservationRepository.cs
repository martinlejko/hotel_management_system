using HotelManagementSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelManagementSystem.Core.Repositories
{
    /// <summary>
    /// Repository interface for Reservation entity operations.
    /// Extends the generic <see cref="IRepository{T}"/> interface with reservation-specific methods.
    /// </summary>
    public interface IReservationRepository : IRepository<Reservation>
    {
        /// <summary>
        /// Retrieves all reservations with their associated Customer and Room details included.
        /// </summary>
        /// <returns>A collection of Reservation entities with navigation properties populated.</returns>
        Task<IEnumerable<Reservation>> GetAllReservationsWithDetailsAsync();
        
        /// <summary>
        /// Retrieves reservations that fall within the specified date range.
        /// </summary>
        /// <param name="startDate">The start date of the range.</param>
        /// <param name="endDate">The end date of the range.</param>
        /// <returns>A collection of reservations within the date range.</returns>
        Task<IEnumerable<Reservation>> GetReservationsByDateRangeAsync(DateTime startDate, DateTime endDate);
        
        /// <summary>
        /// Gets the IDs of rooms that are already booked for the specified date range.
        /// </summary>
        /// <param name="checkIn">The check-in date.</param>
        /// <param name="checkOut">The check-out date.</param>
        /// <param name="excludeReservationId">Optional reservation ID to exclude from the results.</param>
        /// <returns>A collection of room IDs that are already booked.</returns>
        Task<IEnumerable<int>> GetBookedRoomIdsAsync(DateTime checkIn, DateTime checkOut, int excludeReservationId = 0);
        
        /// <summary>
        /// Checks if a room has any reservations.
        /// </summary>
        /// <param name="roomId">The ID of the room to check.</param>
        /// <returns>True if the room has reservations; otherwise, false.</returns>
        Task<bool> HasReservationsForRoomAsync(int roomId);
        
        /// <summary>
        /// Checks if a customer has any reservations.
        /// </summary>
        /// <param name="customerId">The ID of the customer to check.</param>
        /// <returns>True if the customer has reservations; otherwise, false.</returns>
        Task<bool> HasReservationsForCustomerAsync(int customerId);
    }
} 