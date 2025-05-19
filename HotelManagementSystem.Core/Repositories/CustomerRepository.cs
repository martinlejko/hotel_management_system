using HotelManagementSystem.Core.Data;
using HotelManagementSystem.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelManagementSystem.Core.Repositories
{
    /// <summary>
    /// Repository implementation for Customer entity operations.
    /// Inherits from the generic <see cref="Repository{T}"/> class.
    /// </summary>
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public CustomerRepository(HotelDbContext context) : base(context)
        {
        }
    }
} 