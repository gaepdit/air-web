using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.Notifications;
using AirWeb.AppServices.Core.EntityServices.Users;
using AirWeb.Domain.Compliance.ComplianceEntities.ComplianceMonitoring;
using AirWeb.TestData.SampleData;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace AppServicesTests.Compliance.NotificationTypes;

public class GetListTests
{
    [Test]
    public async Task ReturnsViewDtoList()
    {
        // Arrange
        var itemList = new List<NotificationType> { new(Guid.Empty, SampleText.ValidName) };

        var repoMock = Substitute.For<INotificationTypeRepository>();
        repoMock.GetOrderedListAsync(Arg.Any<CancellationToken>())
            .Returns(itemList);

        using var cache = Substitute.For<IMemoryCache>();
        var appService = new NotificationTypeService(Setup.Mapper!, repoMock,
            Substitute.For<INotificationTypeManager>(), Substitute.For<IUserService>(), cache,
            Substitute.For<ILogger<NotificationTypeService>>());

        // Act
        var result = await appService.GetListAsync();

        //Assert
        result.Should().BeEquivalentTo(itemList);
    }
}
