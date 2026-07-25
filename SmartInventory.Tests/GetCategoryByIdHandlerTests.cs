using Moq;
using SmartInventory.Application.Handlers;
using SmartInventory.Application.Queries;
using SmartInventory.Domain.Entities;
using SmartInventory.Infrastructure.Interfaces;

namespace SmartInventory.Tests
{
    public class GetCategoryByIdHandlerTests
    {
        [Test]
        public async Task Handle_Should_Return_Category()
        {
            var id = Guid.NewGuid();

            var repository = new Mock<ICategoryRepository>();

            repository.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(new Category
                {
                    Id = id,
                    Name = "Electronics"
                });

            var handler =
                new GetCategoryByIdHandler(repository.Object);

            var result = await handler.Handle(
                new GetCategoryByIdQuery(id),
                CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Name, Is.EqualTo("Electronics"));
        }

        [Test]
        public async Task Handle_Should_Return_Null_When_Category_Not_Found()
        {
            var id = Guid.NewGuid();
            var repository = new Mock<ICategoryRepository>();
            repository.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync((Category?)null);
            var handler =
                new GetCategoryByIdHandler(repository.Object);
            var result = await handler.Handle(
                new GetCategoryByIdQuery(id),
                CancellationToken.None);
            Assert.That(result, Is.Null);
        }
    }
}
