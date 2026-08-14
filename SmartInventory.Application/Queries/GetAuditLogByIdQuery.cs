using MediatR;
using SmartInventory.Application.DTOs;

namespace SmartInventory.Application.Queries
{
    public record GetAuditLogByIdQuery(Guid Id)
        : IRequest<AuditLogDto?>;
}