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

        RuleFor(e => e.Website)
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
            .WithMessage("The Website must be a valid web address.")
            .When(x => !string.IsNullOrEmpty(x.Website));

        RuleFor(e => e.SicCode)
            .Must(id => id is null || SicData.Exists(id))
            .WithMessage(_ => "The SIC Code entered does not exist or is not active.");
    }
}
