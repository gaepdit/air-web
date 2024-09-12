using AirWeb.AppServices.Compliance.WorkEntries.Reports;
using AirWeb.Domain.ComplianceEntities.WorkEntries;
using AirWeb.TestData.SampleData;
using FluentValidation.TestHelper;

namespace AppServicesTests.WorkEntries.Validators;

public class ReportCreateValidatorTests
{
    private readonly ReportCreateValidator _validator = new();

    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        // Arrange
        var model = new ReportCreateDto
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
    public async Task ReceivedDateInFuture_ReturnsAsInvalid()
    {
        // Arrange
        var model = new ReportCreateDto
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
        var model = new ReportCreateDto
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
    public async Task ReceivedDateBeforeSentDate_ReturnsAsInvalid()
    {
        // Arrange
        var model = new ReportCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            ReceivedDate = DateOnly.FromDateTime(DateTime.Today).AddDays(-2),
            SentDate = DateOnly.FromDateTime(DateTime.Today).AddDays(-1),
        };

        // Act
        var result = await _validator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.ReceivedDate);
    }

    [Test]
    public async Task ReportingPeriodStartTooOld_ReturnsAsInvalid()
    {
        // Arrange
        var model = new ReportCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            ReportingPeriodStart = new DateOnly(WorkEntry.EarliestWorkEntryYear - 1, 1, 1),
        };

        // Act
        var result = await _validator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.ReportingPeriodStart);
    }

    [Test]
    public async Task ReportingPeriodEndInFuture_ReturnsAsInvalid()
    {
        // Arrange
        var model = new ReportCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            ReportingPeriodEnd = DateOnly.FromDateTime(DateTime.Today).AddDays(1),
        };

        // Act
        var result = await _validator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.ReportingPeriodEnd);
    }

    [Test]
    public async Task ReportingPeriodEndBeforeStart_ReturnsAsInvalid()
    {
        // Arrange
        var model = new ReportCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            ReportingPeriodStart = DateOnly.FromDateTime(DateTime.Today).AddDays(-2),
            ReportingPeriodEnd = DateOnly.FromDateTime(DateTime.Today).AddDays(-3),
        };

        // Act
        var result = await _validator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.ReportingPeriodEnd);
    }

    [Test]
    public async Task DueDateTooFarInFuture_ReturnsAsInvalid()
    {
        // Arrange
        var model = new ReportCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            DueDate = DateOnly.FromDateTime(DateTime.Today).AddYears(2),
        };

        // Act
        var result = await _validator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.DueDate);
    }

    [Test]
    public async Task DueDateTooOld_ReturnsAsInvalid()
    {
        // Arrange
        var model = new ReportCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            DueDate = new DateOnly(WorkEntry.EarliestWorkEntryYear - 1, 1, 1),
        };

        // Act
        var result = await _validator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.DueDate);
    }

    [Test]
    public async Task SentDateInFuture_ReturnsAsInvalid()
    {
        // Arrange
        var model = new ReportCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            SentDate = DateOnly.FromDateTime(DateTime.Today).AddDays(1),
        };

        // Act
        var result = await _validator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.SentDate);
    }

    [Test]
    public async Task SentDateTooOld_ReturnsAsInvalid()
    {
        // Arrange
        var model = new ReportCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            SentDate = new DateOnly(WorkEntry.EarliestWorkEntryYear - 1, 1, 1),
        };

        // Act
        var result = await _validator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.SentDate);
    }

    [Test]
    public async Task SentDateBeforeReportingPeriodEnd_ReturnsAsInvalid()
    {
        // Arrange
        var model = new ReportCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            ReportingPeriodEnd = DateOnly.FromDateTime(DateTime.Today).AddDays(-1),
            SentDate = DateOnly.FromDateTime(DateTime.Today).AddDays(-2),
        };

        // Act
        var result = await _validator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.SentDate);
    }

    [Test]
    public async Task AcknowledgmentLetterDateBeforeReceivedDate_ReturnsAsInvalid()
    {
        // Arrange
        var model = new ReportCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            ReceivedDate = DateOnly.FromDateTime(DateTime.Today).AddDays(-1),
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
