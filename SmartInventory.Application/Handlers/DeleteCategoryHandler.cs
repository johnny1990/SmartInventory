using MediatR;
using SmartInventory.Application.Commands;
using SmartInventory.Infrastructure.Interfaces;

namespace SmartInventory.Application.Handlers
{
    public class DeleteCategoryHandler
        : IRequestHandler<DeleteCategoryCommand, bool>
    {
        private readonly ICategoryRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCategoryHandler(
            ICategoryRepository repository,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(
            DeleteCategoryCommand request,
            CancellationToken cancellationToken)
        {
            var category =
                await _repository.GetByIdAsync(request.Id);

            if (category == null)
                return false;

            await _repository.DeleteAsync(category);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}