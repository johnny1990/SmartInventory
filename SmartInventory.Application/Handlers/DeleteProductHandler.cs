using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartInventory.Application.Commands;
using SmartInventory.Infrastructure.Interfaces;
using SmartInventory.Infrastructure.Persistence;

namespace SmartInventory.Application.Handlers
{
    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly SmartInventoryDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProductHandler(
            SmartInventoryDbContext context,
            IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(
            DeleteProductCommand request,
            CancellationToken cancellationToken)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (product == null)
                return false;

            _context.Products.Remove(product);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}