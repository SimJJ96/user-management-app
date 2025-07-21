using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using UserManagement.API.Controllers;
using UserManagement.API.DTOs;
using UserManagement.API.Mapping;
using UserManagement.Database.Data;
using UserManagement.Domain.Entities;
using UserManagement.Domain.Interfaces;
using UserManagement.Domain.Services;
using UserManagement.Infrastructure.Repositories;
using Xunit;

namespace UserManagement.Tests.Controllers
{
    public class UsersControllerTests
    {
        private readonly IMapper _mapper;

        public UsersControllerTests()
        {
            var configExpr = new MapperConfigurationExpression();
            configExpr.AddProfile(new UserProfile());

            var loggerFactory = LoggerFactory.Create(builder => { });

            var config = new MapperConfiguration(configExpr, loggerFactory);
            _mapper = config.CreateMapper();
        }

        private AppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // unique DB per test
                .Options;
            return new AppDbContext(options);
        }

        private UsersController CreateController(AppDbContext context)
        {
            var mockCreateValidator = new Mock<IValidator<CreateUserDto>>();
            mockCreateValidator.Setup(v => v.ValidateAsync(It.IsAny<CreateUserDto>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(new ValidationResult());

            var mockUpdateValidator = new Mock<IValidator<UpdateUserDto>>();
            mockUpdateValidator.Setup(v => v.ValidateAsync(It.IsAny<UpdateUserDto>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(new ValidationResult());

            var userRepository = new UserRepository(context);
            var userService = new UserService(userRepository);

            return new UsersController(context, _mapper, mockCreateValidator.Object, mockUpdateValidator.Object, userService);
        }

        [Fact]
        public async Task GetUsers_ReturnsUsers()
        {
            var userList = new List<UserDto>
            {
                new UserDto { Id = 1, FirstName = "Alice", LastName = "Smith", Email = "alice@example.com" },
                new UserDto { Id = 2, FirstName = "Bob", LastName = "Smith", Email = "bob@example.com" }
            };

            var pagedResponse = new PageResponseDto<UserDto>
            {
                PageNumber = 1,
                PageSize = 10,
                TotalRecords = userList.Count,
                Data = userList
            };

            using var context = CreateContext();
            context.Users.Add(new User { FirstName = "Alice", LastName = "Smith", Email = "alice@example.com" });
            context.Users.Add(new User { FirstName = "Bob", LastName = "Smith", Email = "bob@example.com" });
            await context.SaveChangesAsync();

            var controller = CreateController(context);

            var request = new PageRequestDto
            {
                Search = null,
                SortBy = null,
                PageNumber = 1,
                PageSize = 10
            };

            var result = await controller.GetUsers(request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<PageResponseDto<UserDto>>(okResult.Value);
            var data = response.Data.ToList();


            Assert.Equal(2, response.TotalRecords);
            Assert.Equal(1, response.PageNumber);
            Assert.Equal(10, response.PageSize);
            Assert.Equal(2, response.Data.Count());
            Assert.Contains(data, u => u.FirstName == "Alice" && u.LastName == "Smith");
            Assert.Contains(data, u => u.FirstName == "Bob" && u.LastName == "Smith");
        }

        [Fact]
        public async Task GetUser_ReturnsUser_WhenExists()
        {
            using var context = CreateContext();
            var user = new User { FirstName = "Bob", LastName = "Lee", Email = "bob@example.com" };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var controller = CreateController(context);
            var result = await controller.GetUser(user.Id);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returned = Assert.IsType<UserDto>(okResult.Value);
            Assert.Equal("Bob", returned.FirstName);
        }

        [Fact]
        public async Task GetUser_ReturnsNotFound_WhenNotExists()
        {
            using var context = CreateContext();
            var controller = CreateController(context);

            var result = await controller.GetUser(999);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateUser_AddsUser_AndReturnsCreatedUser()
        {
            using var context = CreateContext();
            var controller = CreateController(context);

            var dto = new CreateUserDto
            {
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane@example.com"
            };

            var result = await controller.CreateUser(dto);

            var createdAt = Assert.IsType<CreatedAtActionResult>(result.Result);
            var createdUser = Assert.IsType<UserDto>(createdAt.Value);
            Assert.Equal("Jane", createdUser.FirstName);
        }

        [Fact]
        public async Task UpdateUser_UpdatesExistingUser()
        {
            using var context = CreateContext();
            var user = new User { FirstName = "Old", LastName = "Name", Email = "old@example.com" };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var controller = CreateController(context);
            var dto = new UpdateUserDto
            {
                FirstName = "New",
                LastName = "Name",
                Email = "new@example.com"
            };

            var result = await controller.UpdateUser(user.Id, dto);
            Assert.IsType<NoContentResult>(result);

            var updated = await context.Users.FindAsync(user.Id);
            Assert.Equal("New", updated!.FirstName);
        }

        [Fact]
        public async Task UpdateUser_ReturnsNotFound_WhenUserMissing()
        {
            using var context = CreateContext();
            var controller = CreateController(context);

            var dto = new UpdateUserDto { FirstName = "X", LastName = "Y", Email = "xy@example.com" };
            var result = await controller.UpdateUser(1234, dto);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteUser_RemovesUser()
        {
            using var context = CreateContext();
            var user = new User { FirstName = "Delete", LastName = "Me", Email = "delete@example.com" };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var controller = CreateController(context);
            var result = await controller.DeleteUser(user.Id);

            Assert.IsType<NoContentResult>(result);
            Assert.Null(await context.Users.FindAsync(user.Id));
        }

        [Fact]
        public async Task DeleteUser_ReturnsNotFound_WhenUserMissing()
        {
            using var context = CreateContext();
            var controller = CreateController(context);

            var result = await controller.DeleteUser(404);
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
