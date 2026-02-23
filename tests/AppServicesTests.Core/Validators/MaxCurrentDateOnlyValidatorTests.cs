using AirWeb.AppServices.Core.CommonDtos;
using FluentValidation.TestHelper;

namespace AppServicesTests.Core.Validators;

public class MaxDateOnlyValidatorTests
{
    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        // Arrange
        var model = new MaxDateOnlyDto { Date = DateOnly.FromDateTime(DateTime.Today) };
        var validator = new MaxDateOnlyValidator();

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public async Task DateInFuture_ReturnsAsInvalid()
    {
        // Arrange
        var model = new MaxDateOnlyDto { Date = DateOnly.FromDateTime(DateTime.Today).AddDays(1) };
        var validator = new MaxDateOnlyValidator();

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeFalse();
    }
}
