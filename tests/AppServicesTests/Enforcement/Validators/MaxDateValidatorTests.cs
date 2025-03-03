using AirWeb.AppServices.CommonDtos;
using FluentValidation.TestHelper;

namespace AppServicesTests.Enforcement.Validators;

public class MaxDateValidatorTests
{
    [Test]
    public async Task ValidMaxDateDto_ReturnsAsValid()
    {
        // Arrange
        var validator = new MaxCurrentDateOnlyValidator();
        var model = new MaxCurrentDateOnlyDto { Date = DateOnly.FromDateTime(DateTime.Today) };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public async Task InvalidMaxDateDto_ReturnsAsInvalid()
    {
        // Arrange
        var validator = new MaxCurrentDateOnlyValidator();
        var model = new MaxCurrentDateOnlyDto
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
        var validator = new CommentAndMaxDateValidator();
        var model = new CommentAndMaxDateDto { Date = DateOnly.FromDateTime(DateTime.Today) };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public async Task InvalidCommentAndMaxDateDto_ReturnsAsInvalid()
    {
        // Arrange
        var validator = new CommentAndMaxDateValidator();
        var model = new CommentAndMaxDateDto
        {
            Date = DateOnly.FromDateTime(DateTime.Today).AddDays(1)
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeFalse();
    }
}
