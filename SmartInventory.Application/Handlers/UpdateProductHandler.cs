using MediatR;
using SmartInventory.Application.Commands;
using SmartInventory.Infrastructure.Interfaces;

namespace SmartInventory.Application.Handlers
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, bool>
    {
        private readonly IProductRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuditRepository _auditRepository;


        public UpdateProductHandler(
            IProductRepository repository,
            IUnitOfWork unitOfWork,
            IAuditRepository auditRepository)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _auditRepository = auditRepository;
        }

        public async Task<bool> Handle(
            UpdateProductCommand request,
            CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(request.Id);

            if (product == null)
                return false;

            product.Name = request.Name;
            product.SKU = request.SKU;
            product.Price = request.Price;
            product.QuantityInStock = request.QuantityInStock;


            await _auditRepository.LogAsync(
                "Update",
                "Product",
                $"Product updated (Id: {request.Id}): {product.Name}, SKU: {product.SKU}, Price: {product.Price}, QuantityInStock: {product.QuantityInStock}");

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
