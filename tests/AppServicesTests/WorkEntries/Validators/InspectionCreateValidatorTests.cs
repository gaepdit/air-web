using AirWeb.AppServices.Compliance.WorkEntries.Inspections;
using AirWeb.Domain;
using AirWeb.TestData.SampleData;
using FluentValidation.TestHelper;

namespace AppServicesTests.WorkEntries.Validators;

public class InspectionCreateValidatorTests
{
    private readonly InspectionCreateValidator _validator = new();

    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        // Arrange
        var model = new InspectionCreateDto
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
    public async Task InspectionStartedDateInFuture_ReturnsAsInvalid()
    {
        // Arrange
        var model = new InspectionCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            InspectionStartedDate = DateOnly.FromDateTime(DateTime.Today).AddDays(1),
        };

        // Act
        var result = await _validator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.InspectionStartedDate);
    }

    [Test]
    public async Task InspectionStartedDateTooOld_ReturnsAsInvalid()
    {
        // Arrange
        var model = new InspectionCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            InspectionStartedDate = new DateOnly(ComplianceConstants.EarliestWorkEntryYear - 1, 1, 1),
        };

        // Act
        var result = await _validator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.InspectionStartedDate);
    }

    [Test]
    public async Task InspectionEndedDateInFuture_ReturnsAsInvalid()
    {
        // Arrange
        var model = new InspectionCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            InspectionEndedDate = DateOnly.FromDateTime(DateTime.Today).AddDays(1),
        };

        // Act
        var result = await _validator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.InspectionEndedDate);
    }

    [Test]
    public async Task InspectionEndedDateTooOld_ReturnsAsInvalid()
    {
        // Arrange
        var model = new InspectionCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            InspectionEndedDate = new DateOnly(ComplianceConstants.EarliestWorkEntryYear - 1, 1, 1),
        };

        // Act
        var result = await _validator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.InspectionEndedDate);
    }

    [Test]
    public async Task InspectionEndedDateBeforeStartedDate_ReturnsAsInvalid()
    {
        // Arrange
        var model = new InspectionCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            InspectionStartedDate = DateOnly.FromDateTime(DateTime.Today).AddDays(-2),
            InspectionEndedDate = DateOnly.FromDateTime(DateTime.Today).AddDays(-3),
        };

        // Act
        var result = await _validator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.InspectionEndedDate);
    }

    [Test]
    public async Task AcknowledgmentLetterDateBeforeInspectionEndedDate_ReturnsAsInvalid()
    {
        // Arrange
        var model = new InspectionCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            InspectionEndedDate = DateOnly.FromDateTime(DateTime.Today).AddDays(-1),
            AcknowledgmentLetterDate = DateOnly.FromDateTime(DateTime.Today).AddDays(-2),
        };

        // Act
        var result = await _validator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.AcknowledgmentLetterDate);
    }
}
