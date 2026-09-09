using AirWeb.AppServices.Compliance.Enforcement.EnforcementActionCommand;
using FluentValidation;
using FluentValidation.TestHelper;

namespace AppServicesTests.Compliance.Enforcement.Validators;

public class EnforcementActionAddResponseValidatorTests
{
    [Test]
    public async Task DefaultValuedDto_ReturnsAsValid()
    {
        // Arrange
        var model = new EnforcementActionAddResponseDto();

        var context = new ValidationContext<EnforcementActionAddResponseDto>(model)
        {
            RootContextData = { ["enforcementAction.IssueDate"] = DateOnly.FromDateTime(DateTime.Today) },
        };

        var validator = new EnforcementActionAddResponseValidator();

        // Act
        var result = await validator.TestValidateAsync(context);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public async Task ResponseDateInFuture_ReturnsAsInvalid()
    {
        // Arrange
        var model = new EnforcementActionAddResponseDto
        {
            Date = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
        };

        var context = new ValidationContext<EnforcementActionAddResponseDto>(model)
        {
            RootContextData = { ["enforcementAction.IssueDate"] = DateOnly.FromDateTime(DateTime.Today) },
        };

        var validator = new EnforcementActionAddResponseValidator();

        // Act
        var result = await validator.TestValidateAsync(context);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Test]
    public async Task ResponseDateBeforeIssueDate_ReturnsAsInvalid()
    {
        // Arrange
        var model = new EnforcementActionAddResponseDto
        {
            Date = DateOnly.FromDateTime(DateTime.Today.AddDays(-1)),
        };

        var context = new ValidationContext<EnforcementActionAddResponseDto>(model)
        {
            RootContextData = { ["enforcementAction.IssueDate"] = DateOnly.FromDateTime(DateTime.Today) },
        };

        var validator = new EnforcementActionAddResponseValidator();

        // Act
        var result = await validator.TestValidateAsync(context);

        // Assert
        result.IsValid.Should().BeFalse();
    }
}
