using AirWeb.AppServices.Core.CommonDtos;
using FluentValidation.TestHelper;

namespace AppServicesTests.Core.Validators;

public class MaxDateValidatorTests
{
    [Test]
    public async Task ValidMaxDateDto_ReturnsAsValid()
    {
        // Arrange
        var validator = new MaxDateOnlyValidator();
        var model = new MaxDateOnlyDto { Date = DateOnly.FromDateTime(DateTime.Today) };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public async Task InvalidMaxDateDto_ReturnsAsInvalid()
    {
        // Arrange
        var validator = new MaxDateOnlyValidator();
        var model = new MaxDateOnlyDto
        {
            Date = DateOnly.FromDateTime(DateTime.Today).AddDays(1)
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Test]
    public async Task ValidCommentAndMaxDateDto_ReturnsAsValid()
    {
        // Arrange
        var validator = new MaxDateAndCommentValidator();
        var model = new MaxDateAndCommentDto { Date = DateOnly.FromDateTime(DateTime.Today) };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public async Task InvalidCommentAndMaxDateDto_ReturnsAsInvalid()
    {
        // Arrange
        var validator = new MaxDateAndCommentValidator();
        var model = new MaxDateAndCommentDto
        {
            Date = DateOnly.FromDateTime(DateTime.Today).AddDays(1)
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeFalse();
    }
}
