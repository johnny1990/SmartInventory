using MediatR;

namespace SmartInventory.Application.Commands
{
    public record UpdateCategoryCommand(
        Guid Id,
        string Name,
        string? Description
    ) : IRequest<bool>;
}
