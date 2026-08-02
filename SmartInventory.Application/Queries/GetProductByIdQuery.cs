using MediatR;
using SmartInventory.Application.DTOs;

namespace SmartInventory.Application.Queries
{
    public record GetProductByIdQuery(Guid Id)
        : IRequest<ProductDto?>;
}
