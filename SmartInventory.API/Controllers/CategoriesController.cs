using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartInventory.Application.Commands;
using SmartInventory.Application.Queries;

namespace SmartInventory.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoriesController(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result =
                await _mediator.Send(new GetCategoriesQuery());

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result =
                await _mediator.Send(new GetCategoryByIdQuery(id));

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            CreateCategoryCommand command)
        {
            var id =
                await _mediator.Send(command);

            return Ok(id);
        }

        [HttpPut]
        public async Task<IActionResult> Update(
            UpdateCategoryCommand command)
        {
            var updated =
                await _mediator.Send(command);

            if (!updated)
                return NotFound();

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted =
                await _mediator.Send(
                    new DeleteCategoryCommand(id));

            if (!deleted)
                return NotFound();

            return Ok(deleted);
        }
    }
}
