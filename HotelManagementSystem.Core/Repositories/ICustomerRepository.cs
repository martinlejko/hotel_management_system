using HotelManagementSystem.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelManagementSystem.Core.Repositories
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<IEnumerable<Customer>> GetCustomersWithReservationsAsync();
        Task<Customer?> GetCustomerWithReservationsAsync(int customerId);
    }
} 