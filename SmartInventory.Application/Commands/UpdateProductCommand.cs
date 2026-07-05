using MediatR;

namespace SmartInventory.Application.Commands
{
    public record UpdateProductCommand(
    Guid Id,
    string Name,
    string SKU,
    decimal Price,
    int QuantityInStock)
    : IRequest<bool>;
}
