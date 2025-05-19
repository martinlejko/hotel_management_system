using HotelManagementSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelManagementSystem.Core.Repositories
{
    /// <summary>
    /// Repository interface for Room entity operations.
    /// Extends the generic <see cref="IRepository{T}"/> interface.
    /// </summary>
    public interface IRoomRepository : IRepository<Room>
    {
    }
} 