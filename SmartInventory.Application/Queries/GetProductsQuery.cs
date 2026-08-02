using MediatR;
using SmartInventory.Application.DTOs;
using SmartInventory.Infrastructure.Common;

namespace SmartInventory.Application.Queries
{
public record GetProductsQuery(
    ProductSearchParameters SearchParameters)
    : IRequest<List<ProductDto>>;
}