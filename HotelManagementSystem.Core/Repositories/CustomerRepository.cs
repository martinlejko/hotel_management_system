using HotelManagementSystem.Core.Data;
using HotelManagementSystem.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelManagementSystem.Core.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(HotelDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Customer>> GetCustomersWithReservationsAsync()
        {
            return await _context.Customers
                .Include(c => c.Reservations)
                .ThenInclude(r => r.Room)
                .ToListAsync();
        }

        public async Task<Customer?> GetCustomerWithReservationsAsync(int customerId)
        {
            return await _context.Customers
                .Include(c => c.Reservations)
                .ThenInclude(r => r.Room)
                .FirstOrDefaultAsync(c => c.Id == customerId);
        }
    }
} 