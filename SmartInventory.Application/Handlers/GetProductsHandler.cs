using MediatR;
using SmartInventory.Application.DTOs;
using SmartInventory.Application.Queries;
using SmartInventory.Infrastructure.Common;
using SmartInventory.Infrastructure.Interfaces;

namespace SmartInventory.Application.Handlers
{
    public class GetProductsHandler
        : IRequestHandler<GetProductsQuery, PagedResult<ProductDto>>
    {
        private readonly IProductRepository _repository;

        public GetProductsHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<ProductDto>> Handle(
            GetProductsQuery request,
            CancellationToken cancellationToken)
        {
            var (products, totalCount) =
                await _repository.GetAllAsync(request.SearchParameters);

            var items = products.Select(x => new ProductDto
            {
                Id = x.Id,
                Name = x.Name,
                SKU = x.SKU,
                Price = x.Price,
                QuantityInStock = x.QuantityInStock,
                CategoryId = x.CategoryId,
                CategoryName = x.Category.Name
            }).ToList();

            return new PagedResult<ProductDto>
            {
                Items = items,
                Page = request.SearchParameters.Page,
                PageSize = request.SearchParameters.PageSize,
                TotalItems = totalCount,
                TotalPages = (int)Math.Ceiling(
                    totalCount / (double)request.SearchParameters.PageSize)
            };
        }
    }
}
