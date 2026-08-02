using Moq;
using SmartInventory.Application.Handlers;
using SmartInventory.Application.Queries;
using SmartInventory.Domain.Entities;
using SmartInventory.Infrastructure.Interfaces;

namespace SmartInventory.Tests
{
    public class GetProductByIdHandlerTests
    {
        [Test]
        public async Task Handle_ReturnsProductDto_WhenProductExists()
        {
            // Arrange
            var mockRepository = new Mock<IProductRepository>();
            var handler = new GetProductByIdHandler(mockRepository.Object);
            var query = new GetProductByIdQuery(Guid.NewGuid());
            var product = new Product
            {
                Id = query.Id,
                Name = "Test Product",
                Price = 10.99m,
                CategoryId = Guid.NewGuid()
            };
            mockRepository.Setup(repo => repo.GetByIdAsync(query.Id)).ReturnsAsync(product);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(product.Id));
            Assert.That(result.Name, Is.EqualTo(product.Name));
            Assert.That(result.Price, Is.EqualTo(product.Price));
            Assert.That(result.CategoryId, Is.EqualTo(product.CategoryId));
        }

        [Test]
        public async Task Handle_ReturnsNull_WhenProductDoesNotExist()
        {
            // Arrange
            var mockRepository = new Mock<IProductRepository>();
            var handler = new GetProductByIdHandler(mockRepository.Object);
            var query = new GetProductByIdQuery(Guid.NewGuid());
            mockRepository.Setup(repo => repo.GetByIdAsync(query.Id)).ReturnsAsync((Product?)null);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task Handle_ThrowsArgumentNullException_WhenQueryIsNull()
        {
            // Arrange
            var mockRepository = new Mock<IProductRepository>();
            var handler = new GetProductByIdHandler(mockRepository.Object);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(null, CancellationToken.None));
        }
    }
}
