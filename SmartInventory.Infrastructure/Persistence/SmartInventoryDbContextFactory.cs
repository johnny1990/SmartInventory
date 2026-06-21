using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SmartInventory.Infrastructure.Persistence;

public class SmartInventoryDbContextFactory
    : IDesignTimeDbContextFactory<SmartInventoryDbContext>
{
    public SmartInventoryDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder =
            new DbContextOptionsBuilder<SmartInventoryDbContext>();

        optionsBuilder.UseSqlServer(
            "Server=MAN\\SQLEXPRESS;Database=SmartInventoryDb;Trusted_Connection=True;TrustServerCertificate=True;");

        return new SmartInventoryDbContext(
            optionsBuilder.Options);
    }
}