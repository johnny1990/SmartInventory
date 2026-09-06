using SmartInventory.Domain.Entities;

namespace SmartInventory.Infrastructure.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task AddAsync(RefreshToken refreshToken);

        Task<RefreshToken?> GetByTokenAsync(string token);

        Task RevokeAsync(RefreshToken refreshToken);
    }
}