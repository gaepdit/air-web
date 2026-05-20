using AirWeb.AppServices.Compliance.Enforcement.EnforcementActionCommand;
using FluentValidation.TestHelper;

namespace AppServicesTests.Compliance.Enforcement.Validators;

public class EnforcementActionEditValidatorTests
{
    [Test]
    public async Task DefaultValuedDto_ReturnsAsValid()
    {
        // Arrange
        var validator = new EnforcementActionEditValidator();
        var model = new EnforcementActionEditDto();

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public async Task DtoWithValidDates_ReturnsAsValid()
    {
        // Arrange
        var validator = new EnforcementActionEditValidator();
        var model = new EnforcementActionEditDto
        {
            IssueDate = DateOnly.FromDateTime(DateTime.Today),
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public async Task IssueDateInFuture_ReturnsAsInvalid()
    {
        // Arrange
        var validator = new EnforcementActionEditValidator();
        var model = new EnforcementActionEditDto
        {
            IssueDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeFalse();
    }
}
