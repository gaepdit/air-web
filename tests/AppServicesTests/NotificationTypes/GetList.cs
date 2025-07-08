using AirWeb.AppServices.AuthenticationServices;
using AirWeb.AppServices.NamedEntities.NotificationTypes;
using AirWeb.Domain.NamedEntities.NotificationTypes;
using AirWeb.TestData.SampleData;

namespace AppServicesTests.NotificationTypes;

public class GetList
{
    [Test]
    public async Task ReturnsViewDtoList()
    {
        var itemList = new List<NotificationType> { new(Guid.Empty, SampleText.ValidName) };
        var repoMock = Substitute.For<INotificationTypeRepository>();
        repoMock.GetOrderedListAsync(Arg.Any<CancellationToken>())
            .Returns(itemList);
        var managerMock = Substitute.For<INotificationTypeManager>();
        var userServiceMock = Substitute.For<IUserService>();
        var appService =
            new NotificationTypeService(AppServicesTestsSetup.Mapper!, repoMock, managerMock, userServiceMock);

        var result = await appService.GetListAsync();

        result.Should().BeEquivalentTo(itemList);
    }
}
