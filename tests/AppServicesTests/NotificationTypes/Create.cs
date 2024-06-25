using AirWeb.AppServices.DomainEntities.NotificationTypes;
using AirWeb.AppServices.UserServices;
using AirWeb.Domain.Entities.NotificationTypes;
using AirWeb.Domain.Identity;
using AirWeb.TestData.Constants;

namespace AppServicesTests.NotificationTypes;

public class Create
{
    [Test]
    public async Task WhenResourceIsValid_ReturnsId()
    {
        var item = new NotificationType(Guid.NewGuid(), TextData.ValidName);
        var repoMock = Substitute.For<INotificationTypeRepository>();
        var managerMock = Substitute.For<INotificationTypeManager>();
        managerMock.CreateAsync(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<CancellationToken>())
            .Returns(item);
        var userServiceMock = Substitute.For<IUserService>();
        userServiceMock.GetCurrentUserAsync()
            .Returns((ApplicationUser?)null);
        var appService =
            new NotificationTypeService(AppServicesTestsSetup.Mapper!, repoMock, managerMock, userServiceMock);

        var result = await appService.CreateAsync(item.Name);

        result.Should().Be(item.Id);
    }
}
