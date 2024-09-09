using AirWeb.AppServices.Compliance.WorkEntries.Accs;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.TestData.SampleData;
using FluentValidation.TestHelper;

namespace AppServicesTests.WorkEntries.Validators;

public class AccCreateValidatorTests
{
    private readonly AccCreateValidator _validator = new();

    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        // Arrange
        var model = new AccCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
        };

        // Act
        var result = await _validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public async Task AccYearTooOld_ReturnsAsInvalid()
    {
        // Arrange
        var model = new AccCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            AccReportingYear = WorkEntry.EarliestWorkEntryYear - 1,
        };

        // Act
        var result = await _validator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.AccReportingYear);
    }

    [Test]
    public async Task AccYearInFuture_ReturnsAsInvalid()
    {
        // Arrange
        var model = new AccCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            AccReportingYear = DateTime.Today.Year + 1,
        };

        // Act
        var result = await _validator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.AccReportingYear);
    }

    [Test]
    public async Task ReceivedDateInFuture_ReturnsAsInvalid()
    {
        // Arrange
        var model = new AccCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            ReceivedDate = DateOnly.FromDateTime(DateTime.Today).AddDays(1),
        };

        // Act
        var result = await _validator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.ReceivedDate);
    }

    [Test]
    public async Task ReceivedDateTooOld_ReturnsAsInvalid()
    {
        // Arrange
        var model = new AccCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            ReceivedDate = new DateOnly(WorkEntry.EarliestWorkEntryYear - 1, 1, 1),
        };

        // Act
        var result = await _validator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.ReceivedDate);
    }

    [Test]
    public async Task ReceivedDateBeforePostmarkDate_ReturnsAsInvalid()
    {
        // Arrange
        var model = new AccCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            ReceivedDate = DateOnly.FromDateTime(DateTime.Today).AddDays(-2),
            Postmarked = DateOnly.FromDateTime(DateTime.Today).AddDays(-1),
        };

        // Act
        var result = await _validator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.ReceivedDate);
    }

    [Test]
    public async Task PostmarkDateInFuture_ReturnsAsInvalid()
    {
        // Arrange
        var model = new AccCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            Postmarked = DateOnly.FromDateTime(DateTime.Today).AddDays(1),
        };

        // Act
        var result = await _validator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.Postmarked);
    }

    [Test]
    public async Task PostmarkDateTooOld_ReturnsAsInvalid()
    {
        // Arrange
        var model = new AccCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            Postmarked = new DateOnly(WorkEntry.EarliestWorkEntryYear - 1, 1, 1),
        };

        // Act
        var result = await _validator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.Postmarked);
    }
}
