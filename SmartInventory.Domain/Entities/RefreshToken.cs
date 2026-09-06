using SmartInventory.Domain.Common;
using SmartInventory.Domain.Entities;

namespace SmartInventory.Domain.Entities
{

    public class RefreshToken : BaseEntity
    {
        public Guid UserId { get; set; }

        public string Token { get; set; } = string.Empty;

        public DateTime ExpiresAt { get; set; }

        public DateTime? RevokedAt { get; set; }

        public bool IsRevoked => RevokedAt.HasValue;

        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

        public User User { get; set; } = null!;
    }
}
