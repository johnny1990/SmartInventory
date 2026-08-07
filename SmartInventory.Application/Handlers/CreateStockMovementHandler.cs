using MediatR;
using SmartInventory.Application.Commands;
using SmartInventory.Domain.Entities;
using SmartInventory.Domain.Enums;
using SmartInventory.Infrastructure.Interfaces;

namespace SmartInventory.Application.Handlers
{
    public class CreateStockMovementHandler
        : IRequestHandler<CreateStockMovementCommand, Guid>
    {
        private readonly IProductRepository _productRepository;
        private readonly IStockMovementRepository _movementRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuditRepository _auditRepository;

        public CreateStockMovementHandler(
            IProductRepository productRepository,
            IStockMovementRepository movementRepository,
            IUnitOfWork unitOfWork,
            IAuditRepository auditRepository)
        {
            _productRepository = productRepository;
            _movementRepository = movementRepository;
            _unitOfWork = unitOfWork;
            _auditRepository = auditRepository;
        }

        public async Task<Guid> Handle(
            CreateStockMovementCommand request,
            CancellationToken cancellationToken)
        {
            var product =
                await _productRepository.GetByIdAsync(request.ProductId);

            if (product == null)
                throw new Exception("Product not found.");

            switch (request.MovementType)
            {
                case MovementType.StockIn:
                    product.QuantityInStock += request.Quantity;
                    break;

                case MovementType.StockOut:

                    if (product.QuantityInStock < request.Quantity)
                        throw new Exception("Not enough stock.");

                    product.QuantityInStock -= request.Quantity;
                    break;

                case MovementType.Adjustment:

                    product.QuantityInStock = request.Quantity;
                    break;
            }

            var movement = new StockMovement
            {
                Id = Guid.NewGuid(),
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                MovementType = request.MovementType,
                Notes = request.Notes,
                CreatedAt = DateTime.UtcNow
            };

            await _movementRepository.AddAsync(movement);
            
            await _auditRepository.LogAsync(
                "Create",
                "StockMovement",
                $"Id={movement.Id}; Created stock movement for product '{product.Name}'");

            await _unitOfWork.SaveChangesAsync();

            return movement.Id;
        }
    }
}

