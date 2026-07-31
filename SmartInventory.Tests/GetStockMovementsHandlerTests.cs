using Moq;
using SmartInventory.Application.Handlers;
using SmartInventory.Application.Queries;
using SmartInventory.Domain.Entities;
using SmartInventory.Domain.Enums;
using SmartInventory.Infrastructure.Interfaces;

namespace SmartInventory.Tests
{
    [TestFixture]
    public class GetStockMovementsHandlerTests
    {
        private Mock<IStockMovementRepository> _repository = null!;
        private GetStockMovementsHandler _handler = null!;

        [SetUp]
        public void Setup()
        {
            _repository = new Mock<IStockMovementRepository>();

            _handler = new GetStockMovementsHandler(
                _repository.Object);
        }

        [Test]
        public async Task Handle_ShouldReturnAllStockMovements()
        {
            // Arrange
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Laptop"
            };

            var movements = new List<StockMovement>
        {
            new()
            {
                Id = Guid.NewGuid(),
                ProductId = product.Id,
                Product = product,
                Quantity = 15,
                MovementType = MovementType.StockIn,
                Notes = "Supplier",
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                ProductId = product.Id,
                Product = product,
                Quantity = 5,
                MovementType = MovementType.StockOut,
                Notes = "Customer",
                CreatedAt = DateTime.UtcNow
            }
        };

            _repository
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(movements);

            // Act
            var result = await _handler.Handle(
                new GetStockMovementsQuery(),
                CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));

            Assert.That(result[0].ProductName, Is.EqualTo("Laptop"));
            Assert.That(result[0].Quantity, Is.EqualTo(15));
            Assert.That(result[0].MovementType, Is.EqualTo("StockIn"));

            _repository.Verify(
                x => x.GetAllAsync(),
                Times.Once);
        }

        [Test]
        public async Task Handle_WhenNoMovements_ShouldReturnEmptyList()
        {
            // Arrange
            _repository
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<StockMovement>());

            // Act
            var result = await _handler.Handle(
                new GetStockMovementsQuery(),
                CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);

            _repository.Verify(
                x => x.GetAllAsync(),
                Times.Once);
        }
    }
}
