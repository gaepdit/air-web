using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.ComplianceWorkDto.Command;
using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.Inspections;
using AirWeb.Domain.Compliance;
using AirWeb.TestData.SampleData;
using FluentValidation.TestHelper;

namespace AppServicesTests.Compliance.ComplianceMonitoring.Validators;

public class InspectionCreateValidatorTests
{
    private static readonly ComplianceWorkCommandValidator ComplianceWorkCommandValidator = new();

    private static readonly ComplianceWorkCreateValidator ComplianceWorkCreateValidator =
        new(ComplianceWorkCommandValidator);

    private static readonly InspectionCommandValidator InspectionCommandValidator = new();

    private readonly InspectionCreateValidator _validator = new(ComplianceWorkCreateValidator,
        InspectionCommandValidator);

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
    public async Task ValidMultidayDto_ReturnsAsValid()
    {
        // Arrange
        var model = new InspectionCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            MultiDayInspection = true,
        };

        // Act
        var result = await _validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public async Task InspectionStartedDateInFuture_IsInvalid()
    {
        // Arrange
        var model = new InspectionCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            InspectionStartedDate = DateOnly.FromDateTime(DateTime.Today).AddDays(1),
            MultiDayInspection = false,
        };

        // Act
        var result = await _validator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.InspectionStartedDate);
    }

    [Test]
    public async Task InspectionStartedDateTooOld_IsInvalid()
    {
        // Arrange
        var model = new InspectionCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            InspectionStartedDate = new DateOnly(ComplianceConstants.EarliestComplianceWorkYear - 1, 1, 1),
            MultiDayInspection = false,
        };

        // Act
        var result = await _validator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.InspectionStartedDate);
    }

    [Test]
    public async Task InspectionEndedDateInFuture_IsInvalid()
    {
        // Arrange
        var model = new InspectionCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            InspectionStartedDate = DateOnly.FromDateTime(DateTime.Today),
            InspectionEndedDate = DateOnly.FromDateTime(DateTime.Today).AddDays(1),
            MultiDayInspection = true,
        };

        // Act
        var result = await _validator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.InspectionEndedDate);
    }

    [Test]
    public async Task InspectionEndedDateTooOld_IsInvalid()
    {
        // Arrange
        var model = new InspectionCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            InspectionStartedDate = DateOnly.FromDateTime(DateTime.Today),
            InspectionEndedDate = new DateOnly(ComplianceConstants.EarliestComplianceWorkYear - 1, 1, 1),
            MultiDayInspection = true,
        };

        // Act
        var result = await _validator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.InspectionEndedDate);
    }

    [Test]
    public async Task InspectionEndedDateBeforeStartedDate_IsInvalid()
    {
        // Arrange
        var model = new InspectionCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            InspectionStartedDate = DateOnly.FromDateTime(DateTime.Today),
            InspectionEndedDate = DateOnly.FromDateTime(DateTime.Today).AddDays(-1),
            MultiDayInspection = true,
        };

        // Act
        var result = await _validator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.InspectionEndedDate);
    }

    [Test]
    public async Task AcknowledgmentLetterDateBeforeInspectionEndedDate_IsInvalid()
    {
        // Arrange
        var model = new InspectionCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            InspectionStartedDate = DateOnly.FromDateTime(DateTime.Today).AddDays(-2),
            InspectionEndedDate = DateOnly.FromDateTime(DateTime.Today),
            AcknowledgmentLetterDate = DateOnly.FromDateTime(DateTime.Today).AddDays(-1),
            MultiDayInspection = true,
        };

        // Act
        var result = await _validator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.AcknowledgmentLetterDate);
    }

    [Test]
    public async Task NotMultiday_InspectionEndedDateInFuture_Ignored()
    {
        // Arrange
        var model = new InspectionCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            InspectionStartedDate = DateOnly.FromDateTime(DateTime.Today),
            InspectionEndedDate = DateOnly.FromDateTime(DateTime.Today).AddDays(1),
            MultiDayInspection = false,
        };

        // Act
        var result = await _validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public async Task NotMultiday_InspectionEndedDateTooOld_Ignored()
    {
        // Arrange
        var model = new InspectionCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            InspectionStartedDate = DateOnly.FromDateTime(DateTime.Today),
            InspectionEndedDate = new DateOnly(ComplianceConstants.EarliestComplianceWorkYear - 1, 1, 1),
            MultiDayInspection = false,
        };

        // Act
        var result = await _validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public async Task NotMultiday_InspectionEndedDateBeforeStartedDate_Ignored()
    {
        // Arrange
        var model = new InspectionCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            InspectionStartedDate = DateOnly.FromDateTime(DateTime.Today),
            InspectionEndedDate = DateOnly.FromDateTime(DateTime.Today).AddDays(-1),
            MultiDayInspection = false,
        };

        // Act
        var result = await _validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public async Task NotMultiday_AcknowledgmentLetterDateBeforeInspectionEndedDate_Ignored()
    {
        // Arrange
        var model = new InspectionCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            InspectionStartedDate = DateOnly.FromDateTime(DateTime.Today).AddDays(-2),
            InspectionEndedDate = DateOnly.FromDateTime(DateTime.Today),
            AcknowledgmentLetterDate = DateOnly.FromDateTime(DateTime.Today).AddDays(-1),
            MultiDayInspection = false,
        };

        // Act
        var result = await _validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public async Task NotMultiday_AcknowledgmentLetterDateBeforeInspectionStart_IsInvalid()
    {
        // Arrange
        var model = new InspectionCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            InspectionStartedDate = DateOnly.FromDateTime(DateTime.Today).AddDays(-1),
            InspectionEndedDate = DateOnly.FromDateTime(DateTime.Today),
            AcknowledgmentLetterDate = DateOnly.FromDateTime(DateTime.Today).AddDays(-2),
            MultiDayInspection = false,
        };

        // Act
        var result = await _validator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.AcknowledgmentLetterDate);
    }

    [Test]
    public async Task NotMultiday_EndTimeBeforeStartTime_IsInvalid()
    {
        // Arrange
        var model = new InspectionCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            InspectionStartedDate = DateOnly.FromDateTime(DateTime.Today).AddDays(-1),
            InspectionEndedDate = DateOnly.FromDateTime(DateTime.Today),
            InspectionStartedTime = TimeOnly.FromDateTime(DateTime.Now.AddHours(-1)),
            InspectionEndedTime = TimeOnly.FromDateTime(DateTime.Now.AddHours(-2)),
            MultiDayInspection = false,
        };

        // Act
        var result = await _validator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.InspectionEndedTime);
    }

    [Test]
    public async Task Multiday_EndTimeBeforeStartTime_EndDateLaterThanStartDate_IsValid()
    {
        // Arrange
        var model = new InspectionCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            InspectionStartedDate = DateOnly.FromDateTime(DateTime.Today).AddDays(-1),
            InspectionEndedDate = DateOnly.FromDateTime(DateTime.Today),
            InspectionStartedTime = TimeOnly.FromDateTime(DateTime.Now.AddHours(-1)),
            InspectionEndedTime = TimeOnly.FromDateTime(DateTime.Now.AddHours(-2)),
            MultiDayInspection = true,
        };

        // Act
        var result = await _validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public async Task Multiday_EndTimeBeforeStartTime_EndDateSameAsStartDate_IsInvalid()
    {
        // Arrange
        var model = new InspectionCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            InspectionStartedDate = DateOnly.FromDateTime(DateTime.Today),
            InspectionEndedDate = DateOnly.FromDateTime(DateTime.Today),
            InspectionStartedTime = TimeOnly.FromDateTime(DateTime.Now.AddHours(-1)),
            InspectionEndedTime = TimeOnly.FromDateTime(DateTime.Now.AddHours(-2)),
            MultiDayInspection = true,
        };

        // Act
        var result = await _validator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.InspectionEndedTime);
    }
}
