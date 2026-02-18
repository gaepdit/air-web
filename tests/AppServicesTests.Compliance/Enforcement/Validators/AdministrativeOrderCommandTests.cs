using AirWeb.AppServices.Compliance.Enforcement.EnforcementActionCommand;
using FluentValidation.TestHelper;

namespace AppServicesTests.Compliance.Enforcement.Validators;

public class AdministrativeOrderCommandTests
{
    [Test]
    public async Task NullValuedDto_ReturnsAsValid()
    {
        // Arrange
        var validator = new AdministrativeOrderCommandValidator();
        var model = new AdministrativeOrderCommandDto();

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public async Task DtoWithValidDates_ReturnsAsValid()
    {
        // Arrange
        var validator = new AdministrativeOrderCommandValidator();
        var model = new AdministrativeOrderCommandDto
        {
            ExecutedDate = DateOnly.FromDateTime(DateTime.Today),
            IssueDate = DateOnly.FromDateTime(DateTime.Today),
            AppealedDate = DateOnly.FromDateTime(DateTime.Today),
            ResolvedDate = DateOnly.FromDateTime(DateTime.Today),
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public async Task ExecutedDateInFuture_ReturnsAsInvalid()
    {
        // Arrange
        var validator = new AdministrativeOrderCommandValidator();
        var model = new AdministrativeOrderCommandDto
        {
            ExecutedDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Test]
    public async Task IssueDateInFuture_ReturnsAsInvalid()
    {
        // Arrange
        var validator = new AdministrativeOrderCommandValidator();
        var model = new AdministrativeOrderCommandDto
        {
            ExecutedDate = DateOnly.FromDateTime(DateTime.Today),
            IssueDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Test]
    public async Task AppealedDateInFuture_ReturnsAsInvalid()
    {
        // Arrange
        var validator = new AdministrativeOrderCommandValidator();
        var model = new AdministrativeOrderCommandDto
        {
            ExecutedDate = DateOnly.FromDateTime(DateTime.Today),
            IssueDate = DateOnly.FromDateTime(DateTime.Today),
            AppealedDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
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
        var validator = new AdministrativeOrderCommandValidator();
        var model = new AdministrativeOrderCommandDto
        {
            ExecutedDate = DateOnly.FromDateTime(DateTime.Today),
            IssueDate = DateOnly.FromDateTime(DateTime.Today),
            AppealedDate = DateOnly.FromDateTime(DateTime.Today),
            ResolvedDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Test]
    public async Task IssueDateBeforeExecuted_ReturnsAsInvalid()
    {
        // Arrange
        var validator = new AdministrativeOrderCommandValidator();
        var model = new AdministrativeOrderCommandDto
        {
            ExecutedDate = DateOnly.FromDateTime(DateTime.Today),
            IssueDate = DateOnly.FromDateTime(DateTime.Today).AddDays(-1),
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Test]
    public async Task IssuedWhenNotExecuted_ReturnsAsInvalid()
    {
        // Arrange
        var validator = new AdministrativeOrderCommandValidator();
        var model = new AdministrativeOrderCommandDto
        {
            IssueDate = DateOnly.FromDateTime(DateTime.Today),
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Test]
    public async Task AppealedDateBeforeIssued_ReturnsAsInvalid()
    {
        // Arrange
        var validator = new AdministrativeOrderCommandValidator();
        var model = new AdministrativeOrderCommandDto
        {
            ExecutedDate = DateOnly.FromDateTime(DateTime.Today).AddDays(-2),
            IssueDate = DateOnly.FromDateTime(DateTime.Today),
            AppealedDate = DateOnly.FromDateTime(DateTime.Today).AddDays(-1),
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Test]
    public async Task AppealedWhenNotIssued_ReturnsAsInvalid()
    {
        // Arrange
        var validator = new AdministrativeOrderCommandValidator();
        var model = new AdministrativeOrderCommandDto
        {
            ExecutedDate = DateOnly.FromDateTime(DateTime.Today),
            AppealedDate = DateOnly.FromDateTime(DateTime.Today),
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Test]
    public async Task ResolvedDateBeforeAppealed_ReturnsAsInvalid()
    {
        // Arrange
        var validator = new AdministrativeOrderCommandValidator();
        var model = new AdministrativeOrderCommandDto
        {
            ExecutedDate = DateOnly.FromDateTime(DateTime.Today).AddDays(-2),
            IssueDate = DateOnly.FromDateTime(DateTime.Today).AddDays(-2),
            AppealedDate = DateOnly.FromDateTime(DateTime.Today),
            ResolvedDate = DateOnly.FromDateTime(DateTime.Today).AddDays(-1),
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
        var validator = new AdministrativeOrderCommandValidator();
        var model = new AdministrativeOrderCommandDto
        {
            ExecutedDate = DateOnly.FromDateTime(DateTime.Today).AddDays(-2),
            IssueDate = DateOnly.FromDateTime(DateTime.Today),
            ResolvedDate = DateOnly.FromDateTime(DateTime.Today).AddDays(-1),
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Test]
    public async Task ResolvedDateBeforeExecuted_ReturnsAsInvalid()
    {
        // Arrange
        var validator = new AdministrativeOrderCommandValidator();
        var model = new AdministrativeOrderCommandDto
        {
            ExecutedDate = DateOnly.FromDateTime(DateTime.Today).AddDays(-1),
            ResolvedDate = DateOnly.FromDateTime(DateTime.Today).AddDays(-2),
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Test]
    public async Task ResolvedWhenNotAppealed_ReturnsAsValid()
    {
        // Arrange
        var validator = new AdministrativeOrderCommandValidator();
        var model = new AdministrativeOrderCommandDto
        {
            ExecutedDate = DateOnly.FromDateTime(DateTime.Today),
            IssueDate = DateOnly.FromDateTime(DateTime.Today),
            ResolvedDate = DateOnly.FromDateTime(DateTime.Today),
        };

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}
