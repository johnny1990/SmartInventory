using MediatR;
using SmartInventory.Application.DTOs;
using SmartInventory.Infrastructure.Common;

namespace SmartInventory.Application.Queries
{
    public record GetAuditLogsQuery(
        AuditLogSearchParameters SearchParameters)
        : IRequest<PagedResult<AuditLogDto>>;
}