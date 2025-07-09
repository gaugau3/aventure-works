using FluentValidation;

namespace AdventureWorks.Api.Validators;

public class OrderDtoValidator : ValidatorBase<OrderDto>
{
    public OrderDtoValidator()
    {
        RuleFor(x => x.CustomerId)
            .GreaterThan(0).WithMessage("CustomerId must be greater than 0");

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("Order must contain at least one item");

        RuleFor(x => x.BillToAddressId)
            .GreaterThan(0).WithMessage("Billing address is required");

        RuleFor(x => x.ShipToAddressId)
            .GreaterThan(0).WithMessage("Shipping address is required");

        RuleFor(x => x.ShipMethodId)
            .GreaterThan(0).WithMessage("Shipping method is required");

        RuleForEach(x => x.Items).SetValidator(new OrderItemDtoValidator());
    }
}
