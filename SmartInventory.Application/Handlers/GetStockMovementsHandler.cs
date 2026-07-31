using MediatR;
using SmartInventory.Application.DTOs;
using SmartInventory.Application.Queries;
using SmartInventory.Infrastructure.Interfaces;

namespace SmartInventory.Application.Handlers
{
    public class GetStockMovementsHandler
        : IRequestHandler<GetStockMovementsQuery,
            List<StockMovementDto>>
    {
        private readonly IStockMovementRepository _repository;

        public GetStockMovementsHandler(
            IStockMovementRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<StockMovementDto>> Handle(
            GetStockMovementsQuery request,
            CancellationToken cancellationToken)
        {
            var movements =
                await _repository.GetAllAsync();

            return movements.Select(x => new StockMovementDto
            {
                Id = x.Id,
                ProductId = x.ProductId,
                ProductName = x.Product.Name,
                Quantity = x.Quantity,
                MovementType = x.MovementType.ToString(),
                Notes = x.Notes,
                CreatedAt = x.CreatedAt
            }).ToList();
        }
    }
}
