using AirWeb.AppServices.Compliance.Enforcement.EnforcementActionCommand;
using AirWeb.TestData.SampleData;
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
    public async Task DtoWithValidValues_ReturnsAsValid()
    {
        // Arrange
        var validator = new EnforcementActionEditValidator();
        var model = new EnforcementActionEditDto
        {
            IssueDate = DateOnly.FromDateTime(DateTime.Today),
            Notes = SampleText.ValidName,
            ResponseRequested = true,
            IsResponseReceived = true,
            ResponseReceived = DateOnly.FromDateTime(DateTime.Today),
            ResponseComment = SampleText.ValidName,
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

    [Test]
    public async Task ResponseDateInFuture_ReturnsAsInvalid()
    {
        // Arrange
        var validator = new EnforcementActionEditValidator();
        var model = new EnforcementActionEditDto
        {
            IssueDate = DateOnly.FromDateTime(DateTime.Today),
            IsResponseReceived = true,
            ResponseReceived = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Test]
    public async Task ResponseDateBeforeIssueDate_ReturnsAsInvalid()
    {
        // Arrange
        var validator = new EnforcementActionEditValidator();
        var model = new EnforcementActionEditDto
        {
            IssueDate = DateOnly.FromDateTime(DateTime.Today),
            IsResponseReceived = true,
            ResponseReceived = DateOnly.FromDateTime(DateTime.Today.AddDays(-1)),
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Test]
    public async Task ResponseDateBeforeIssueDate_WhenIsResponseReceivedIsFalse_ReturnsAsValid()
    {
        // Arrange
        var validator = new EnforcementActionEditValidator();
        var model = new EnforcementActionEditDto
        {
            IssueDate = DateOnly.FromDateTime(DateTime.Today),
            IsResponseReceived = false,
            ResponseReceived = DateOnly.FromDateTime(DateTime.Today.AddDays(-1)),
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public async Task ResponseDateNull_WhenIsResponseReceived_ReturnsAsInvalid()
    {
        // Arrange
        var validator = new EnforcementActionEditValidator();
        var model = new EnforcementActionEditDto
        {
            IssueDate = DateOnly.FromDateTime(DateTime.Today),
            IsResponseReceived = true,
            ResponseReceived = null,
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Test]
    public async Task IsResponseReceived_WhenNotIssued_ReturnsAsInvalid()
    {
        // Arrange
        var validator = new EnforcementActionEditValidator();
        var model = new EnforcementActionEditDto
        {
            IssueDate = null,
            IsResponseReceived = true,
            ResponseReceived = DateOnly.FromDateTime(DateTime.Today),
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeFalse();
    }
}
