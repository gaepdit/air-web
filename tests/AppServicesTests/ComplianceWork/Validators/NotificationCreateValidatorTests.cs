using AirWeb.AppServices.Compliance.ComplianceWork.Notifications;
using AirWeb.AppServices.Compliance.ComplianceWork.WorkEntryDto.Command;
using AirWeb.Domain;
using AirWeb.TestData.SampleData;
using FluentValidation.TestHelper;

namespace AppServicesTests.ComplianceWork.Validators;

public class NotificationCreateValidatorTests
{
    private static readonly WorkEntryCommandValidator WorkEntryCommandValidator = new();
    private static readonly WorkEntryCreateValidator WorkEntryCreateValidator = new(WorkEntryCommandValidator);
    private static readonly NotificationCommandValidator NotificationCommandValidator = new();

    private static readonly NotificationCreateValidator CreateValidator =
        new(WorkEntryCreateValidator, NotificationCommandValidator);

    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        // Arrange
        var model = new NotificationCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            NotificationTypeId = SampleText.UnassignedGuid,
        };

        // Act
        var result = await CreateValidator.TestValidateAsync(model);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Test]
    public async Task MissingNotificationType_ReturnsAsInvalid()
    {
        // Arrange
        var model = new NotificationCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
        };

        // Act
        var result = await CreateValidator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.NotificationTypeId);
    }

    [Test]
    public async Task ReceivedDateInFuture_ReturnsAsInvalid()
    {
        // Arrange
        var model = new NotificationCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            NotificationTypeId = SampleText.UnassignedGuid,
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
        var model = new NotificationCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            NotificationTypeId = SampleText.UnassignedGuid,
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
    public async Task ReceivedDateBeforeSentDate_ReturnsAsInvalid()
    {
        // Arrange
        var model = new NotificationCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            NotificationTypeId = SampleText.UnassignedGuid,
            ReceivedDate = DateOnly.FromDateTime(DateTime.Today).AddDays(-2),
            SentDate = DateOnly.FromDateTime(DateTime.Today).AddDays(-1),
        };

        // Act
        var result = await CreateValidator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.ReceivedDate);
    }

    [Test]
    public async Task SentDateInFuture_ReturnsAsInvalid()
    {
        // Arrange
        var model = new NotificationCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            NotificationTypeId = SampleText.UnassignedGuid,
            SentDate = DateOnly.FromDateTime(DateTime.Today).AddDays(1),
        };

        // Act
        var result = await CreateValidator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.SentDate);
    }

    [Test]
    public async Task SentDateTooOld_ReturnsAsInvalid()
    {
        // Arrange
        var model = new NotificationCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            NotificationTypeId = SampleText.UnassignedGuid,
            SentDate = new DateOnly(ComplianceConstants.EarliestWorkEntryYear - 1, 1, 1),
        };

        // Act
        var result = await CreateValidator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.SentDate);
    }

    [Test]
    public async Task DueDateInFuture_ReturnsAsValid()
    {
        // Arrange
        var model = new NotificationCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            NotificationTypeId = SampleText.UnassignedGuid,
            DueDate = DateOnly.FromDateTime(DateTime.Today).AddDays(1),
        };

        // Act
        var result = await CreateValidator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeTrue();
        result.ShouldNotHaveValidationErrorFor(dto => dto.DueDate);
    }

    [Test]
    public async Task DueDateTooOld_ReturnsAsInvalid()
    {
        // Arrange
        var model = new NotificationCreateDto
        {
            FacilityId = SampleText.ValidFacilityId,
            ResponsibleStaffId = SampleText.UnassignedGuid.ToString(),
            NotificationTypeId = SampleText.UnassignedGuid,
            DueDate = new DateOnly(ComplianceConstants.EarliestWorkEntryYear - 1, 1, 1),
        };

        // Act
        var result = await CreateValidator.TestValidateAsync(model);

        // Assert
        using var scope = new AssertionScope();
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(dto => dto.DueDate);
    }
}
