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
    }
} 