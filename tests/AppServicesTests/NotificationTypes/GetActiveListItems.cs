using AirWeb.AppServices.AuthenticationServices;
using AirWeb.AppServices.Lookups.NotificationTypes;
using AirWeb.Domain.ComplianceEntities.ComplianceMonitoring;
using System.Linq.Expressions;

namespace AppServicesTests.NotificationTypes;

public class GetActiveListItems
{
    [Test]
    public async Task GetAsListItems_ReturnsListOfListItems()
    {
        // Arrange
        var itemList = new List<NotificationType>
        {
            new(Guid.Empty, "One"),
            new(Guid.Empty, "Two"),
        };

        var repoMock = Substitute.For<INotificationTypeRepository>();
        repoMock.GetOrderedListAsync(Arg.Any<Expression<Func<NotificationType, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(itemList);

        var managerMock = Substitute.For<INotificationTypeManager>();
        var userServiceMock = Substitute.For<IUserService>();
        var appService =
            new NotificationTypeService(AppServicesTestsSetup.Mapper!, repoMock, managerMock, userServiceMock);

        // Act
        var result = await appService.GetAsListItemsAsync();

        // Assert
        result.Should().BeEquivalentTo(itemList);
    }
}
