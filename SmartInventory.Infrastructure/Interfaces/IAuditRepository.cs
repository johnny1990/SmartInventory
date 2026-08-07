namespace SmartInventory.Infrastructure.Interfaces
{
    public interface IAuditRepository
    {
        Task LogAsync(
    string action,
    string entityName,
    string? changes = null);
    }
}
