using HotelManagementSystem.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace HotelManagementSystem.Core.Data
{
    public class HotelDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Customer)
                .WithMany(c => c.Reservations)
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Room)
                .WithMany(r => r.Reservations)
                .HasForeignKey(r => r.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed some sample data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed rooms
            modelBuilder.Entity<Room>().HasData(
                new Room { Id = 1, RoomNumber = "101", Type = RoomType.Single, Capacity = 1, PricePerNight = 80, Description = "Standard single room", IsAvailable = true },
                new Room { Id = 2, RoomNumber = "102", Type = RoomType.Double, Capacity = 2, PricePerNight = 120, Description = "Standard double room", IsAvailable = true },
                new Room { Id = 3, RoomNumber = "103", Type = RoomType.Twin, Capacity = 2, PricePerNight = 120, Description = "Twin room with two single beds", IsAvailable = true },
                new Room { Id = 4, RoomNumber = "201", Type = RoomType.Suite, Capacity = 3, PricePerNight = 200, Description = "Luxury suite with separate living area", IsAvailable = true },
                new Room { Id = 5, RoomNumber = "202", Type = RoomType.Family, Capacity = 4, PricePerNight = 180, Description = "Family room with double bed and bunk beds", IsAvailable = true }
            );

            // Seed customers
            modelBuilder.Entity<Customer>().HasData(
                new { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", PhoneNumber = "123-456-7890", Address = "123 Main St", CreatedAt = DateTime.Now },
                new { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com", PhoneNumber = "987-654-3210", Address = "456 Elm St", CreatedAt = DateTime.Now }
            );

            // Seed reservations
            modelBuilder.Entity<Reservation>().HasData(
                new
                {
                    Id = 1,
                    CustomerId = 1,
                    RoomId = 1,
                    CheckInDate = DateTime.Now.Date,
                    CheckOutDate = DateTime.Now.Date.AddDays(3),
                    Status = ReservationStatus.Confirmed,
                    TotalPrice = 240m, // 3 days * $80
                    CreatedAt = DateTime.Now,
                    SpecialRequests = "Late check-in"
                },
                new
                {
                    Id = 2,
                    CustomerId = 2,
                    RoomId = 4,
                    CheckInDate = DateTime.Now.Date.AddDays(1),
                    CheckOutDate = DateTime.Now.Date.AddDays(5),
                    Status = ReservationStatus.Confirmed,
                    TotalPrice = 800m, // 4 days * $200
                    CreatedAt = DateTime.Now,
                    SpecialRequests = "Early check-in, extra pillows"
                }
            );
        }
    }
} 