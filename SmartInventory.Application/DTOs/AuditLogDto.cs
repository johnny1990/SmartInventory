namespace SmartInventory.Application.DTOs
{
    public class AuditLogDto
    {
        public Guid Id { get; set; }

        public string Action { get; set; } = string.Empty;

        public string EntityName { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string? Changes { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
