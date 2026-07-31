using MediatR;
using SmartInventory.Application.DTOs;

namespace SmartInventory.Application.Queries
{
    public record GetStockMovementsQuery()
    : IRequest<List<StockMovementDto>>;
}
