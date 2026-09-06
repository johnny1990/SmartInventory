using MediatR;
using SmartInventory.Application.DTOs;

namespace SmartInventory.Application.Commands
{
    public class RefreshTokenCommand : IRequest<LoginResponse>
    {
        public string RefreshToken { get; set; } = string.Empty;

        public RefreshTokenCommand(string refreshToken)
        {
            RefreshToken = refreshToken;
        }
    }
}