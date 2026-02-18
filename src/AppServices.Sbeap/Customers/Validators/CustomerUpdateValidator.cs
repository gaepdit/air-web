using AirWeb.AppServices.Sbeap.Customers.Dto;
using AirWeb.Domain.Core.Data;
using AirWeb.Domain.Sbeap.Entities.Customers;
using FluentValidation;

namespace AirWeb.AppServices.Sbeap.Customers.Validators;

public class CustomerUpdateValidator : AbstractValidator<CustomerUpdateDto>
{
    public CustomerUpdateValidator()
    {
        RuleFor(e => e.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MinimumLength(Customer.MinNameLength);

        RuleFor(e => e.SicCodeId)
            .Must(id => id is null || SicCodes.Exists(id))
            .WithMessage(_ => "The SIC Code entered does not exist or is not active.");
    }
}
