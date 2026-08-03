using Microsoft.EntityFrameworkCore;
using SmartInventory.Domain.Entities;
using SmartInventory.Infrastructure.Common;
using SmartInventory.Infrastructure.Interfaces;
using SmartInventory.Infrastructure.Persistence;

namespace SmartInventory.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly SmartInventoryDbContext _context;

        public ProductRepository(SmartInventoryDbContext context)
        {
            _context = context;
        }

        public async Task<Product> AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            return product;
        }

        public async Task<(List<Product> Products, int TotalCount)> GetAllAsync(ProductSearchParameters parameters)
        {
            var query = _context.Products
                .Include(x => x.Category)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(parameters.Search))
            {
                query = query.Where(x =>
                    x.Name.Contains(parameters.Search) ||
                    x.SKU.Contains(parameters.Search));
            }

            if (parameters.CategoryId.HasValue)
            {
                query = query.Where(x =>
                    x.CategoryId == parameters.CategoryId.Value);
            }

            if (parameters.MinPrice.HasValue)
            {
                query = query.Where(x =>
                    x.Price >= parameters.MinPrice.Value);
            }

            if (parameters.MaxPrice.HasValue)
            {
                query = query.Where(x =>
                    x.Price <= parameters.MaxPrice.Value);
            }

            query = parameters.SortBy?.ToLower() switch
            {
                "price" when parameters.Descending =>
                    query.OrderByDescending(x => x.Price),

                "price" =>
                    query.OrderBy(x => x.Price),

                "name" when parameters.Descending =>
                    query.OrderByDescending(x => x.Name),

                "name" =>
                    query.OrderBy(x => x.Name),

                _ => query.OrderBy(x => x.Name)
            };

            var totalCount = await query.CountAsync();

            var products = await query
                .Skip((parameters.Page - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            return (products, totalCount);
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            return await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
