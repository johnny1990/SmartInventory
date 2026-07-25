using MediatR;
using SmartInventory.Application.DTOs;
using SmartInventory.Application.Queries;
using SmartInventory.Infrastructure.Interfaces;

namespace SmartInventory.Application.Handlers
{
    public class GetCategoryByIdHandler
        : IRequestHandler<GetCategoryByIdQuery, CategoryDto?>
    {
        private readonly ICategoryRepository _repository;

        public GetCategoryByIdHandler(
            ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<CategoryDto?> Handle(
            GetCategoryByIdQuery request,
            CancellationToken cancellationToken)
        {
            var category =
                await _repository.GetByIdAsync(request.Id);

            if (category == null)
                return null;

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
        }
    }
}