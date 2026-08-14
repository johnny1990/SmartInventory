using SmartInventory.Domain.Entities;
using SmartInventory.Infrastructure.Common;

namespace SmartInventory.Infrastructure.Interfaces
{
    public interface IAuditRepository
    {
        Task LogAsync(
    string action,
    string entityName,
    string? changes = null);

        Task<(List<AuditLog> Logs, int TotalCount)> GetAllAsync(
           AuditLogSearchParameters parameters);

        Task<AuditLog?> GetByIdAsync(Guid id);
    }
}
