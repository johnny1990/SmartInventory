using SmartInventory.Domain.Entities;

namespace SmartInventory.Infrastructure.Interfaces
{
    public interface IJwtTokenGenerator
    {
        (string Token, DateTime ExpiresAt) GenerateToken(User user);
    }
}
