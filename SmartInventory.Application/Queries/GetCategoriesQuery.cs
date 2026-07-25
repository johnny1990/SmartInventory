using MediatR;
using SmartInventory.Application.DTOs;

namespace SmartInventory.Application.Queries
{
    public record GetCategoriesQuery()
        : IRequest<List<CategoryDto>>;
}
