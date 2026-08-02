using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartInventory.Application.Commands;
using SmartInventory.Application.Queries;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace SmartInventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthorizationController(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(
            RegisterCommand command)
        {
            var id =
                await _mediator.Send(command);

            return Ok(id);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
    LoginCommand command)
        {
            var token =
                await _mediator.Send(command);

            return Ok(token);
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)
                              ?? User.FindFirst(JwtRegisteredClaimNames.Sub);

            if (userIdClaim == null)
                return Unauthorized();

            var userId = Guid.Parse(userIdClaim.Value);

            var result = await _mediator.Send(
                new GetCurrentUserQuery(userId));

            return Ok(result);
        }
    }
}
