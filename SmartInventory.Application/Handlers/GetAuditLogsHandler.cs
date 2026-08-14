using MediatR;
using SmartInventory.Application.DTOs;
using SmartInventory.Application.Queries;
using SmartInventory.Infrastructure.Common;
using SmartInventory.Infrastructure.Interfaces;

namespace SmartInventory.Application.Handlers
{
    public class GetAuditLogsHandler
        : IRequestHandler<
            GetAuditLogsQuery,
            PagedResult<AuditLogDto>>
    {
        private readonly IAuditRepository _repository;

        public GetAuditLogsHandler(
            IAuditRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<AuditLogDto>> Handle(
            GetAuditLogsQuery request,
            CancellationToken cancellationToken)
        {
            var (logs, totalCount) =
                await _repository.GetAllAsync(
                    request.SearchParameters);

            var items = logs.Select(x => new AuditLogDto
            {
                Id = x.Id,
                Action = x.Action,
                EntityName = x.EntityName,
                UserName = x.UserName,
                Changes = x.Changes,
                CreatedAt = x.CreatedAt
            }).ToList();

            return new PagedResult<AuditLogDto>
            {
                Items = items,
                Page = request.SearchParameters.Page,
                PageSize = request.SearchParameters.PageSize,
                TotalItems = totalCount,
                TotalPages = (int)Math.Ceiling(
                    totalCount /
                    (double)request.SearchParameters.PageSize)
            };
        }
    }
}