using Moq;
using SmartInventory.Application.Commands;
using SmartInventory.Application.Handlers;
using SmartInventory.Domain.Entities;
using SmartInventory.Infrastructure.Interfaces;

namespace SmartInventory.Tests
{
    public class DeleteCategoryHandlerTests
    {
        [Test]
        public async Task Handle_Should_Delete_Category()
        {
            var id = Guid.NewGuid();

            var repository = new Mock<ICategoryRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();

            repository.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(new Category
                {
                    Id = id,
                    Name = "Electronics"
                });

            var handler =
                new DeleteCategoryHandler(
                    repository.Object,
                    unitOfWork.Object);

            var result = await handler.Handle(
                new DeleteCategoryCommand(id),
                CancellationToken.None);

            Assert.That(result);

            repository.Verify(r =>
                r.DeleteAsync(It.IsAny<Category>()), Times.Once);

            unitOfWork.Verify(u =>
                u.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task Handle_Should_Return_False_When_Category_Not_Found()
        {
            var id = Guid.NewGuid();
            var repository = new Mock<ICategoryRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();
            repository.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync((Category)null);
            var handler =
                new DeleteCategoryHandler(
                    repository.Object,
                    unitOfWork.Object);
            var result = await handler.Handle(
                new DeleteCategoryCommand(id),
                CancellationToken.None);
            Assert.That(result, Is.False);
            repository.Verify(r =>
                r.DeleteAsync(It.IsAny<Category>()), Times.Never);
            unitOfWork.Verify(u =>
                u.SaveChangesAsync(), Times.Never);
        }

        [Test]
        public async Task Handle_Should_Throw_Exception_When_Delete_Fails()
        {
            var id = Guid.NewGuid();
            var repository = new Mock<ICategoryRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();
            repository.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(new Category
                {
                    Id = id,
                    Name = "Electronics"
                });
            repository.Setup(r => r.DeleteAsync(It.IsAny<Category>()))
                .ThrowsAsync(new Exception("Delete failed"));
            var handler =
                new DeleteCategoryHandler(
                    repository.Object,
                    unitOfWork.Object);
            Assert.ThrowsAsync<Exception>(async () =>
            {
                await handler.Handle(
                    new DeleteCategoryCommand(id),
                    CancellationToken.None);
            });
            repository.Verify(r =>
                r.DeleteAsync(It.IsAny<Category>()), Times.Once);
            unitOfWork.Verify(u =>
                u.SaveChangesAsync(), Times.Never);
        }
    }
}
