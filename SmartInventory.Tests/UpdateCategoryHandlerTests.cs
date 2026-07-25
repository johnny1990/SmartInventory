using Moq;
using NUnit.Framework;
using SmartInventory.Application.Commands;
using SmartInventory.Application.Handlers;
using SmartInventory.Domain.Entities;
using SmartInventory.Infrastructure.Interfaces;

namespace SmartInventory.Tests
{
    public class UpdateCategoryHandlerTests
    {
        [Test]
        public async Task Handle_Should_Update_Category()
        {
            var id = Guid.NewGuid();

            var repository = new Mock<ICategoryRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();

            repository.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(new Category
                {
                    Id = id,
                    Name = "Old"
                });

            var handler =
                new UpdateCategoryHandler(
                    repository.Object,
                    unitOfWork.Object);

            var command = new UpdateCategoryCommand(
                id,
                "New",
                "Updated");

            var result = await handler.Handle(
                command,
                CancellationToken.None);

            Assert.That(result, Is.True);

            repository.Verify(r =>
                r.UpdateAsync(It.IsAny<Category>()), Times.Once);

            unitOfWork.Verify(u =>
                u.SaveChangesAsync(), Times.Once);
        }
    }
}

