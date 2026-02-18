using AirWeb.AppServices.Sbeap.Customers.Dto;
using AirWeb.AppServices.Sbeap.Customers.Validators;
using AirWeb.Domain.Core.Entities;
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
        var validator = new CustomerUpdateValidator(Substitute.For<ISicCodeRepository>());

        var result = await validator.TestValidateAsync(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task NameTooShort_ReturnsAsInvalid()
    {
        var model = DefaultCustomerUpdate with { Name = Constants.ShortName };
        var validator = new CustomerUpdateValidator(Substitute.For<ISicCodeRepository>());

        var result = await validator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(e => e.Name);
    }

    [Test]
    public async Task CustomerWithValidSic_ReturnsAsValid()
    {
        var model = DefaultCustomerUpdate with
        {
            Name = Constants.ValidName,
            SicCodeId = "0000",
        };
        var sic = Substitute.For<ISicCodeRepository>();
        sic.ExistsAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(true);
        var validator = new CustomerUpdateValidator(sic);

        var result = await validator.TestValidateAsync(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task CustomerWithInvalidSic_ReturnsAsInvalid()
    {
        var model = DefaultCustomerUpdate with
        {
            Name = Constants.ShortName,
            SicCodeId = "0000",
        };
        var sic = Substitute.For<ISicCodeRepository>();
        sic.ExistsAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(false);
        var validator = new CustomerUpdateValidator(sic);

        var result = await validator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(e => e.Name);
    }
}
