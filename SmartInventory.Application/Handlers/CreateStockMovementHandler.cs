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
            // Validate movement quantity.
            switch (request.MovementType)
            {
                case MovementType.StockIn:
                case MovementType.StockOut:

                    if (request.Quantity <= 0)
                        throw new ArgumentOutOfRangeException(
                            nameof(request.Quantity),
                            "Stock-in and stock-out quantities must be greater than zero.");

                    break;

                case MovementType.Adjustment:

                    if (request.Quantity < 0)
                        throw new ArgumentOutOfRangeException(
                            nameof(request.Quantity),
                            "Adjustment quantity cannot be negative.");

                    break;

                default:

                    throw new ArgumentException(
                        "Invalid movement type.",
                        nameof(request.MovementType));
            }

            // Load the product as a tracked entity.
            var product = await _productRepository.GetByIdForUpdateAsync(
                request.ProductId);

            if (product == null)
                throw new Exception("Product not found.");

            // Store stock before the movement.
            var stockBefore = product.QuantityInStock;

            // Apply stock movement.
            switch (request.MovementType)
            {
                case MovementType.StockIn:

                    product.QuantityInStock =
                        checked(product.QuantityInStock + request.Quantity);

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

            // Store stock after the movement.
            var stockAfter = product.QuantityInStock;

            // Create stock movement record.
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

            // Determine audit action.
            var auditAction = request.MovementType switch
            {
                MovementType.StockIn => "Stock In",
                MovementType.StockOut => "Stock Out",
                MovementType.Adjustment => "Stock Adjustment",
                _ => "Stock Movement"
            };

            // Create audit log.
            await _auditRepository.LogAsync(
                auditAction,
                "StockMovement",
                $"Product='{product.Name}'; " +
                $"Quantity={request.Quantity}; " +
                $"StockBefore={stockBefore}; " +
                $"StockAfter={stockAfter}; " +
                $"MovementId={movement.Id}");

            // Save Product + StockMovement + AuditLog.
            await _unitOfWork.SaveChangesAsync();

            return movement.Id;
        }
    }
}