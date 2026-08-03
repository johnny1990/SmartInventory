using SmartInventory.Domain.Entities;
using SmartInventory.Infrastructure.Common;

namespace SmartInventory.Infrastructure.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> AddAsync(Product product);

        Task<(List<Product> Products, int TotalCount)> GetAllAsync(ProductSearchParameters parameters);

        Task<Product?> GetByIdAsync(Guid id);

        Task SaveChangesAsync();
    }
}
