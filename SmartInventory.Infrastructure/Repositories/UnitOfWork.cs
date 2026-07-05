using SmartInventory.Infrastructure.Interfaces;
using SmartInventory.Infrastructure.Persistence;

namespace SmartInventory.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SmartInventoryDbContext _context;

        public UnitOfWork(SmartInventoryDbContext context)
        {
            _context = context;
        }

        public Task<int> SaveChangesAsync()
            => _context.SaveChangesAsync();
    }
}
