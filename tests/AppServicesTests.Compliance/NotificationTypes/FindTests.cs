using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.Notifications;
using AirWeb.AppServices.Core.EntityServices.Users;
using AirWeb.Domain.Compliance.ComplianceEntities.ComplianceMonitoring;
using AirWeb.TestData.SampleData;
using Microsoft.Extensions.Logging;

namespace AppServicesTests.Compliance.NotificationTypes;

public class FindTests
{
    [Test]
    public async Task WhenItemExists_ReturnsViewDto()
    {
        // Arrange
        var item = new NotificationType(Guid.Empty, SampleText.ValidName);

        var repoMock = Substitute.For<INotificationTypeRepository>();
        repoMock.FindAsync(item.Id, Arg.Any<CancellationToken>())
            .Returns(item);

        var appService = new NotificationTypeService(Setup.Mapper!, repoMock,
            Substitute.For<INotificationTypeManager>(), Substitute.For<IUserService>(), Setup.FakeCache!,
            Substitute.For<ILogger<NotificationTypeService>>());

        // Act
        var result = await appService.FindAsync(Guid.Empty);

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

        var appService = new NotificationTypeService(Setup.Mapper!, repoMock,
            Substitute.For<INotificationTypeManager>(), Substitute.For<IUserService>(), Setup.FakeCache!,
            Substitute.For<ILogger<NotificationTypeService>>());

        // Act
        var result = await appService.FindAsync(Guid.Empty);

        //Assert
        result.Should().BeNull();
    }
}
