using AirWeb.AppServices.AuthenticationServices;
using AirWeb.AppServices.Lookups.NotificationTypes;
using AirWeb.Domain.Identity;
using AirWeb.Domain.Lookups.NotificationTypes;
using AirWeb.TestData.SampleData;

namespace AppServicesTests.NotificationTypes;

public class Create
{
    [Test]
    public async Task WhenResourceIsValid_ReturnsId()
    {
        var item = new NotificationType(Guid.NewGuid(), SampleText.ValidName);
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
