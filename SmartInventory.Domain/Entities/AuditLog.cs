using SmartInventory.Domain.Common;

namespace SmartInventory.Domain.Entities;

public class AuditLog : BaseEntity
{
    public string Action { get; set; } = string.Empty;

    public string EntityName { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public string? Changes { get; set; }
}