using SmartInventory.Domain.Entities;

namespace SmartInventory.Infrastructure.Interfaces
{
    public interface IStockMovementRepository
    {
        Task<List<StockMovement>> GetAllAsync();

        Task AddAsync(StockMovement movement);
    }
}
