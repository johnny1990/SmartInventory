using SmartInventory.Domain.Common;

namespace SmartInventory.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string SKU { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int QuantityInStock { get; set; }

    public Guid CategoryId { get; set; }

    public Category Category { get; set; } = null!;

    public ICollection<StockMovement> StockMovements { get; set; }
        = new List<StockMovement>();
}