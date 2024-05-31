using AirWeb.AppServices.EntryTypes;
using AirWeb.AppServices.UserServices;
using AirWeb.Domain.Entities.NotificationTypes;
using AirWeb.TestData.Constants;

namespace AppServicesTests.EntryTypes;

public class GetList
{
    [Test]
    public async Task ReturnsViewDtoList()
    {
        var itemList = new List<NotificationType> { new(Guid.Empty, TextData.ValidName) };
        var repoMock = Substitute.For<INotificationTypeRepository>();
        repoMock.GetOrderedListAsync(Arg.Any<CancellationToken>())
            .Returns(itemList);
        var managerMock = Substitute.For<INotificationTypeManager>();
        var userServiceMock = Substitute.For<IUserService>();
        var appService = new EntryTypeService(AppServicesTestsSetup.Mapper!, repoMock, managerMock, userServiceMock);

        var result = await appService.GetListAsync();

        result.Should().BeEquivalentTo(itemList);
    }
}
