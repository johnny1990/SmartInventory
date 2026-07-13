using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartInventory.Application.Commands;

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
    }
}
