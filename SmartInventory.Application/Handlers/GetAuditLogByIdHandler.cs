using MediatR;
using SmartInventory.Application.DTOs;
using SmartInventory.Application.Queries;
using SmartInventory.Infrastructure.Interfaces;

namespace SmartInventory.Application.Handlers
{
    public class GetAuditLogByIdHandler
        : IRequestHandler<GetAuditLogByIdQuery, AuditLogDto?>
    {
        private readonly IAuditRepository _repository;

        public GetAuditLogByIdHandler(
            IAuditRepository repository)
        {
            _repository = repository;
        }

        public async Task<AuditLogDto?> Handle(
            GetAuditLogByIdQuery request,
            CancellationToken cancellationToken)
        {
            var log =
                await _repository.GetByIdAsync(request.Id);

            if (log == null)
                return null;

            return new AuditLogDto
            {
                Id = log.Id,
                Action = log.Action,
                EntityName = log.EntityName,
                UserName = log.UserName,
                Changes = log.Changes,
                CreatedAt = log.CreatedAt
            };
        }
    }
}