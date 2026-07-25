using Moq;
using SmartInventory.Application.Commands;
using SmartInventory.Application.Handlers;
using SmartInventory.Domain.Entities;
using SmartInventory.Infrastructure.Interfaces;

namespace SmartInventory.Tests;

public class CreateCategoryHandlerTests
{
    [Test]
    public async Task Handle_Should_Create_Category()
    {
        // Arrange
        var repository = new Mock<ICategoryRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();

        var handler = new CreateCategoryHandler(
            repository.Object,
            unitOfWork.Object);

        var command = new CreateCategoryCommand(
            "Electronics",
            "Electronic devices");

        // Act
        var result = await handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        repository.Verify(r =>
            r.AddAsync(It.IsAny<Category>()), Times.Once);

        unitOfWork.Verify(u =>
            u.SaveChangesAsync(), Times.Once);

        Assert.That(result, Is.Not.EqualTo(Guid.Empty));
    }

    [Test]
    public async Task Handle_Should_Throw_Exception_When_Repository_Fails()
    {
        // Arrange
        var repository = new Mock<ICategoryRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        repository.Setup(r =>
            r.AddAsync(It.IsAny<Category>()))
            .ThrowsAsync(new Exception("Database error"));
        var handler = new CreateCategoryHandler(
            repository.Object,
            unitOfWork.Object);
        var command = new CreateCategoryCommand(
            "Electronics",
            "Electronic devices");
        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () =>
            await handler.Handle(command, CancellationToken.None));
    }

    [Test]
    public async Task Handle_Should_Throw_Exception_When_UnitOfWork_Fails()
    {
        // Arrange
        var repository = new Mock<ICategoryRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u =>
            u.SaveChangesAsync())
            .ThrowsAsync(new Exception("Database error"));
        var handler = new CreateCategoryHandler(
            repository.Object,
            unitOfWork.Object);
        var command = new CreateCategoryCommand(
            "Electronics",
            "Electronic devices");
        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () =>
            await handler.Handle(command, CancellationToken.None));
    }
}
