using AirWeb.AppServices.Compliance.Enforcement.EnforcementActionCommand;
using FluentValidation.TestHelper;

namespace AppServicesTests.Compliance.Enforcement.Validators;

public class LetterOfNoncomplianceValidatorTests
{
    [Test]
    public async Task DefaultValuedDto_ReturnsAsValid()
    {
        // Arrange
        var validator = new LetterOfNoncomplianceEditValidator();
        var model = new LetterOfNoncomplianceEditDto();

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public async Task DtoWithValidDates_ReturnsAsValid()
    {
        // Arrange
        var validator = new LetterOfNoncomplianceEditValidator();
        var model = new LetterOfNoncomplianceEditDto
        {
            IssueDate = DateOnly.FromDateTime(DateTime.Today),
            ResolvedDate = DateOnly.FromDateTime(DateTime.Today),
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public async Task DtoWithOnlyIssuedDate_ReturnsAsValid()
    {
        // Arrange
        var validator = new LetterOfNoncomplianceEditValidator();
        var model = new LetterOfNoncomplianceEditDto
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
        var validator = new LetterOfNoncomplianceEditValidator();
        var model = new LetterOfNoncomplianceEditDto
        {
            IssueDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Test]
    public async Task ResolvedDateInFuture_ReturnsAsInvalid()
    {
        // Arrange
        var validator = new LetterOfNoncomplianceEditValidator();
        var model = new LetterOfNoncomplianceEditDto
        {
            IssueDate = DateOnly.FromDateTime(DateTime.Today),
            ResolvedDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Test]
    public async Task ResolvedWhenNotIssued_ReturnsAsInvalid()
    {
        // Arrange
        var validator = new LetterOfNoncomplianceEditValidator();
        var model = new LetterOfNoncomplianceEditDto
        {
            ResolvedDate = DateOnly.FromDateTime(DateTime.Today),
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Test]
    public async Task ResolvedDateBeforeIssued_ReturnsAsInvalid()
    {
        // Arrange
        var validator = new LetterOfNoncomplianceEditValidator();
        var model = new LetterOfNoncomplianceEditDto
        {
            IssueDate = DateOnly.FromDateTime(DateTime.Today),
            ResolvedDate = DateOnly.FromDateTime(DateTime.Today).AddDays(-1),
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeFalse();
    }
}
