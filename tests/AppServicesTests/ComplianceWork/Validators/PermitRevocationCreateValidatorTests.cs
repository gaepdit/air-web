﻿using AirWeb.AppServices.Compliance.ComplianceWork.PermitRevocations;
using AirWeb.AppServices.Compliance.ComplianceWork.WorkEntryDto.Command;
using AirWeb.Domain;
using AirWeb.TestData.SampleData;
using FluentValidation.TestHelper;

namespace AppServicesTests.ComplianceWork.Validators;

public class PermitRevocationCreateValidatorTests
{
    private static readonly WorkEntryCommandValidator WorkEntryCommandValidator = new();
    private static readonly WorkEntryCreateValidator WorkEntryCreateValidator = new(WorkEntryCommandValidator);
    private static readonly PermitRevocationCommandValidator PermitRevocationCommandValidator = new();

    private static readonly PermitRevocationCreateValidator CreateValidator = new(WorkEntryCreateValidator,
        PermitRevocationCommandValidator);

    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        // Arrange
        var model = new PermitRevocationCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
        };

        // Act
        var result = await CreateValidator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public async Task ReceivedDateInFuture_ReturnsAsInvalid()
    {
        // Arrange
        var model = new PermitRevocationCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            ReceivedDate = DateOnly.FromDateTime(DateTime.Today).AddDays(1),
        };

        // Act
        var result = await CreateValidator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.ReceivedDate);
    }

    [Test]
    public async Task ReceivedDateTooOld_ReturnsAsInvalid()
    {
        // Arrange
        var model = new PermitRevocationCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            ReceivedDate = new DateOnly(ComplianceConstants.EarliestWorkEntryYear - 1, 1, 1),
        };

        // Act
        var result = await CreateValidator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.ReceivedDate);
    }

    [Test]
    public async Task RevocationDateTooFarInFuture_ReturnsAsInvalid()
    {
        // Arrange
        var model = new PermitRevocationCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            PermitRevocationDate = DateOnly.FromDateTime(DateTime.Today).AddYears(1).AddDays(1),
        };

        // Act
        var result = await CreateValidator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.PermitRevocationDate);
    }

    [Test]
    public async Task RevocationDateNotTooFarInFuture_ReturnsAsValid()
    {
        // Arrange
        var model = new PermitRevocationCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            PermitRevocationDate = DateOnly.FromDateTime(DateTime.Today).AddYears(1).AddDays(-1),
        };

        // Act
        var result = await CreateValidator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public async Task RevocationDateTooOld_ReturnsAsInvalid()
    {
        // Arrange
        var model = new PermitRevocationCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            PermitRevocationDate = new DateOnly(ComplianceConstants.EarliestWorkEntryYear - 1, 1, 1),
        };

        // Act
        var result = await CreateValidator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.PermitRevocationDate);
    }

    [Test]
    public async Task ShutdownDateTooFarInFuture_ReturnsAsInvalid()
    {
        // Arrange
        var model = new PermitRevocationCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            PhysicalShutdownDate = DateOnly.FromDateTime(DateTime.Today).AddYears(1).AddDays(1),
        };

        // Act
        var result = await CreateValidator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.PhysicalShutdownDate);
    }

    [Test]
    public async Task ShutdownDateNotTooFarInFuture_ReturnsAsValid()
    {
        // Arrange
        var model = new PermitRevocationCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            PhysicalShutdownDate = DateOnly.FromDateTime(DateTime.Today).AddYears(1).AddDays(-1),
        };

        // Act
        var result = await CreateValidator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public async Task ShutdownDateTooOld_ReturnsAsInvalid()
    {
        // Arrange
        var model = new PermitRevocationCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            PhysicalShutdownDate = new DateOnly(ComplianceConstants.EarliestWorkEntryYear - 1, 1, 1),
        };

        // Act
        var result = await CreateValidator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.PhysicalShutdownDate);
    }
}
