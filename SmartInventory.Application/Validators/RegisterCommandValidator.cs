using FluentValidation;
using SmartInventory.Application.Commands;

namespace SmartInventory.Application.Validators
{
    public class RegisterCommandValidator
    : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .EmailAddress();

            RuleFor(x => x.Password)
                .MinimumLength(8);
        }
    }
}
