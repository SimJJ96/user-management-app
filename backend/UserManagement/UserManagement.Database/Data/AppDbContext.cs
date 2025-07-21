using Microsoft.EntityFrameworkCore;
using UserManagement.Domain.Entities;

namespace UserManagement.Database.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply unique constraint to Email column
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Seed 5 dummy users
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    FirstName = "Alice",
                    LastName = "Smith",
                    Email = "alice.smith@example.com",
                    PhoneNumber = "1234567890",
                    ZipCode = "12345"
                },
                new User
                {
                    Id = 2,
                    FirstName = "Bob",
                    LastName = "Johnson",
                    Email = "bob.johnson@example.com",
                    PhoneNumber = "2345678901",
                    ZipCode = "23456"
                },
                new User
                {
                    Id = 3,
                    FirstName = "Charlie",
                    LastName = "Williams",
                    Email = "charlie.williams@example.com",
                    PhoneNumber = "3456789012",
                    ZipCode = "34567"
                },
                new User
                {
                    Id = 4,
                    FirstName = "Diana",
                    LastName = "Brown",
                    Email = "diana.brown@example.com",
                    PhoneNumber = "4567890123",
                    ZipCode = "45678"
                },
                new User
                {
                    Id = 5,
                    FirstName = "Ethan",
                    LastName = "Davis",
                    Email = "ethan.davis@example.com",
                    PhoneNumber = "5678901234",
                    ZipCode = "56789"
                }
            );
        }
    }
}
