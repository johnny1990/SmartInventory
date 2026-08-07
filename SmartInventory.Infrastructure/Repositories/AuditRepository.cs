using SmartInventory.Domain.Entities;
using SmartInventory.Infrastructure.Interfaces;
using SmartInventory.Infrastructure.Persistence;

namespace SmartInventory.Infrastructure.Repositories
{
    public class AuditRepository : IAuditRepository
    {
        private readonly SmartInventoryDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public AuditRepository(
            SmartInventoryDbContext context,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task LogAsync(
            string action,
            string entityName,
            string? changes = null)
        {
            var log = new AuditLog
            {
                Id = Guid.NewGuid(),
                Action = action,
                EntityName = entityName,
                UserName = _currentUserService.IsAuthenticated
                    ? _currentUserService.UserName!
                    : "System",
                Changes = changes,
                CreatedAt = DateTime.UtcNow
            };

            _context.AuditLogs.Add(log);

            await _context.SaveChangesAsync();
        }
    }
}