using MediatR;
using SmartInventory.Application.DTOs;

namespace SmartInventory.Application.Queries
{
    public record GetProductsQuery()
    : IRequest<List<ProductDto>>;
}
