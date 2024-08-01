using AirWeb.AppServices.NamedEntities.NotificationTypes;
using AirWeb.Domain.NamedEntities.NotificationTypes;
using AirWeb.TestData.SampleData;
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

        var model = new NotificationTypeCreateDto { Name = SampleText.ValidName };

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
            .Returns(new NotificationType(Guid.Empty, SampleText.ValidName));

        var model = new NotificationTypeCreateDto { Name = SampleText.ValidName };

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

        var model = new NotificationTypeCreateDto { Name = SampleText.ShortName };

        // Act
        var result = await new NotificationTypeCreateValidator(repoMock).TestValidateAsync(model);

        // Assert
        result.ShouldHaveValidationErrorFor(e => e.Name);
    }
}
