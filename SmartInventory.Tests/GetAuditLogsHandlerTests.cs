using Moq;
using SmartInventory.Application.Handlers;
using SmartInventory.Application.Queries;
using SmartInventory.Domain.Entities;
using SmartInventory.Infrastructure.Common;
using SmartInventory.Infrastructure.Interfaces;

namespace SmartInventory.Tests.Handlers
{
    public class GetAuditLogsHandlerTests
    {
        [Test]
        public async Task GetAuditLogs_ShouldReturnPagedResult()
        {
            // Arrange
            var repository = new Mock<IAuditRepository>();

            var logs = new List<AuditLog>
            {
                new AuditLog
                {
                    Id = Guid.NewGuid(),
                    Action = "Stock Out",
                    EntityName = "StockMovement",
                    UserName = "john",
                    Changes = "StockBefore=25; StockAfter=24",
                    CreatedAt = DateTime.UtcNow
                },
                new AuditLog
                {
                    Id = Guid.NewGuid(),
                    Action = "Create",
                    EntityName = "Product",
                    UserName = "john",
                    Changes = "Created product",
                    CreatedAt = DateTime.UtcNow
                }
            };

            var parameters = new AuditLogSearchParameters
            {
                Page = 1,
                PageSize = 10,
                SortBy = "createdAt",
                Descending = true
            };

            repository
                .Setup(x => x.GetAllAsync(parameters))
                .ReturnsAsync((logs, 2));

            var handler = new GetAuditLogsHandler(
                repository.Object);

            var query = new GetAuditLogsQuery(parameters);

            // Act
            var result = await handler.Handle(
                query,
                CancellationToken.None);

            // Assert
            Assert.That(result != null);
            Assert.That(result.TotalItems == 2);
            Assert.That(result.Items.Count == 2);


        }
    }
}