using MediatR;
using SmartInventory.Application.Commands;
using SmartInventory.Infrastructure.Interfaces;

namespace SmartInventory.Application.Handlers
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, bool>
    {
        private readonly IProductRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductHandler(
            IProductRepository repository,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
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

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
