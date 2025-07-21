using Microsoft.EntityFrameworkCore;
using UserManagement.Database.Data;
using UserManagement.Domain.Entities;
using UserManagement.Infrastructure.Repositories;

namespace UserManagement.Tests.Repositories
{
    public class UserRepositoryTests
    {
        private async Task<AppDbContext> GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "UserTestDb")
                .Options;

            var context = new AppDbContext(options);
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            return context;
        }

        [Fact]
        public async Task AddUser_ShouldAddUserSuccessfully()
        {
            // Arrange
            var context = await GetInMemoryDbContext();
            var repo = new UserRepository(context);

            var user = new User
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com"
            };

            await repo.AddAsync(user);
            await context.SaveChangesAsync();

            var userInDb = await context.Users.FirstOrDefaultAsync(u => u.Email == "john.doe@example.com");
            Assert.NotNull(userInDb);
            Assert.Equal("John", userInDb.FirstName);
        }

        [Fact]
        public async Task GetUserById_ShouldReturnCorrectUser()
        {
            var context = await GetInMemoryDbContext();
            var repo = new UserRepository(context);

            var user = new User
            {
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@example.com"
            };
            await repo.AddAsync(user);
            await context.SaveChangesAsync();

            var retrievedUser = await repo.GetByIdAsync(user.Id);

            Assert.NotNull(retrievedUser);
            Assert.Equal("Jane", retrievedUser.FirstName);
        }

        [Fact]
        public async Task UpdateUser_ShouldModifyUserDetails()
        {
            var context = await GetInMemoryDbContext();
            var repo = new UserRepository(context);

            var user = new User
            {
                FirstName = "Alice",
                LastName = "Johnson",
                Email = "alice.johnson@example.com"
            };
            await repo.AddAsync(user);
            await context.SaveChangesAsync();

            user.FirstName = "AliceUpdated";
            user.Email = "alice.updated@example.com";

            await repo.UpdateAsync(user);
            await context.SaveChangesAsync();

            var updatedUser = await context.Users.FindAsync(user.Id);
            Assert.Equal("AliceUpdated", updatedUser.FirstName);
            Assert.Equal("alice.updated@example.com", updatedUser.Email);
        }

        [Fact]
        public async Task DeleteUser_ShouldRemoveUser()
        {
            var context = await GetInMemoryDbContext();
            var repo = new UserRepository(context);

            var user = new User
            {
                FirstName = "Bob",
                LastName = "Brown",
                Email = "bob.brown@example.com"
            };
            await repo.AddAsync(user);
            await context.SaveChangesAsync();

            await repo.DeleteAsync(user.Id);
            await context.SaveChangesAsync();

            var deletedUser = await context.Users.FindAsync(user.Id);
            Assert.Null(deletedUser);
        }

    }
}
