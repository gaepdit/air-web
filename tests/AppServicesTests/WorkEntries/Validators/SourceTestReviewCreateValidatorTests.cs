﻿using AirWeb.AppServices.Compliance.WorkEntries;
using AirWeb.AppServices.Compliance.WorkEntries.SourceTestReviews;
using AirWeb.TestData.SampleData;
using FluentValidation.TestHelper;

namespace AppServicesTests.WorkEntries.Validators;

public class SourceTestReviewCreateValidatorTests
{
    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        // Arrange
        var model = new SourceTestReviewCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            TestReportIsClosed = true,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
        };

        var entryService = Substitute.For<IWorkEntryService>();
        entryService.SourceTestReviewExistsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(false);

        var validator = new SourceTestReviewCreateValidator(entryService);

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
        };

        var entryService = Substitute.For<IWorkEntryService>();
        entryService.SourceTestReviewExistsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(false);

        var validator = new SourceTestReviewCreateValidator(entryService);

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
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            ReceivedByComplianceDate = DateOnly.FromDateTime(DateTime.Today).AddDays(1),
        };

        var entryService = Substitute.For<IWorkEntryService>();
        entryService.SourceTestReviewExistsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(true);

        var validator = new SourceTestReviewCreateValidator(entryService);

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.ReceivedByComplianceDate);
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

        var entryService = Substitute.For<IWorkEntryService>();
        entryService.SourceTestReviewExistsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(false);

        var validator = new SourceTestReviewCreateValidator(entryService);

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

        var entryService = Substitute.For<IWorkEntryService>();
        entryService.SourceTestReviewExistsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(false);

        var validator = new SourceTestReviewCreateValidator(entryService);

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.ReceivedByComplianceDate);
    }
}
