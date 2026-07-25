using MediatR;

namespace SmartInventory.Application.Commands
{
    public record CreateCategoryCommand(
        string Name,
        string? Description
    ) : IRequest<Guid>;
}
