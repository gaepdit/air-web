using AirWeb.AppServices.CommonDtos;
using FluentValidation.TestHelper;

namespace AppServicesTests.Common;

public class MaxCurrentDateOnlyValidatorTests
{
    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        // Arrange
        var model = new MaxCurrentDateOnlyDto { Date = DateOnly.FromDateTime(DateTime.Today) };
        var validator = new MaxCurrentDateOnlyValidator();

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public async Task DateInFuture_ReturnsAsInvalid()
    {
        // Arrange
        var model = new MaxCurrentDateOnlyDto { Date = DateOnly.FromDateTime(DateTime.Today).AddDays(1) };
        var validator = new MaxCurrentDateOnlyValidator();

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeFalse();
    }
}
