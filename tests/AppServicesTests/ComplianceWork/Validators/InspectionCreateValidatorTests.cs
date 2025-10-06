﻿using AirWeb.AppServices.Compliance.ComplianceWork.Inspections;
using AirWeb.AppServices.Compliance.ComplianceWork.WorkEntryDto.Command;
using AirWeb.Domain;
using AirWeb.TestData.SampleData;
using FluentValidation.TestHelper;

namespace AppServicesTests.ComplianceWork.Validators;

public class InspectionCreateValidatorTests
{
    private static readonly WorkEntryCommandValidator WorkEntryCommandValidator = new();
    private static readonly WorkEntryCreateValidator WorkEntryCreateValidator = new(WorkEntryCommandValidator);
    private static readonly InspectionCommandValidator InspectionCommandValidator = new();
    private readonly InspectionCreateValidator _validator = new(WorkEntryCreateValidator, InspectionCommandValidator);

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
