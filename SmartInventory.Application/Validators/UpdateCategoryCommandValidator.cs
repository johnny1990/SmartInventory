using FluentValidation;
using SmartInventory.Application.Commands;

namespace SmartInventory.Application.Validators
{
    public class UpdateCategoryCommandValidator
        : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Description)
                .MaximumLength(500);
        }
    }
}
