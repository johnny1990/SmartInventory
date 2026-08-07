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
        private readonly IAuditRepository _auditRepository;

        public DeleteProductHandler(
            SmartInventoryDbContext context,
            IUnitOfWork unitOfWork,
            IAuditRepository auditRepository)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _auditRepository = auditRepository;
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

            await _auditRepository.LogAsync(
                "DeleteProduct",            
                "Product",                  
                $"Product with ID {request.Id} deleted."); 

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}