using AirWeb.AppServices.DomainEntities.NotificationTypes;
using AirWeb.Domain.Entities.NotificationTypes;
using AirWeb.TestData.Constants;
using FluentValidation.TestHelper;

namespace AppServicesTests.NamedEntityValidators;

public class CreateValidator
{
    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        // Arrange
        var repoMock = Substitute.For<INotificationTypeRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns((NotificationType?)null);

        var model = new NotificationTypeCreateDto(TextData.ValidName);

        // Act
        var result = await new NotificationTypeCreateValidator(repoMock).TestValidateAsync(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(e => e.Name);
    }

    [Test]
    public async Task DuplicateName_ReturnsAsInvalid()
    {
        // Arrange
        var repoMock = Substitute.For<INotificationTypeRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new NotificationType(Guid.Empty, TextData.ValidName));

        var model = new NotificationTypeCreateDto(TextData.ValidName);

        // Act
        var result = await new NotificationTypeCreateValidator(repoMock).TestValidateAsync(model);

        // Assert
        result.ShouldHaveValidationErrorFor(e => e.Name)
            .WithErrorMessage("The name entered already exists.");
    }

    [Test]
    public async Task NameTooShort_ReturnsAsInvalid()
    {
        // Arrange
        var repoMock = Substitute.For<INotificationTypeRepository>();
        repoMock.FindByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns((NotificationType?)null);

        var model = new NotificationTypeCreateDto(TextData.ShortName);

        // Act
        var result = await new NotificationTypeCreateValidator(repoMock).TestValidateAsync(model);

        // Assert
        result.ShouldHaveValidationErrorFor(e => e.Name);
    }
}
