using AirWeb.AppServices.Compliance.WorkEntries.SourceTestReviews;
using AirWeb.TestData.SampleData;
using FluentValidation.TestHelper;

namespace AppServicesTests.WorkEntries.Validators;

public class SourceTestReviewCreateValidatorTests
{
    private readonly SourceTestReviewCreateValidator _validator = new();

    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        // Arrange
        var model = new SourceTestReviewCreateDto
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
    public async Task AcknowledgmentLetterDateBeforeReceivedByCompliance_ReturnsAsInvalid()
    {
        // Arrange
        var model = new SourceTestReviewCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            AcknowledgmentLetterDate = DateOnly.FromDateTime(DateTime.Today).AddDays(-1),
            ReceivedByCompliance = DateOnly.FromDateTime(DateTime.Today),
        };

        // Act
        var result = await _validator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.AcknowledgmentLetterDate);
    }

    [Test]
    public async Task ReceivedByComplianceInFuture_ReturnsAsInvalid()
    {
        // Arrange
        var model = new SourceTestReviewCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            ReceivedByCompliance = DateOnly.FromDateTime(DateTime.Today).AddDays(1),
        };

        // Act
        var result = await _validator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.ReceivedByCompliance);
    }
}
