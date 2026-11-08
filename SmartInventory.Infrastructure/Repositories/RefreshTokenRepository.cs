using Microsoft.EntityFrameworkCore;
using SmartInventory.Domain.Entities;
using SmartInventory.Infrastructure.Interfaces;
using SmartInventory.Infrastructure.Persistence;

namespace SmartInventory.Infrastructure.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly SmartInventoryDbContext _context;

        public RefreshTokenRepository(
            SmartInventoryDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(RefreshToken refreshToken)
        {
            await _context.RefreshTokens.AddAsync(refreshToken);
        }

        //Commented provisory for now!
        //public async Task<RefreshToken?> GetByTokenAsync(string token)
        //{
        //    return await _context.RefreshTokens
        //        .Include(x => x.User)
        //        .FirstOrDefaultAsync(x => x.Token == token);
        //}


        //public async Task<RefreshToken?> GetByTokenAsync(string token)
        //{
        //    var connection = _context.Database.GetDbConnection();

        //    Console.WriteLine("=== REFRESH TOKEN DIAGNOSTICS ===");
        //    Console.WriteLine($"Database: {connection.Database}");
        //    Console.WriteLine($"Data source: {connection.DataSource}");

        //    var count = await _context.RefreshTokens.CountAsync();

        //    Console.WriteLine($"Refresh token rows visible to EF: {count}");

        //    var exists = await _context.RefreshTokens
        //        .AnyAsync(x => x.Token == token);

        //    Console.WriteLine($"Exact token exists according to EF: {exists}");

        //    var result = await _context.RefreshTokens
        //        .Include(x => x.User)
        //        .FirstOrDefaultAsync(x => x.Token == token);

        //    Console.WriteLine($"Token lookup returned a result: {result != null}");
        //    Console.WriteLine("=== END DIAGNOSTICS ===");

        //    return result;
        //}


        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            var exists = await _context.RefreshTokens
                .AnyAsync(x => x.Token == token);

            Console.WriteLine($"Exact token exists according to EF: {exists}");

            // Test 1: Find the refresh token without loading User.
            var tokenOnly = await _context.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == token);

            Console.WriteLine(
                $"Lookup without Include returned a result: {tokenOnly != null}");

            // Test 2: Find the refresh token and load User.
            var tokenWithUser = await _context.RefreshTokens
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Token == token);

            Console.WriteLine(
                $"Lookup with Include returned a result: {tokenWithUser != null}");

            return tokenWithUser;
        }


        public async Task RevokeAsync(RefreshToken refreshToken)
        {
            refreshToken.RevokedAt = DateTime.UtcNow;

            await Task.CompletedTask;
        }
    }
}