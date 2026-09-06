using System.Security.Cryptography;
using SmartInventory.Infrastructure.Interfaces;

namespace SmartInventory.Infrastructure.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        public string GenerateToken()
        {
            var randomBytes = RandomNumberGenerator.GetBytes(64);

            return Convert.ToBase64String(randomBytes);
        }
    }
}