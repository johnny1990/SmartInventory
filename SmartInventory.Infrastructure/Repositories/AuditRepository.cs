using Microsoft.EntityFrameworkCore;
using SmartInventory.Domain.Entities;
using SmartInventory.Infrastructure.Common;
using SmartInventory.Infrastructure.Interfaces;
using SmartInventory.Infrastructure.Persistence;

namespace SmartInventory.Infrastructure.Repositories
{
    public class AuditRepository : IAuditRepository
    {
        private readonly SmartInventoryDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public AuditRepository(
            SmartInventoryDbContext context,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task LogAsync(
            string action,
            string entityName,
            string? changes = null)
        {
            var log = new AuditLog
            {
                Id = Guid.NewGuid(),
                Action = action,
                EntityName = entityName,
                UserName = _currentUserService.IsAuthenticated
                    ? _currentUserService.UserName!
                    : "System",
                Changes = changes,
                CreatedAt = DateTime.UtcNow
            };

            await _context.AuditLogs.AddAsync(log);
        }

        public async Task<(List<AuditLog> Logs, int TotalCount)> GetAllAsync(
            AuditLogSearchParameters parameters)
        {
            var query = _context.AuditLogs
                .AsNoTracking()
                .AsQueryable();

            // General search
            if (!string.IsNullOrWhiteSpace(parameters.Search))
            {
                query = query.Where(x =>
                    x.Action.Contains(parameters.Search) ||
                    x.EntityName.Contains(parameters.Search) ||
                    x.UserName.Contains(parameters.Search) ||
                    (x.Changes != null &&
                     x.Changes.Contains(parameters.Search)));
            }

            // Filter by action
            if (!string.IsNullOrWhiteSpace(parameters.Action))
            {
                query = query.Where(x =>
                    x.Action == parameters.Action);
            }

            // Filter by entity
            if (!string.IsNullOrWhiteSpace(parameters.EntityName))
            {
                query = query.Where(x =>
                    x.EntityName == parameters.EntityName);
            }

            // Filter by user
            if (!string.IsNullOrWhiteSpace(parameters.UserName))
            {
                query = query.Where(x =>
                    x.UserName == parameters.UserName);
            }

            // Sorting
            query = parameters.SortBy?.ToLower() switch
            {
                "action" when parameters.Descending =>
                    query.OrderByDescending(x => x.Action),

                "action" =>
                    query.OrderBy(x => x.Action),

                "entityname" when parameters.Descending =>
                    query.OrderByDescending(x => x.EntityName),

                "entityname" =>
                    query.OrderBy(x => x.EntityName),

                "username" when parameters.Descending =>
                    query.OrderByDescending(x => x.UserName),

                "username" =>
                    query.OrderBy(x => x.UserName),

                "createdat" when parameters.Descending =>
                    query.OrderByDescending(x => x.CreatedAt),

                _ =>
                    query.OrderByDescending(x => x.CreatedAt)
            };

            // Count BEFORE pagination
            var totalCount = await query.CountAsync();

            // Pagination
            var logs = await query
                .Skip((parameters.Page - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            return (logs, totalCount);
        }

        public async Task<AuditLog?> GetByIdAsync(Guid id)
        {
            return await _context.AuditLogs
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}