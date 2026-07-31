using MediatR;
using SmartInventory.Application.Commands;
using SmartInventory.Domain.Entities;
using SmartInventory.Domain.Enums;
using SmartInventory.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartInventory.Application.Handlers
{
    public class CreateStockMovementHandler
        : IRequestHandler<CreateStockMovementCommand, Guid>
    {
        private readonly IProductRepository _productRepository;

        private readonly IStockMovementRepository _movementRepository;

        private readonly IUnitOfWork _unitOfWork;

        public CreateStockMovementHandler(
            IProductRepository productRepository,
            IStockMovementRepository movementRepository,
            IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _movementRepository = movementRepository;
            _unitOfWork = unitOfWork;
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

            await _unitOfWork.SaveChangesAsync();

            return movement.Id;
        }
    }
}

