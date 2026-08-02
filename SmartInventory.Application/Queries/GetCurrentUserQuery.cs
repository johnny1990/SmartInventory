using MediatR;
using SmartInventory.Application.DTOs;

namespace SmartInventory.Application.Queries
{
    public class GetCurrentUserQuery : IRequest<UserDto>
    {
        public Guid UserId { get; }

        public GetCurrentUserQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}