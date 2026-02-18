using AirWeb.AppServices.Sbeap.Customers.Dto;
using AirWeb.AppServices.Sbeap.Customers.Validators;
using AirWeb.Domain.Core.Data;
using AppServicesTests.Sbeap.TestData;
using FluentValidation.TestHelper;

namespace AppServicesTests.Sbeap.Customers;

public class CustomerCreateValidatorTests
{
    private static ContactCreateDto EmptyContactCreate => new(Guid.Empty);
    private static CustomerCreateDto EmptyCustomerCreate => new();
    private static readonly PhoneNumberCreateValidator PhoneNumberValidator = new();
    private static readonly ContactCreateValidator ContactValidator = new(PhoneNumberValidator);

    private static readonly CustomerCreateValidator CustomerValidator =
        new(ContactValidator);

    [Test]
    public async Task ValidDtoWithEmptyContact_ReturnsAsValid()
    {
        var model = EmptyCustomerCreate with
        {
            Name = Constants.ValidName,
            Contact = EmptyContactCreate,
        };

        var result = await CustomerValidator.TestValidateAsync(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task ValidDtoWithValidContact_ReturnsAsValid()
    {
        var model = EmptyCustomerCreate with
        {
            Name = Constants.ValidName,
            Contact = EmptyContactCreate with { Title = Constants.Phrase },
        };

        var result = await CustomerValidator.TestValidateAsync(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task NameTooShort_ReturnsAsInvalid()
    {
        var model = EmptyCustomerCreate with
        {
            Name = Constants.ShortName,
            Contact = EmptyContactCreate,
        };

        var result = await CustomerValidator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(e => e.Name);
    }

    [Test]
    public async Task InvalidWebsite_ReturnsAsInvalid()
    {
        var model = EmptyCustomerCreate with
        {
            Name = Constants.ValidName,
            Website = Constants.NonExistentName, // invalid as website
            Contact = EmptyContactCreate,
        };

        var result = await CustomerValidator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(e => e.Website);
    }

    [Test]
    public async Task InvalidContactEmail_ReturnsAsInvalid()
    {
        var model = EmptyCustomerCreate with
        {
            Name = Constants.ValidName,
            Contact = EmptyContactCreate with
            {
                Title = Constants.Phrase,
                Email = Constants.NonExistentName, // invalid as email
            },
        };

        var result = await CustomerValidator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(e => e.Contact.Email);
    }

    [Test]
    public async Task ContactWithoutNameOrTitle_ReturnsAsInvalid()
    {
        var model = EmptyCustomerCreate with
        {
            Name = Constants.ValidName,
            Contact = EmptyContactCreate with { Email = Constants.ValidEmail },
        };

        var result = await CustomerValidator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(e => e.Contact.Title);
    }

    [Test]
    public async Task CustomerWithValidSic_ReturnsAsValid()
    {
        // Arrange
        var model = EmptyCustomerCreate with
        {
            Name = Constants.ValidName,
            Contact = EmptyContactCreate,
            SicCodeId = SicCodes.Data.First().Id,
        };

        var validator = new CustomerCreateValidator(ContactValidator);

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task CustomerWithInvalidSic_ReturnsAsInvalid()
    {
        // Arrange
        var model = EmptyCustomerCreate with
        {
            Name = Constants.ValidName,
            Contact = EmptyContactCreate,
            SicCodeId = "0000",
        };
        var validator = new CustomerCreateValidator(ContactValidator);

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldHaveValidationErrorFor(e => e.SicCodeId);
    }
}
