using Microsoft.EntityFrameworkCore;
using SmartInventory.Domain.Entities;

namespace SmartInventory.Infrastructure.Persistence;

public class SmartInventoryDbContext : DbContext
{
    public SmartInventoryDbContext(
        DbContextOptions<SmartInventoryDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();

    public DbSet<Category> Categories => Set<Category>();

    public DbSet<StockMovement> StockMovements => Set<StockMovement>();

    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(SmartInventoryDbContext).Assembly);
    }
}