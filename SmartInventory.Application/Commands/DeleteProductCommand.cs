using MediatR;

namespace SmartInventory.Application.Commands
{
    public record DeleteProductCommand(Guid Id)
    : IRequest<bool>;
}
