using HotelManagementSystem.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelManagementSystem.Core.Repositories
{
    /// <summary>
    /// Repository interface for Customer entity operations.
    /// Extends the generic <see cref="IRepository{T}"/> interface.
    /// </summary>
    public interface ICustomerRepository : IRepository<Customer>
    {
    }
} 