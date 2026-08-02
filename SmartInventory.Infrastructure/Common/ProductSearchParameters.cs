
namespace SmartInventory.Infrastructure.Common
{
    public class ProductSearchParameters
    {
        public string? Search { get; set; }

        public Guid? CategoryId { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }

        public string? SortBy { get; set; }

        public bool Descending { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
