using AirWeb.AppServices.Sbeap.Customers.Dto;
using AirWeb.Domain.Core.Data;
using AirWeb.Domain.Sbeap.Entities.Customers;
using FluentValidation;

namespace AirWeb.AppServices.Sbeap.Customers.Validators;

public class CustomerCreateValidator : AbstractValidator<CustomerCreateDto>
{
    public CustomerCreateValidator(IValidator<ContactCreateDto> contactValidator)
    {
        RuleFor(e => e.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MinimumLength(Customer.MinNameLength);

        RuleFor(e => e.Website)
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
            .WithMessage("The Website must be a valid web address.")
            .When(x => !string.IsNullOrEmpty(x.Website));

        RuleFor(e => e.SicCodeId)
            .Must(id => id is null || SicCodes.Exists(id))
            .WithMessage(_ => "The SIC Code entered does not exist or is not active.");

        // Embedded Contact
        RuleFor(e => e.Contact)
            .SetValidator(contactValidator)
            .When(e => !e.Contact.IsEmpty);
    }
}
