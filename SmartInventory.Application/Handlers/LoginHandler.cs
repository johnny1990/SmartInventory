using MediatR;
using SmartInventory.Application.Commands;
using SmartInventory.Application.DTOs;
using SmartInventory.Infrastructure.Interfaces;

namespace SmartInventory.Application.Handlers
{
    public class LoginHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IUserRepository _repository;

        private readonly IPasswordHasher _hasher;

        private readonly IJwtTokenGenerator _jwt;

        public LoginHandler(
            IUserRepository repository,
            IPasswordHasher hasher,
            IJwtTokenGenerator jwt)
        {
            _repository = repository;

            _hasher = hasher;

            _jwt = jwt;
        }

        public async Task<LoginResponse> Handle(
            LoginCommand request,
            CancellationToken cancellationToken)
        {
            var user =
                await _repository.GetByUsernameAsync(
                    request.Username);

            if (user == null)
                throw new Exception(
                    "Invalid credentials.");

            var valid =
                _hasher.VerifyPassword(
                    request.Password,
                    user.PasswordHash);

            if (!valid)
                throw new Exception(
                    "Invalid credentials.");

            var token =
                _jwt.GenerateToken(user);

            return new LoginResponse
            {
                Token = token
            };
        }
    }
}