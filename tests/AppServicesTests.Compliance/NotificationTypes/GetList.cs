using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.Notifications;
using AirWeb.AppServices.Core.EntityServices.Users;
using AirWeb.Domain.Compliance.ComplianceEntities.ComplianceMonitoring;
using AirWeb.TestData.SampleData;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace AppServicesTests.Compliance.NotificationTypes;

public class GetList
{
    [Test]
    public async Task ReturnsViewDtoList()
    {
        // Arrange
        var itemList = new List<NotificationType> { new(Guid.Empty, SampleText.ValidName) };
        var repoMock = Substitute.For<INotificationTypeRepository>();
        repoMock.GetOrderedListAsync(Arg.Any<CancellationToken>())
            .Returns(itemList);
        var managerMock = Substitute.For<INotificationTypeManager>();
        var userServiceMock = Substitute.For<IUserService>();

        using var cache = Substitute.For<IMemoryCache>();

        var appService = new NotificationTypeService(Setup.Mapper!, repoMock, managerMock, userServiceMock, cache,
            logger: Substitute.For<ILogger<NotificationTypeService>>());

        // Act
        var result = await appService.GetListAsync();

        //Assert
        result.Should().BeEquivalentTo(itemList);
    }
}
