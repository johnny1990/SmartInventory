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
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result =
                await _mediator.Send(new GetProductsQuery());

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            CreateProductCommand command)
        {
            var id = await _mediator.Send(command);

            return Ok(id);
        }

        [HttpPut]
        public async Task<IActionResult> Update(
    UpdateProductCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteProductCommand(id));

            return Ok(result);
        }
    }
}
