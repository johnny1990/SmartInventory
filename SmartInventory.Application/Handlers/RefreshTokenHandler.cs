using MediatR;
using SmartInventory.Application.Commands;
using SmartInventory.Application.DTOs;
using SmartInventory.Infrastructure.Interfaces;

namespace SmartInventory.Application.Handlers
{
    public class RefreshTokenHandler
        : IRequestHandler<RefreshTokenCommand, LoginResponse>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUnitOfWork _unitOfWork;

        public RefreshTokenHandler(
            IRefreshTokenRepository refreshTokenRepository,
            IRefreshTokenService refreshTokenService,
            IJwtTokenGenerator jwtTokenGenerator,
            IUnitOfWork unitOfWork)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _refreshTokenService = refreshTokenService;
            _jwtTokenGenerator = jwtTokenGenerator;
            _unitOfWork = unitOfWork;
        }

        public async Task<LoginResponse> Handle(
            RefreshTokenCommand request,
            CancellationToken cancellationToken)
        {
            var refreshToken =
                await _refreshTokenRepository.GetByTokenAsync(
                    request.RefreshToken);

            if (refreshToken == null)
                throw new Exception("Invalid refresh token.");

            if (refreshToken.IsRevoked)
                throw new Exception("Refresh token has been revoked.");

            if (refreshToken.IsExpired)
                throw new Exception("Refresh token has expired.");

            var (token, expiresAt) = _jwtTokenGenerator.GenerateToken(refreshToken.User);

            return new LoginResponse
            {
                Token = token,
                RefreshToken = refreshToken.Token,
                ExpiresAt = expiresAt
            };
        }
    }
}