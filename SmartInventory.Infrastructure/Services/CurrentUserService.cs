using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SmartInventory.Infrastructure.Interfaces;

namespace SmartInventory.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ClaimsPrincipal? User =>
            _httpContextAccessor.HttpContext?.User;

        public bool IsAuthenticated =>
            User?.Identity?.IsAuthenticated ?? false;

        public string? UserId =>
            User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        public string? UserName =>
            User?.FindFirst(ClaimTypes.Name)?.Value;

        public string? Email =>
            User?.FindFirst(ClaimTypes.Email)?.Value;
    }
}