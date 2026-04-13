using AirWeb.AppServices.Sbeap.Customers.Dto;
using AirWeb.AppServices.Sbeap.Customers.Validators;
using AirWeb.Domain.Core.Data;
using AppServicesTests.Sbeap.TestData;
using FluentValidation.TestHelper;

namespace AppServicesTests.Sbeap.Customers;

public class UpdateValidatorTests
{
    private static CustomerUpdateDto DefaultCustomerUpdate => new();

    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        var model = DefaultCustomerUpdate with { Name = Constants.ValidName };
        var validator = new CustomerUpdateValidator();

        var result = await validator.TestValidateAsync(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task NameTooShort_ReturnsAsInvalid()
    {
        var model = DefaultCustomerUpdate with { Name = Constants.ShortName };
        var validator = new CustomerUpdateValidator();

        var result = await validator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(e => e.Name);
    }

    [Test]
    public async Task CustomerWithValidSic_ReturnsAsValid()
    {
        var model = DefaultCustomerUpdate with
        {
            Name = Constants.ValidName,
            SicCode = SicData.Sics.First().Code,
        };
        var validator = new CustomerUpdateValidator();

        var result = await validator.TestValidateAsync(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task CustomerWithInvalidSic_ReturnsAsInvalid()
    {
        var model = DefaultCustomerUpdate with
        {
            Name = Constants.ShortName,
            SicCode = "0000",
        };
        var validator = new CustomerUpdateValidator();

        var result = await validator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(e => e.Name);
    }
}
