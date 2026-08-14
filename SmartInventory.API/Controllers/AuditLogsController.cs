using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartInventory.Application.Queries;
using SmartInventory.Infrastructure.Common;

namespace SmartInventory.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class AuditLogsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuditLogsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] AuditLogSearchParameters parameters)
        {
            var result =
                await _mediator.Send(
                    new GetAuditLogsQuery(parameters));

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var log = await _mediator.Send(
                new GetAuditLogByIdQuery(id));

            if (log == null)
                return NotFound();

            return Ok(log);
        }
    }
}