using AutoMapper;
using MyApp.AppServices.EntryTypes;
using MyApp.AppServices.UserServices;
using MyApp.Domain.Entities.EntryTypes;
using MyApp.TestData.Constants;

namespace AppServicesTests.EntryTypes;

public class FindForUpdate
{
    [Test]
    public async Task WhenItemExists_ReturnsViewDto()
    {
        var item = new EntryType(Guid.Empty, TextData.ValidName);
        var repoMock = Substitute.For<IEntryTypeRepository>();
        repoMock.FindAsync(item.Id, Arg.Any<CancellationToken>())
            .Returns(item);
        var managerMock = Substitute.For<IEntryTypeManager>();
        var userServiceMock = Substitute.For<IUserService>();
        var appService = new EntryTypeService(AppServicesTestsSetup.Mapper!, repoMock, managerMock, userServiceMock);

        var result = await appService.FindForUpdateAsync(Guid.Empty);

        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsNull()
    {
        var id = Guid.Empty;
        var repoMock = Substitute.For<IEntryTypeRepository>();
        repoMock.FindAsync(id, Arg.Any<CancellationToken>())
            .Returns((EntryType?)null);
        var managerMock = Substitute.For<IEntryTypeManager>();
        var mapperMock = Substitute.For<IMapper>();
        var userServiceMock = Substitute.For<IUserService>();
        var appService = new EntryTypeService(mapperMock, repoMock, managerMock, userServiceMock);

        var result = await appService.FindForUpdateAsync(Guid.Empty);

        result.Should().BeNull();
    }
}
