using Moq;
using SmartInventory.Application.Commands;
using SmartInventory.Application.Handlers;
using SmartInventory.Domain.Entities;
using SmartInventory.Infrastructure.Interfaces;

namespace SmartInventory.Tests
{
    public class CreateProductHandlerTests
    {
        [Test]
        public async Task Handle_Should_Create_Product()
        {
            // Arrange

            var repositoryMock =
                new Mock<IProductRepository>();

            repositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Product>()))
                .ReturnsAsync((Product p) => p);

            var command =
                new CreateProductCommand(
                    "Laptop",
                    "LAP-001",
                    3500,
                    10,
                    Guid.NewGuid());

            var handler =
                new CreateProductHandler(
                    repositoryMock.Object);

            // Act

            var id =
                await handler.Handle(
                    command,
                    CancellationToken.None);

            // Assert

            //id.Should().NotBe(Guid.Empty);

            repositoryMock.Verify(
                x => x.AddAsync(It.IsAny<Product>()),
                Times.Once);

            repositoryMock.Verify(
                x => x.SaveChangesAsync(),
                Times.Once);
        }

        [Test]
        public async Task Handle_Should_Throw_Exception_When_Repository_Fails()
        {
            // Arrange
            var repositoryMock =
                new Mock<IProductRepository>();
            repositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Product>()))
                .ThrowsAsync(new Exception("Database error"));
            var command =
                new CreateProductCommand(
                    "Laptop",
                    "LAP-001",
                    3500,
                    10,
                    Guid.NewGuid());
            var handler =
                new CreateProductHandler(
                    repositoryMock.Object);
            // Act & Assert
            Assert.ThrowsAsync<Exception>(async () =>
            {
                await handler.Handle(
                    command,
                    CancellationToken.None);
            });
            repositoryMock.Verify(
                x => x.AddAsync(It.IsAny<Product>()),
                Times.Once);
            repositoryMock.Verify(
                x => x.SaveChangesAsync(),
                Times.Never);
        }

        [Test]
        public async Task Handle_Should_Throw_Exception_When_Command_Is_Invalid()
        {
            // Arrange
            var repositoryMock =
                new Mock<IProductRepository>();
            var command =
                new CreateProductCommand(
                    "", // Invalid name
                    "LAP-001",
                    3500,
                    10,
                    Guid.NewGuid());
            var handler =
                new CreateProductHandler(
                    repositoryMock.Object);
            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await handler.Handle(
                    command,
                    CancellationToken.None);
            });
            repositoryMock.Verify(
                x => x.AddAsync(It.IsAny<Product>()),
                Times.Never);
            repositoryMock.Verify(
                x => x.SaveChangesAsync(),
                Times.Never);
        }
    }
}
