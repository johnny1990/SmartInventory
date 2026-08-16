using Moq;
using SmartInventory.Application.Commands;
using SmartInventory.Application.Handlers;
using SmartInventory.Domain.Entities;
using SmartInventory.Domain.Enums;
using SmartInventory.Infrastructure.Interfaces;

namespace SmartInventory.Tests
{
    [NonParallelizable]
    [TestFixture]
    public class CreateStockMovementHandlerTests
    {
        private Mock<IProductRepository> _productRepository = null!;
        private Mock<IStockMovementRepository> _movementRepository = null!;
        private Mock<IUnitOfWork> _unitOfWork = null!;
        private Mock<IAuditRepository> _auditRepository = null!;

        private CreateStockMovementHandler _handler = null!;

        [SetUp]
        public void Setup()
        {
            _productRepository = new Mock<IProductRepository>();
            _movementRepository = new Mock<IStockMovementRepository>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _auditRepository = new Mock<IAuditRepository>();

            _handler = new CreateStockMovementHandler(
                _productRepository.Object,
                _movementRepository.Object,
                _unitOfWork.Object,
                _auditRepository.Object);
        }

        [Test]
        public async Task Handle_StockIn_ShouldIncreaseInventory()
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Laptop",
                QuantityInStock = 10
            };

            _productRepository
                .Setup(x => x.GetByIdAsync(product.Id))
                .ReturnsAsync(product);

            _productRepository
                .Setup(x => x.GetByIdForUpdateAsync(product.Id))
                .ReturnsAsync(product);

            var command = new CreateStockMovementCommand(
                product.Id,
                5,
                MovementType.StockIn,
                "Supplier delivery");

            await _handler.Handle(command, CancellationToken.None);

            Assert.That(product.QuantityInStock, Is.EqualTo(15));

            _movementRepository.Verify(
                x => x.AddAsync(It.IsAny<StockMovement>()),
                Times.Once);

            _unitOfWork.Verify(
                x => x.SaveChangesAsync(),
                Times.Once);
        }

        [Test]
        public async Task Handle_StockOut_ShouldDecreaseInventory()
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Keyboard",
                QuantityInStock = 20
            };

            _productRepository
                .Setup(x => x.GetByIdAsync(product.Id))
                .ReturnsAsync(product);

            _productRepository
                .Setup(x => x.GetByIdForUpdateAsync(product.Id))
                .ReturnsAsync(product);

            var command = new CreateStockMovementCommand(
                product.Id,
                8,
                MovementType.StockOut,
                "Customer order");

            await _handler.Handle(command, CancellationToken.None);

            Assert.That(product.QuantityInStock, Is.EqualTo(12));

            _movementRepository.Verify(
                x => x.AddAsync(It.IsAny<StockMovement>()),
                Times.Once);

            _unitOfWork.Verify(
                x => x.SaveChangesAsync(),
                Times.Once);
        }

        [Test]
        public void Handle_ProductNotFound_ShouldThrowException()
        {
            _productRepository
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Product?)null);

            var command = new CreateStockMovementCommand(
                Guid.NewGuid(),
                10,
                MovementType.StockIn,
                "");

            var ex = Assert.ThrowsAsync<Exception>(async () =>
                await _handler.Handle(command, CancellationToken.None));

            Assert.That(ex!.Message, Is.EqualTo("Product not found."));
        }

        [Test]
        public void Handle_NotEnoughStock_ShouldThrowException()
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                QuantityInStock = 3
            };

            _productRepository
                .Setup(x => x.GetByIdAsync(product.Id))
                .ReturnsAsync(product);

            _productRepository
                .Setup(x => x.GetByIdForUpdateAsync(product.Id))
                .ReturnsAsync(product);

            var command = new CreateStockMovementCommand(
                product.Id,
                10,
                MovementType.StockOut,
                "");

            var ex = Assert.ThrowsAsync<Exception>(async () =>
                await _handler.Handle(command, CancellationToken.None));

            Assert.That(ex!.Message, Is.EqualTo("Not enough stock."));
        }

        [Test]
        public async Task Handle_Adjustment_ShouldSetInventory()
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                QuantityInStock = 50
            };

            _productRepository
                .Setup(x => x.GetByIdAsync(product.Id))
                .ReturnsAsync(product);

            _productRepository
                .Setup(x => x.GetByIdForUpdateAsync(product.Id))
                .ReturnsAsync(product);

            var command = new CreateStockMovementCommand(
                product.Id,
                32,
                MovementType.Adjustment,
                "Inventory count");

            await _handler.Handle(command, CancellationToken.None);

            Assert.That(product.QuantityInStock, Is.EqualTo(32));
        }
    }
}

