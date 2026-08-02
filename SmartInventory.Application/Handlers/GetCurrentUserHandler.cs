using MediatR;
using SmartInventory.Application.DTOs;
using SmartInventory.Application.Queries;
using SmartInventory.Infrastructure.Interfaces;

namespace SmartInventory.Application.Handlers
{
    public class GetCurrentUserHandler
        : IRequestHandler<GetCurrentUserQuery, UserDto>
    {
        private readonly IUserRepository _repository;

        public GetCurrentUserHandler(
            IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<UserDto> Handle(
            GetCurrentUserQuery request,
            CancellationToken cancellationToken)
        {
            var user =
                await _repository.GetByIdAsync(request.UserId);

            if (user == null)
                throw new Exception("User not found.");

            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role
            };
        }
    }
}