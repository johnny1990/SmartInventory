using MediatR;
using SmartInventory.Application.DTOs;

namespace SmartInventory.Application.Queries
{
    public record GetCategoryByIdQuery(Guid Id)
        : IRequest<CategoryDto?>;
}
