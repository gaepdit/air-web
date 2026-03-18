using AirWeb.AppServices.Compliance.Compliance.ComplianceMonitoring.Notifications;
using AirWeb.AppServices.Core.EntityServices.Users;
using AirWeb.Domain.Compliance.ComplianceEntities.ComplianceMonitoring;
using AirWeb.Domain.Core.Entities;
using AirWeb.TestData.SampleData;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace AppServicesTests.Compliance.NotificationTypes;

public class CreateTests
{
    [Test]
    public async Task WhenResourceIsValid_ReturnsId()
    {
        // Arrange
        var item = new NotificationType(Guid.NewGuid(), SampleText.ValidName);
        var repoMock = Substitute.For<INotificationTypeRepository>();

        var managerMock = Substitute.For<INotificationTypeManager>();
        managerMock.CreateAsync(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<CancellationToken>())
            .Returns(item);

        var userServiceMock = Substitute.For<IUserService>();
        userServiceMock.GetCurrentUserAsync()
            .Returns((ApplicationUser?)null);

        using var cache = Substitute.For<IMemoryCache>();

        var appService = new NotificationTypeService(Setup.Mapper!, repoMock, managerMock, userServiceMock, cache,
            Substitute.For<ILogger<NotificationTypeService>>());

        // Act
        var result = await appService.CreateAsync(item.Name);

        //Assert
        result.Should().Be(item.Id);
    }
}
