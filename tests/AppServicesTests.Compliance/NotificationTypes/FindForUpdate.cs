using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.Notifications;
using AirWeb.AppServices.Core.EntityServices.Users;
using AirWeb.Domain.Compliance.ComplianceEntities.ComplianceMonitoring;
using AirWeb.TestData.SampleData;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace AppServicesTests.Compliance.NotificationTypes;

public class FindForUpdate
{
    private static NotificationTypeService GetService(
        INotificationTypeRepository repoMock, INotificationTypeManager managerMock)
    {
        var userServiceMock = Substitute.For<IUserService>();
        var logger = Substitute.For<ILogger<NotificationTypeService>>();
        using var cache = Substitute.For<IMemoryCache>();
        return new NotificationTypeService(Setup.Mapper!, repoMock, managerMock, userServiceMock, cache, logger);
    }

    [Test]
    public async Task WhenItemExists_ReturnsViewDto()
    {
        // Arrange
        var item = new NotificationType(Guid.Empty, SampleText.ValidName);
        var repoMock = Substitute.For<INotificationTypeRepository>();
        repoMock.FindAsync(item.Id, Arg.Any<CancellationToken>())
            .Returns(item);
        var managerMock = Substitute.For<INotificationTypeManager>();
        var appService = GetService(repoMock, managerMock);

        // Act
        var result = await appService.FindForUpdateAsync(Guid.Empty);

        //Assert
        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsNull()
    {
        // Arrange
        var id = Guid.Empty;
        var repoMock = Substitute.For<INotificationTypeRepository>();
        repoMock.FindAsync(id, Arg.Any<CancellationToken>())
            .Returns((NotificationType?)null);
        var managerMock = Substitute.For<INotificationTypeManager>();
        var appService = GetService(repoMock, managerMock);

        // Act
        var result = await appService.FindForUpdateAsync(Guid.Empty);

        //Assert
        result.Should().BeNull();
    }
}
