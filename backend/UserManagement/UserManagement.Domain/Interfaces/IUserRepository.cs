using UserManagement.Domain.Entities;

namespace UserManagement.Domain.Interfaces
{
    public interface IUserRepository
    {
        //Task<IEnumerable<User>> GetAllAsync();
        IQueryable<User> GetAll();
        Task<User?> GetByIdAsync(int id);
        Task<User> AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
    }
}
