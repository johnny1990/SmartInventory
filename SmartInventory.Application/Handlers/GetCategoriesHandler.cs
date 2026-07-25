using MediatR;
using SmartInventory.Application.DTOs;
using SmartInventory.Application.Queries;
using SmartInventory.Infrastructure.Interfaces;

namespace SmartInventory.Application.Handlers
{
    public class GetCategoriesHandler
        : IRequestHandler<GetCategoriesQuery, List<CategoryDto>>
    {
        private readonly ICategoryRepository _repository;

        public GetCategoriesHandler(
            ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<CategoryDto>> Handle(
            GetCategoriesQuery request,
            CancellationToken cancellationToken)
        {
            var categories =
                await _repository.GetAllAsync();

            return categories
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description
                })
                .ToList();
        }
    }
}