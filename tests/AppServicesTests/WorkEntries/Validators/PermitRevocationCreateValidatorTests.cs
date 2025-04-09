using AirWeb.AppServices.Compliance.WorkEntries.PermitRevocations;
using AirWeb.Domain;
using AirWeb.TestData.SampleData;
using FluentValidation.TestHelper;

namespace AppServicesTests.WorkEntries.Validators;

public class PermitRevocationCreateValidatorTests
{
    private readonly PermitRevocationCreateValidator _validator = new();

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
        var result = await _validator.TestValidateAsync(model);

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
        var model = new PermitRevocationCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            ReceivedDate = new DateOnly(ComplianceConstants.EarliestWorkEntryYear - 1, 1, 1),
        };

        // Act
        var result = await _validator.TestValidateAsync(model);

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
        var result = await _validator.TestValidateAsync(model);

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
        var result = await _validator.TestValidateAsync(model);

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
        var result = await _validator.TestValidateAsync(model);

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
        var result = await _validator.TestValidateAsync(model);

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
        var result = await _validator.TestValidateAsync(model);

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
        var result = await _validator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.PhysicalShutdownDate);
    }
}
