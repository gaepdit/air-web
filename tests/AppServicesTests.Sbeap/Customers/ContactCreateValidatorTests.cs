using AirWeb.AppServices.Sbeap.Customers.Dto;
using AirWeb.AppServices.Sbeap.Customers.Validators;
using AppServicesTests.Sbeap.TestData;
using FluentValidation.TestHelper;

namespace AppServicesTests.Sbeap.Customers;

public class ContactCreateValidatorTests
{
    private static ContactCreateDto EmptyContactCreateDto => new(Guid.Empty);
    private static readonly PhoneNumberCreateValidator PhoneNumberValidator = new();
    private static readonly ContactCreateValidator ContactValidator = new(PhoneNumberValidator);

    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        var model = EmptyContactCreateDto with
        {
            GivenName = Constants.ValidName,
            Email = Constants.ValidEmail,
        };

        var result = await ContactValidator.TestValidateAsync(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task EmptyContact_ReturnsAsInvalid()
    {
        var result = await ContactValidator.TestValidateAsync(EmptyContactCreateDto);

        result.ShouldHaveValidationErrorFor(e => e.Title);
    }

    [Test]
    public async Task InvalidEmail_ReturnsAsInvalid()
    {
        var model = EmptyContactCreateDto with
        {
            Title = Constants.Phrase,
            Email = Constants.NonExistentName, // invalid as email
        };

        var result = await ContactValidator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(e => e.Email);
    }

    [Test]
    public async Task MissingNameOrTitle_ReturnsAsInvalid()
    {
        var model = EmptyContactCreateDto with
        {
            Email = Constants.ValidEmail,
        };

        var result = await ContactValidator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(e => e.Title);
    }
}
