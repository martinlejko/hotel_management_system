using HotelManagementSystem.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagementSystem.Core.Data
{
    public class DataSeeder
    {
        private readonly HotelDbContext _context;

        public DataSeeder(HotelDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            if (await _context.Rooms.AnyAsync())
            {
                return; 
            }

            var rooms = new[]
            {
                new Room { RoomNumber = "101", Type = RoomType.Single, Capacity = 1, PricePerNight = 80, Description = "Standard single room", IsAvailable = true },
                new Room { RoomNumber = "102", Type = RoomType.Double, Capacity = 2, PricePerNight = 120, Description = "Standard double room", IsAvailable = true },
                new Room { RoomNumber = "103", Type = RoomType.Twin, Capacity = 2, PricePerNight = 120, Description = "Twin room with two single beds", IsAvailable = true },
                new Room { RoomNumber = "201", Type = RoomType.Suite, Capacity = 3, PricePerNight = 200, Description = "Luxury suite with separate living area", IsAvailable = true },
                new Room { RoomNumber = "202", Type = RoomType.Family, Capacity = 4, PricePerNight = 180, Description = "Family room with double bed and bunk beds", IsAvailable = true }
            };

            await _context.Rooms.AddRangeAsync(rooms);
            await _context.SaveChangesAsync();

            var customers = new[]
            {
                new Customer { FirstName = "Jozko", LastName = "Mrkvicka", Email = "jozino@mrkva.sk", PhoneNumber = "+421944123456", Address = "Pod Jablonom 23", CreatedAt = DateTime.Now },
                new Customer { FirstName = "Alena", LastName = "Bodnata", Email = "alena.bodnata@emal.cz", PhoneNumber = "+420944123456", Address = "Nad Hranicou 12", CreatedAt = DateTime.Now }
            };

            await _context.Customers.AddRangeAsync(customers);
            await _context.SaveChangesAsync();

            var reservations = new[]
            {
                new Reservation
                {
                    CustomerId = 1,
                    RoomId = 1,
                    CheckInDate = DateTime.Now.Date,
                    CheckOutDate = DateTime.Now.Date.AddDays(3),
                    Status = ReservationStatus.Confirmed,
                    TotalPrice = 240m,
                    CreatedAt = DateTime.Now,
                    SpecialRequests = "Late check-in"
                },
                new Reservation
                {
                    CustomerId = 2,
                    RoomId = 4,
                    CheckInDate = DateTime.Now.Date.AddDays(1),
                    CheckOutDate = DateTime.Now.Date.AddDays(5),
                    Status = ReservationStatus.Confirmed,
                    TotalPrice = 800m,
                    CreatedAt = DateTime.Now,
                    SpecialRequests = "Early check-in, extra pillows"
                }
            };

            await _context.Reservations.AddRangeAsync(reservations);
            await _context.SaveChangesAsync();
        }
    }
} 