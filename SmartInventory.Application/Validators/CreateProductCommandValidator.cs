using FluentValidation;
using SmartInventory.Application.Commands;

namespace SmartInventory.Application.Validators
{
    public class CreateProductCommandValidator
    : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.SKU)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.Price)
                .GreaterThan(0);

            RuleFor(x => x.QuantityInStock)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.CategoryId)
                .NotEmpty();
        }
    }
}
