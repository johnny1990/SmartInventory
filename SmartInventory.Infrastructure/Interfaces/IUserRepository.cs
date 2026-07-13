using SmartInventory.Domain.Entities;

namespace SmartInventory.Infrastructure.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);

        Task<User?> GetByEmailAsync(string email);

        Task AddAsync(User user);
    }
}
