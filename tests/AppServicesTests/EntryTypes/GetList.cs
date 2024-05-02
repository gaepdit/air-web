using AirWeb.AppServices.EntryTypes;
using AirWeb.AppServices.UserServices;
using AirWeb.Domain.Entities.EntryTypes;
using AirWeb.TestData.Constants;

namespace AppServicesTests.EntryTypes;

public class GetList
{
    [Test]
    public async Task ReturnsViewDtoList()
    {
        var itemList = new List<EntryType> { new(Guid.Empty, TextData.ValidName) };
        var repoMock = Substitute.For<IEntryTypeRepository>();
        repoMock.GetOrderedListAsync(Arg.Any<CancellationToken>())
            .Returns(itemList);
        var managerMock = Substitute.For<IEntryTypeManager>();
        var userServiceMock = Substitute.For<IUserService>();
        var appService = new EntryTypeService(AppServicesTestsSetup.Mapper!, repoMock, managerMock, userServiceMock);

        var result = await appService.GetListAsync();

        result.Should().BeEquivalentTo(itemList);
    }
}
