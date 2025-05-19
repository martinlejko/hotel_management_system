using HotelManagementSystem.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace HotelManagementSystem.Core.Data
{
    /// <summary>
    /// Database context for the Hotel Management System.
    /// Provides access to the database through Entity Framework Core.
    /// </summary>
    public class HotelDbContext : DbContext
    {
        /// <summary>
        /// Gets or sets the Customers table in the database.
        /// </summary>
        public DbSet<Customer> Customers { get; set; }
        
        /// <summary>
        /// Gets or sets the Rooms table in the database.
        /// </summary>
        public DbSet<Room> Rooms { get; set; }
        
        /// <summary>
        /// Gets or sets the Reservations table in the database.
        /// </summary>
        public DbSet<Reservation> Reservations { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HotelDbContext"/> class.
        /// </summary>
        /// <param name="options">The options to be used by the DbContext.</param>
        public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Configures the model for the database.
        /// Establishes relationships between entities and sets up configuration for each entity.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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
        }
    }
} 