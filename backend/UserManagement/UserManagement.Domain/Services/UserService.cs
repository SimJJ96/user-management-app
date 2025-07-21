using Microsoft.EntityFrameworkCore;
using UserManagement.Domain.Entities;
using UserManagement.Domain.Interfaces;

namespace UserManagement.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;

        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<PagedResult<User>> GetUsersAsync(string? search, string? sortBy, int page, int pageSize)
        {
            var query = _repo.GetAll();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(u =>
                    u.FirstName.Contains(search) ||
                    u.LastName.Contains(search) ||
                    u.Email.Contains(search));
            }

            query = sortBy?.ToLower() switch
            {
                "email" => query.OrderBy(u => u.Email),
                "firstName" => query.OrderBy(u => u.FirstName),
                "lastName" => query.OrderBy(u => u.LastName),
                _ => query.OrderBy(u => u.Id)
            };

            var totalCount = await query.CountAsync();

            var paginatedUsers = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<User>(paginatedUsers, totalCount);
        }

        public Task<User?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
        public Task<User> CreateAsync(User user) => _repo.AddAsync(user);
        public Task UpdateAsync(User user) => _repo.UpdateAsync(user);
        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
    }
}
