using MediatR;
using SmartInventory.Application.Commands;
using SmartInventory.Application.DTOs;
using SmartInventory.Domain.Entities;
using SmartInventory.Infrastructure.Interfaces;

namespace SmartInventory.Application.Handlers
{
    public class LoginHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IUserRepository _repository;
        private readonly IPasswordHasher _hasher;
        private readonly IJwtTokenGenerator _jwt;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IUnitOfWork _unitOfWork;

        public LoginHandler(
            IUserRepository repository,
            IPasswordHasher hasher,
            IJwtTokenGenerator jwt,
            IRefreshTokenRepository refreshTokenRepository,
            IRefreshTokenService refreshTokenService,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _hasher = hasher;
            _jwt = jwt;
            _refreshTokenRepository = refreshTokenRepository;
            _refreshTokenService = refreshTokenService;
            _unitOfWork = unitOfWork;
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

            // Generate access token
            var (accessToken, expiresAt) = _jwt.GenerateToken(user);

            // Generate refresh token
            var refreshTokenValue =
                _refreshTokenService.GenerateToken();

            var refreshToken = new RefreshToken
            {
                UserId = user.Id,
                Token = refreshTokenValue,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow
            };

            await _refreshTokenRepository.AddAsync(
                refreshToken);

            await _unitOfWork.SaveChangesAsync();

            return new LoginResponse
            {
                Token = accessToken,
                RefreshToken = refreshTokenValue,
                ExpiresAt = expiresAt
            };
        }
    }
}