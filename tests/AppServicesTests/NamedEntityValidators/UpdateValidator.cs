using AirWeb.AppServices.DomainEntities.NotificationTypes;
using AirWeb.Domain.Entities.NotificationTypes;
using AirWeb.TestData.Constants;
using FluentValidation;
using FluentValidation.TestHelper;

namespace AppServicesTests.NamedEntityValidators;

public class UpdateValidator
{
    private static ValidationContext<NotificationTypeUpdateDto> GetContext(NotificationTypeUpdateDto model) =>
        new(model) { RootContextData = { ["Id"] = Guid.Empty } };

    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        // Arrange
        var repoMock = Substitute.For<INotificationTypeRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns((NotificationType?)null);

        var model = new NotificationTypeUpdateDto(TextData.ValidName, true);

        // Act
        var result = await new NotificationTypeUpdateValidator(repoMock).TestValidateAsync(GetContext(model));

        // Assert
        result.ShouldNotHaveValidationErrorFor(e => e.Name);
    }

    [Test]
    public async Task DuplicateName_ReturnsAsInvalid()
    {
        // Arrange
        var repoMock = Substitute.For<INotificationTypeRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new NotificationType(Guid.NewGuid(), TextData.ValidName));

        var model = new NotificationTypeUpdateDto(TextData.ValidName, true);

        // Act
        var result = await new NotificationTypeUpdateValidator(repoMock).TestValidateAsync(GetContext(model));

        // Assert
        result.ShouldHaveValidationErrorFor(e => e.Name)
            .WithErrorMessage("The name entered already exists.");
    }

    [Test]
    public async Task DuplicateName_ForSameId_ReturnsAsValid()
    {
        // Arrange
        var repoMock = Substitute.For<INotificationTypeRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new NotificationType(Guid.Empty, TextData.ValidName));

        var model = new NotificationTypeUpdateDto(TextData.ValidName, true);

        // Act
        var result = await new NotificationTypeUpdateValidator(repoMock).TestValidateAsync(GetContext(model));

        // Assert
        result.ShouldNotHaveValidationErrorFor(e => e.Name);
    }

    [Test]
    public async Task NameTooShort_ReturnsAsInvalid()
    {
        // Arrange
        var repoMock = Substitute.For<INotificationTypeRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns((NotificationType?)null);

        var model = new NotificationTypeUpdateDto(TextData.ShortName, true);

        // Act
        var result = await new NotificationTypeUpdateValidator(repoMock).TestValidateAsync(GetContext(model));

        // Assert
        result.ShouldHaveValidationErrorFor(e => e.Name);
    }
}
