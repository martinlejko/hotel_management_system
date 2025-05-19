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
    /// Repository implementation for Room entity operations.
    /// Inherits from the generic <see cref="Repository{T}"/> class.
    /// </summary>
    public class RoomRepository : Repository<Room>, IRoomRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoomRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public RoomRepository(HotelDbContext context) : base(context)
        {
        }
    }
} 