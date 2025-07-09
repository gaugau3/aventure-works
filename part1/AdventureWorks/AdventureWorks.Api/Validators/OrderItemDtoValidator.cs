using FluentValidation;

namespace AdventureWorks.Api.Validators;

public class OrderItemDtoValidator : ValidatorBase<OrderItemDto>
{
    public OrderItemDtoValidator()
    {
        RuleFor(x => x.ProductId)
            .GreaterThan(0).WithMessage("ProductId must be valid");
        RuleFor(x => x.Quantity).GreaterThan((short)0).WithMessage("Quantity must be greater than 0");
        RuleFor(x => x.SpecialOfferId)
            .GreaterThan(0).WithMessage("SpecialOfferId must be greater than 0");
    }
}