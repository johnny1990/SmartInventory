using MediatR;
using SmartInventory.Application.DTOs;
using SmartInventory.Application.Queries;
using SmartInventory.Infrastructure.Interfaces;

namespace SmartInventory.Application.Handlers
{
    public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
    {
        private readonly IProductRepository _productRepository;
        public GetProductByIdHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            var product = await _productRepository.GetByIdAsync(request.Id);
            if (product == null)
            {
                return null;
            }
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                CategoryId = product.CategoryId
            };
        }
    }
}
