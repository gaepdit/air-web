using AirWeb.AppServices.Sbeap.Cases.Dto;
using AirWeb.AppServices.Sbeap.Cases.Validators;
using FluentValidation.TestHelper;

namespace AppServicesTests.Sbeap.Cases;

public class UpdateValidatorTests
{
    private static CaseworkUpdateDto DefaultCaseworkUpdate => new();

    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        var validator = new CaseworkUpdateValidator();
        var result = await validator.TestValidateAsync(DefaultCaseworkUpdate);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task InvalidClosedDate_ReturnsAsInvalid()
    {
        var invalidClosedDate = DefaultCaseworkUpdate with
        {
            CaseOpenedDate = DateOnly.MaxValue,
            CaseClosedDate = DateOnly.MinValue,
        };
        var validator = new CaseworkUpdateValidator();
        var result = await validator.TestValidateAsync(invalidClosedDate);
        result.ShouldHaveValidationErrorFor(e => e.CaseClosedDate);
    }

    [Test]
    public async Task InvalidReferralDate_ReturnsAsInvalid()
    {
        var invalidClosedDate = DefaultCaseworkUpdate with
        {
            CaseOpenedDate = DateOnly.MaxValue,
            ReferralDate = DateOnly.MinValue,
        };
        var validator = new CaseworkUpdateValidator();
        var result = await validator.TestValidateAsync(invalidClosedDate);
        result.ShouldHaveValidationErrorFor(e => e.ReferralDate);
    }
}
