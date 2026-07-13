using MediatR;
using SmartInventory.Application.Commands;
using SmartInventory.Domain.Entities;
using SmartInventory.Infrastructure.Interfaces;

namespace SmartInventory.Application.Handlers
{
    public class RegisterHandler : IRequestHandler<RegisterCommand, Guid>
    {
        private readonly IUserRepository _repository;

        private readonly IPasswordHasher _hasher;

        private readonly IUnitOfWork _unitOfWork;

        public RegisterHandler(
            IUserRepository repository,
            IPasswordHasher hasher,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _hasher = hasher;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(
            RegisterCommand request,
            CancellationToken cancellationToken)
        {
            var existingUser =
                await _repository.GetByUsernameAsync(
                    request.Username);

            if (existingUser != null)
                throw new Exception(
                    "Username already exists.");

            var user = new User
            {
                Username = request.Username,

                Email = request.Email,

                PasswordHash =
                    _hasher.HashPassword(request.Password),

                Role = "User"
            };

            await _repository.AddAsync(user);

            await _unitOfWork.SaveChangesAsync();

            return user.Id;
        }
    }
}