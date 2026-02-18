using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring;
using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.ComplianceWorkDto.Command;
using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.SourceTestReviews;
using AirWeb.TestData.SampleData;
using FluentValidation.TestHelper;

namespace AppServicesTests.Compliance.ComplianceMonitoring.Validators;

public class SourceTestReviewCreateValidatorTests
{
    private static readonly ComplianceWorkCommandValidator ComplianceWorkCommandValidator = new();

    private static readonly ComplianceWorkCreateValidator ComplianceWorkCreateValidator =
        new(ComplianceWorkCommandValidator);

    private static readonly SourceTestReviewCommandValidator SourceTestReviewCommandValidator = new();

    private static SourceTestReviewCreateValidator GetCreateValidator(IComplianceWorkService service) =>
        new(service, ComplianceWorkCreateValidator, SourceTestReviewCommandValidator);

    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        // Arrange
        var model = new SourceTestReviewCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            TestReportIsClosed = true,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            ReceivedByComplianceDate = DateOnly.FromDateTime(DateTime.Today),
        };

        var service = Substitute.For<IComplianceWorkService>();
        service.SourceTestReviewExistsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(false);

        var validator = GetCreateValidator(service);

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public async Task SourceTestIsNotClosed_ReturnsAsInvalid()
    {
        // Arrange
        var model = new SourceTestReviewCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            TestReportIsClosed = false,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            ReceivedByComplianceDate = DateOnly.FromDateTime(DateTime.Today),
        };

        var service = Substitute.For<IComplianceWorkService>();
        service.SourceTestReviewExistsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(false);

        var validator = GetCreateValidator(service);

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.TestReportIsClosed);
    }

    [Test]
    public async Task ReviewAlreadyExists_ReturnsAsInvalid()
    {
        // Arrange
        var model = new SourceTestReviewCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            TestReportIsClosed = true,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            ReceivedByComplianceDate = DateOnly.FromDateTime(DateTime.Today),
        };

        var service = Substitute.For<IComplianceWorkService>();
        service.SourceTestReviewExistsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(true);

        var validator = GetCreateValidator(service);

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result
            .ShouldHaveValidationErrorFor(dto => dto.ReferenceNumber)
            .WithErrorMessage("A compliance review already exists for this reference number.");
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
            ReceivedByComplianceDate = DateOnly.FromDateTime(DateTime.Today),
        };

        var service = Substitute.For<IComplianceWorkService>();
        service.SourceTestReviewExistsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(false);

        var validator = GetCreateValidator(service);

        // Act
        var result = await validator.TestValidateAsync(model);

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
            ReceivedByComplianceDate = DateOnly.FromDateTime(DateTime.Today).AddDays(1),
        };

        var service = Substitute.For<IComplianceWorkService>();
        service.SourceTestReviewExistsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(false);

        var validator = GetCreateValidator(service);

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.ReceivedByComplianceDate);
    }
}
