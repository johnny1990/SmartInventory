using FluentValidation;
using SmartInventory.Application.Commands;

namespace SmartInventory.Application.Validators
{
    public class CreateStockMovementCommandValidator
        : AbstractValidator<CreateStockMovementCommand>
    {
        public CreateStockMovementCommandValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty();

            RuleFor(x => x.Quantity)
                .GreaterThan(0);

            RuleFor(x => x.Notes)
                .MaximumLength(500);
        }
    }
}
