using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.ComplianceWorkDto.Command;
using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.PermitRevocations;
using AirWeb.TestData.SampleData;
using FluentValidation.TestHelper;

namespace AppServicesTests.ComplianceMonitoring.Validators;

public class ComplianceWorkCreateValidatorTests
{
    private static readonly ComplianceWorkCommandValidator ComplianceWorkCommandValidator = new();
    private static readonly ComplianceWorkCreateValidator CreateValidator = new(ComplianceWorkCommandValidator);

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
    public async Task MissingFacilityId_ReturnsAsInvalid()
    {
        // Arrange
        var model = new PermitRevocationCreateDto
        {
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
        };

        // Act
        var result = await CreateValidator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.FacilityId);
    }

    [Test]
    public async Task MissingResponsibleStaffId_ReturnsAsInvalid()
    {
        // Arrange
        var model = new PermitRevocationCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
        };

        // Act
        var result = await CreateValidator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.ResponsibleStaffId);
    }

    [Test]
    public async Task AcknowledgementDateInFuture_ReturnsAsInvalid()
    {
        // Arrange
        var model = new PermitRevocationCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            AcknowledgmentLetterDate = DateOnly.FromDateTime(DateTime.Today).AddDays(1),
        };

        // Act
        var result = await CreateValidator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.AcknowledgmentLetterDate);
    }
}
