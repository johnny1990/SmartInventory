using MediatR;
using SmartInventory.Application.DTOs;

namespace SmartInventory.Application.Commands
{
    public record LoginCommand(
    string Username,
    string Password)
    : IRequest<LoginResponse>;
}
