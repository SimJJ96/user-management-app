using UserManagement.Domain.Entities;

namespace UserManagement.Domain.Interfaces
{
    public interface IUserService
    {
        Task<PagedResult<User>> GetUsersAsync(string? search, string? sortBy, int page, int pageSize);
        Task<User?> GetByIdAsync(int id);
        Task<User> CreateAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
    }
}
