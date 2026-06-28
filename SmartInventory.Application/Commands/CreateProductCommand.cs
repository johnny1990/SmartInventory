using MediatR;

namespace SmartInventory.Application.Commands
{
    public record CreateProductCommand(
    string Name,
    string SKU,
    decimal Price,
    int QuantityInStock,
    Guid CategoryId)
    : IRequest<Guid>;
}
