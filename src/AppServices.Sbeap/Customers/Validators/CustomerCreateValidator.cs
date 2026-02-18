using AirWeb.AppServices.Sbeap.Customers.Dto;
using AirWeb.Domain.Core.Entities;
using AirWeb.Domain.Sbeap.Entities.Customers;
using FluentValidation;

namespace AirWeb.AppServices.Sbeap.Customers.Validators;

public class CustomerCreateValidator : AbstractValidator<CustomerCreateDto>
{
    public CustomerCreateValidator(ISicCodeRepository sic, IValidator<ContactCreateDto> contactValidator)
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
            .MustAsync(async (id, token) => id is null || await sic.ExistsAsync(id, token).ConfigureAwait(false))
            .WithMessage(_ => "The SIC Code entered does not exist.");

        // Embedded Contact
        RuleFor(e => e.Contact)
            .SetValidator(contactValidator)
            .When(e => !e.Contact.IsEmpty);
    }
}
