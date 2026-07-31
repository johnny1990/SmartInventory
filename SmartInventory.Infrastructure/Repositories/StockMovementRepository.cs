using Microsoft.EntityFrameworkCore;
using SmartInventory.Domain.Entities;
using SmartInventory.Infrastructure.Interfaces;
using SmartInventory.Infrastructure.Persistence;

namespace SmartInventory.Infrastructure.Repositories
{
    public class StockMovementRepository : IStockMovementRepository
    {
        private readonly SmartInventoryDbContext _context;

        public StockMovementRepository(
            SmartInventoryDbContext context)
        {
            _context = context;
        }

        public async Task<List<StockMovement>> GetAllAsync()
        {
            return await _context.StockMovements
                .Include(x => x.Product)
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task AddAsync(StockMovement movement)
        {
            await _context.StockMovements.AddAsync(movement);
        }
    }
}
