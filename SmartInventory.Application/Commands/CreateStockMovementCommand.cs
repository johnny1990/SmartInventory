using MediatR;
using SmartInventory.Domain.Enums;

namespace SmartInventory.Application.Commands
{
    public record CreateStockMovementCommand(
    Guid ProductId,
    int Quantity,
    MovementType MovementType,
    string? Notes
) : IRequest<Guid>;
}
