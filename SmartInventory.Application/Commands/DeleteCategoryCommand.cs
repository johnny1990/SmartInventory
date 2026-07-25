using MediatR;

namespace SmartInventory.Application.Commands
{
    public record DeleteCategoryCommand(
       Guid Id
   ) : IRequest<bool>;
}
