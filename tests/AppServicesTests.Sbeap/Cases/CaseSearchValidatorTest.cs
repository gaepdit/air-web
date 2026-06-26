using AirWeb.AppServices.Sbeap.Cases.Dto;
using AirWeb.AppServices.Sbeap.Cases.Validators;
using AwesomeAssertions.Execution;
using FluentValidation.TestHelper;

namespace AppServicesTests.Compliance.Enforcement.Validators;

public class CaseSearchValidatorTests
{
    private CaseSearchValidator _sut = null;

    [SetUp]
    public void SetUp()
    {
        // Arrange
        _sut = new CaseSearchValidator();
    }
    [Test]
    public async Task EmptyDto_ReturnsAsValid()
    {
        // Arrange
        var model = new CaseworkSearchDto();

        // Act
        var result = await _sut.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        // Arrange
        var model = new CaseworkSearchDto
        {
            OpenedFrom = DateOnly.FromDateTime(DateTime.Today.AddDays(-5)),
            OpenedThrough = DateOnly.FromDateTime(DateTime.Today),
            ClosedFrom = DateOnly.FromDateTime(DateTime.Today.AddDays(-5)),
            ClosedThrough = DateOnly.FromDateTime(DateTime.Today),
            ReferredFrom = DateOnly.FromDateTime(DateTime.Today.AddDays(-5)),
            ReferredThrough = DateOnly.FromDateTime(DateTime.Today)
        };

        // Act
        var result = await _sut.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }
    [Test]
    public async Task OpenedFromInFuture_ReturnsAsInvalid()
    {
        // Arrange
        var model = new CaseworkSearchDto
        {
            OpenedFrom = DateOnly.FromDateTime(DateTime.Today).AddDays(1),
        };

        // Act
        var result = await _sut.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.OpenedFrom);
    }

    [Test]
    public async Task OpenedThroughIsBeforeOpenedFrom_ReturnsAsInvalid()
    {
        // Arrange
        var model = new CaseworkSearchDto
        {
            OpenedFrom = DateOnly.FromDateTime(DateTime.Today).AddDays(-1),
            OpenedThrough = DateOnly.FromDateTime(DateTime.Today).AddDays(-5)
        };

        // Act
        var results = await _sut.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        results.IsValid.Should().BeFalse();
        results.ShouldHaveValidationErrorFor(dto => dto.OpenedThrough);
    }
    [Test]
    public async Task ClosedFromInFuture_ReturnsAsInvalid()
    {
        // Arrange
        var model = new CaseworkSearchDto
        {
            ClosedFrom = DateOnly.FromDateTime(DateTime.Today).AddDays(1),
        };

        // Act
        var result = await _sut.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.ClosedFrom);
    }

    [Test]
    public async Task ClosedThroughIsBeforeClosedFrom_ReturnsAsInvalid()
    {
        // Arrange
        var model = new CaseworkSearchDto
        {
            ClosedFrom = DateOnly.FromDateTime(DateTime.Today).AddDays(-1),
            ClosedThrough = DateOnly.FromDateTime(DateTime.Today).AddDays(-5)
        };

        // Act
        var results = await _sut.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        results.IsValid.Should().BeFalse();
        results.ShouldHaveValidationErrorFor(dto => dto.ClosedThrough);
    }
    [Test]
    public async Task ReferredFromInFuture_ReturnsAsInvalid()
    {
        // Arrange
        var model = new CaseworkSearchDto
        {
            ReferredFrom = DateOnly.FromDateTime(DateTime.Today).AddDays(1),
        };

        // Act
        var result = await _sut.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.ReferredFrom);
    }

    [Test]
    public async Task ReferredThroughIsBeforeReferredFrom_ReturnsAsInvalid()
    {
        // Arrange
        var model = new CaseworkSearchDto
        {
            ReferredFrom = DateOnly.FromDateTime(DateTime.Today).AddDays(-1),
            ReferredThrough = DateOnly.FromDateTime(DateTime.Today).AddDays(-5)
        };

        // Act
        var results = await _sut.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        results.IsValid.Should().BeFalse();
        results.ShouldHaveValidationErrorFor(dto => dto.ReferredThrough);
    }

}
