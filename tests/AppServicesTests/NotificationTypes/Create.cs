using AirWeb.AppServices.Compliance.ComplianceMonitoring.Notifications;
using AirWeb.AppServices.Core.AuthenticationServices;
using AirWeb.Core.Entities;
using AirWeb.Domain.ComplianceEntities.ComplianceMonitoring;
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
