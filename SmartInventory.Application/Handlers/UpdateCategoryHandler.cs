using MediatR;
using SmartInventory.Application.Commands;
using SmartInventory.Infrastructure.Interfaces;

namespace SmartInventory.Application.Handlers
{
    public class UpdateCategoryHandler
        : IRequestHandler<UpdateCategoryCommand, bool>
    {
        private readonly ICategoryRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCategoryHandler(
            ICategoryRepository repository,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(
            UpdateCategoryCommand request,
            CancellationToken cancellationToken)
        {
            var category =
                await _repository.GetByIdAsync(request.Id);

            if (category == null)
                return false;

            category.Name = request.Name;
            category.Description = request.Description;
            category.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(category);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}