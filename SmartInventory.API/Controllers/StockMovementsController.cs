using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartInventory.Application.Commands;
using SmartInventory.Application.Queries;

namespace SmartInventory.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class StockMovementsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StockMovementsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result =
                await _mediator.Send(new GetStockMovementsQuery());

            return Ok(result);
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        public async Task<IActionResult> Create(
            CreateStockMovementCommand command)
        {
            var id =
                await _mediator.Send(command);

            return Ok(id);
        }
    }
}