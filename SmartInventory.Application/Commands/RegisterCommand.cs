using MediatR;

namespace SmartInventory.Application.Commands
{
    public record RegisterCommand(
    string Username,
    string Email,
    string Password)
    : IRequest<Guid>;
}
