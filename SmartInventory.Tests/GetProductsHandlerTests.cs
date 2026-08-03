using Moq;
using SmartInventory.Application.Handlers;
using SmartInventory.Application.Queries;
using SmartInventory.Domain.Entities;
using SmartInventory.Infrastructure.Common;
using SmartInventory.Infrastructure.Interfaces;

namespace SmartInventory.Tests
{
    public class GetProductsHandlerTests
    {
        [Test]
        public async Task Handle_Should_Return_All_Products()
        {
            var repository = new Mock<IProductRepository>();
            repository.Setup(r => r.GetAllAsync(It.IsAny<ProductSearchParameters>()))
                .ReturnsAsync((new List<Product>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Laptop",
                        SKU = "LAP123",
                        Price = 999.99m,
                        QuantityInStock = 10,
                        CategoryId = Guid.NewGuid(),
                        Category = new Category
                        {
                            Id = Guid.NewGuid(),
                            Name = "Electronics"
                        }
                    }
                }, 1));
            var handler = new GetProductsHandler(repository.Object);
            var result = await handler.Handle(
                new GetProductsQuery(new ProductSearchParameters()),
                CancellationToken.None);
            Assert.That(result.Items.Count == 1);
            Assert.That(result.Items[0].Name == "Laptop");
        }

        [Test]
        public async Task Handle_Should_Return_Empty_List_When_No_Products()
        {
            var repository = new Mock<IProductRepository>();
            repository.Setup(r => r.GetAllAsync(It.IsAny<ProductSearchParameters>()))
                .ReturnsAsync((new List<Product>(), 0));
            var handler = new GetProductsHandler(repository.Object);
            var result = await handler.Handle(
                new GetProductsQuery(new ProductSearchParameters()),
                CancellationToken.None);
            Assert.That(result.Items.Count == 0);
        }

        [Test]
        public async Task Handle_Should_Return_Paged_Results()
        {
            var repository = new Mock<IProductRepository>();
            repository.Setup(r => r.GetAllAsync(It.IsAny<ProductSearchParameters>()))
                .ReturnsAsync((new List<Product>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Laptop",
                        SKU = "LAP123",
                        Price = 999.99m,
                        QuantityInStock = 10,
                        CategoryId = Guid.NewGuid(),
                        Category = new Category
                        {
                            Id = Guid.NewGuid(),
                            Name = "Electronics"
                        }
                    }
                }, 1));
            var handler = new GetProductsHandler(repository.Object);
            var result = await handler.Handle(
                new GetProductsQuery(new ProductSearchParameters
                {
                    Page = 1,
                    PageSize = 10
                }),
                CancellationToken.None);
            Assert.That(result.Page == 1);
            Assert.That(result.PageSize == 10);
            Assert.That(result.TotalItems == 1);
            Assert.That(result.TotalPages == 1);
        }

        [Test]
        public async Task Handle_Should_Return_Correct_Category_Name()
        {
            var repository = new Mock<IProductRepository>();
            repository.Setup(r => r.GetAllAsync(It.IsAny<ProductSearchParameters>()))
                .ReturnsAsync((new List<Product>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Laptop",
                        SKU = "LAP123",
                        Price = 999.99m,
                        QuantityInStock = 10,
                        CategoryId = Guid.NewGuid(),
                        Category = new Category
                        {
                            Id = Guid.NewGuid(),
                            Name = "Electronics"
                        }
                    }
                }, 1));
            var handler = new GetProductsHandler(repository.Object);
            var result = await handler.Handle(
                new GetProductsQuery(new ProductSearchParameters()),
                CancellationToken.None);
            Assert.That(result.Items[0].CategoryName == "Electronics");
        }

        [Test]
        public async Task Handle_Should_Return_Correct_Product_Details()
        {
            var repository = new Mock<IProductRepository>();
            repository.Setup(r => r.GetAllAsync(It.IsAny<ProductSearchParameters>()))
                .ReturnsAsync((new List<Product>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Laptop",
                        SKU = "LAP123",
                        Price = 999.99m,
                        QuantityInStock = 10,
                        CategoryId = Guid.NewGuid(),
                        Category = new Category
                        {
                            Id = Guid.NewGuid(),
                            Name = "Electronics"
                        }
                    }
                }, 1));
            var handler = new GetProductsHandler(repository.Object);
            var result = await handler.Handle(
                new GetProductsQuery(new ProductSearchParameters()),
                CancellationToken.None);
            Assert.That(result.Items[0].Name == "Laptop");
            Assert.That(result.Items[0].SKU == "LAP123");
            Assert.That(result.Items[0].Price == 999.99m);
            Assert.That(result.Items[0].QuantityInStock == 10);
        }

        [Test]
        public async Task Handle_Should_Return_Correct_Product_Id()
        {
            var productId = Guid.NewGuid();
            var repository = new Mock<IProductRepository>();
            repository.Setup(r => r.GetAllAsync(It.IsAny<ProductSearchParameters>()))
                .ReturnsAsync((new List<Product>
                {
                    new()
                    {
                        Id = productId,
                        Name = "Laptop",
                        SKU = "LAP123",
                        Price = 999.99m,
                        QuantityInStock = 10,
                        CategoryId = Guid.NewGuid(),
                        Category = new Category
                        {
                            Id = Guid.NewGuid(),
                            Name = "Electronics"
                        }
                    }
                }, 1));
            var handler = new GetProductsHandler(repository.Object);
            var result = await handler.Handle(
                new GetProductsQuery(new ProductSearchParameters()),
                CancellationToken.None);
            Assert.That(result.Items[0].Id == productId);
        }
    }
}
