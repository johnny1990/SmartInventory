namespace SmartInventory.Infrastructure.Common
{
    public class AuditLogSearchParameters
    {
        public string? Search { get; set; }

        public string? Action { get; set; }

        public string? EntityName { get; set; }

        public string? UserName { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string? SortBy { get; set; }

        public bool Descending { get; set; }
    }
}
