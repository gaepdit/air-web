using AutoMapper;
using AirWeb.AppServices.EntryTypes;
using AirWeb.AppServices.UserServices;
using AirWeb.Domain.Entities.NotificationTypes;
using AirWeb.TestData.Constants;

namespace AppServicesTests.EntryTypes;

public class FindForUpdate
{
    [Test]
    public async Task WhenItemExists_ReturnsViewDto()
    {
        var item = new NotificationType(Guid.Empty, TextData.ValidName);
        var repoMock = Substitute.For<INotificationTypeRepository>();
        repoMock.FindAsync(item.Id, Arg.Any<CancellationToken>())
            .Returns(item);
        var managerMock = Substitute.For<INotificationTypeManager>();
        var userServiceMock = Substitute.For<IUserService>();
        var appService = new EntryTypeService(AppServicesTestsSetup.Mapper!, repoMock, managerMock, userServiceMock);

        var result = await appService.FindForUpdateAsync(Guid.Empty);

        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsNull()
    {
        var id = Guid.Empty;
        var repoMock = Substitute.For<INotificationTypeRepository>();
        repoMock.FindAsync(id, Arg.Any<CancellationToken>())
            .Returns((NotificationType?)null);
        var managerMock = Substitute.For<INotificationTypeManager>();
        var mapperMock = Substitute.For<IMapper>();
        var userServiceMock = Substitute.For<IUserService>();
        var appService = new EntryTypeService(mapperMock, repoMock, managerMock, userServiceMock);

        var result = await appService.FindForUpdateAsync(Guid.Empty);

        result.Should().BeNull();
    }
}
