using MediatR;
using SmartInventory.Application.Commands;
using SmartInventory.Domain.Entities;
using SmartInventory.Infrastructure.Interfaces;

namespace SmartInventory.Application.Handlers
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IProductRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuditRepository _auditRepository;

        public CreateProductHandler(
            IProductRepository repository,
            IUnitOfWork unitOfWork,
            IAuditRepository auditRepository)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _auditRepository = auditRepository;
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

            await _auditRepository.LogAsync(
            "Create",
            "Product",
            $"Created product '{product.Name}'");

            await _unitOfWork.SaveChangesAsync();

            return product.Id;
        }
    }
}
