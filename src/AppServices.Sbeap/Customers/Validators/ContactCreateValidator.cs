using AirWeb.AppServices.Sbeap.Customers.Dto;
using FluentValidation;

namespace AirWeb.AppServices.Sbeap.Customers.Validators;

public class ContactCreateValidator : AbstractValidator<ContactCreateDto>
{
    public ContactCreateValidator(IValidator<PhoneNumberCreate> phoneNumberCreateValidator)
    {
        RuleFor(e => e.Email).EmailAddress();

        RuleFor(e => e.Title)
            .Must((c, _) =>
                !string.IsNullOrWhiteSpace(c.GivenName) ||
                !string.IsNullOrWhiteSpace(c.FamilyName) ||
                !string.IsNullOrWhiteSpace(c.Title))
            .WithMessage("At least a name or title must be entered to create a contact.");

        // Embedded phone number
        RuleFor(e => e.PhoneNumber)
            .SetValidator(phoneNumberCreateValidator)
            .When(e => !e.PhoneNumber.IsIncomplete);
    }
}
