using Moq;
using SmartInventory.Application.Commands;
using SmartInventory.Application.Handlers;
using SmartInventory.Domain.Entities;
using SmartInventory.Infrastructure.Interfaces;


namespace SmartInventory.Tests
{
    [TestFixture]
    public class RefreshTokenHandlerTests
    {
        [Test]
        public void Handle_InvalidRefreshToken_ThrowsException()
        {
            // Arrange
            var refreshTokenRepository = new Mock<IRefreshTokenRepository>();
            var refreshTokenService = new Mock<IRefreshTokenService>();
            var jwtTokenGenerator = new Mock<IJwtTokenGenerator>();
            var unitOfWork = new Mock<IUnitOfWork>();

            var handler = new RefreshTokenHandler(
                refreshTokenRepository.Object,
                refreshTokenService.Object,
                jwtTokenGenerator.Object,
                unitOfWork.Object);

            // Act & Assert
            Assert.ThrowsAsync<Exception>(async () =>
                await handler.Handle(new RefreshTokenCommand("invalid"), CancellationToken.None));
        }

        [Test]
        public void Handle_ValidRefreshToken_ReturnsLoginResponse()
        {
            // Arrange
            var refreshTokenRepository = new Mock<IRefreshTokenRepository>();
            var refreshTokenService = new Mock<IRefreshTokenService>();
            var jwtTokenGenerator = new Mock<IJwtTokenGenerator>();
            var unitOfWork = new Mock<IUnitOfWork>();

            // Setup a valid refresh token returned by the repository for token "valid"
            var user = new User { Id = Guid.NewGuid(), Username = "testuser" };
            var refreshToken = new RefreshToken
            {
                UserId = user.Id,
                Token = "valid",
                ExpiresAt = DateTime.UtcNow.AddHours(1),
                RevokedAt = null,
                User = user
            };

            refreshTokenRepository
                .Setup(r => r.GetByTokenAsync("valid"))
                .ReturnsAsync(refreshToken);

            jwtTokenGenerator
                .Setup(j => j.GenerateToken(user))
                .Returns(("generated-token", DateTime.UtcNow.AddHours(1)));

            var handler = new RefreshTokenHandler(
                refreshTokenRepository.Object,
                refreshTokenService.Object,
                jwtTokenGenerator.Object,
                unitOfWork.Object);

            // Act & Assert
            Assert.DoesNotThrowAsync(async () =>
                await handler.Handle(new RefreshTokenCommand("valid"), CancellationToken.None));
        }

        [Test]
        public void Handle_RevokedRefreshToken_ThrowsException()
        {
            // Arrange
            var refreshTokenRepository = new Mock<IRefreshTokenRepository>();
            var refreshTokenService = new Mock<IRefreshTokenService>();
            var jwtTokenGenerator = new Mock<IJwtTokenGenerator>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var handler = new RefreshTokenHandler(
                refreshTokenRepository.Object,
                refreshTokenService.Object,
                jwtTokenGenerator.Object,
                unitOfWork.Object);
            // Act & Assert
            Assert.ThrowsAsync<Exception>(async () =>
                await handler.Handle(new RefreshTokenCommand("revoked"), CancellationToken.None));
        }
    }
}
