using MediatR;
using SmartInventory.Application.Commands;
using SmartInventory.Domain.Entities;
using SmartInventory.Infrastructure.Interfaces;

namespace SmartInventory.Application.Handlers
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IProductRepository _repository;

        public CreateProductHandler(
            IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(
            CreateProductCommand request,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ArgumentException("Product name cannot be empty.", nameof(request.Name));
            }

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                SKU = request.SKU,
                Price = request.Price,
                QuantityInStock = request.QuantityInStock,
                CategoryId = request.CategoryId,
                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(product);

            await _repository.SaveChangesAsync();

            return product.Id;
        }
    }
}
