using Moq;
using SmartInventory.Application.Handlers;
using SmartInventory.Application.Queries;
using SmartInventory.Domain.Entities;
using SmartInventory.Infrastructure.Interfaces;

namespace SmartInventory.Tests
{
    public class GetCategoriesHandlerTests
    {
        [Test]
        public async Task Handle_Should_Return_All_Categories()
        {
            var repository = new Mock<ICategoryRepository>();

            repository.Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<Category>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Electronics",
                        Description = "Devices"
                    }
                });

            var handler = new GetCategoriesHandler(repository.Object);

            var result = await handler.Handle(
                new GetCategoriesQuery(),
                CancellationToken.None);

            Assert.That(result.Count == 1);
            Assert.That(result[0].Name == "Electronics");
        }

        [Test]
        public async Task Handle_Should_Return_Empty_List_When_No_Categories()
        {
            var repository = new Mock<ICategoryRepository>();
            repository.Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<Category>());
            var handler = new GetCategoriesHandler(repository.Object);
            var result = await handler.Handle(
                new GetCategoriesQuery(),
                CancellationToken.None);
            Assert.That(result.Count == 0);
        }
    }
}
