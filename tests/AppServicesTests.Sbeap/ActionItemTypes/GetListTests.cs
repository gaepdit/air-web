using AirWeb.AppServices.Core.EntityServices.Users;
using AirWeb.AppServices.Sbeap.ActionItemTypes;
using AirWeb.Domain.Sbeap.Entities.ActionItemTypes;
using AppServicesTests.Sbeap.TestData;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace AppServicesTests.Sbeap.ActionItemTypes;

public class GetListTests
{
    [Test]
    public async Task WhenItemsExist_ReturnsViewDtoList()
    {
        // Arrange
        var itemList = new List<ActionItemType> { new(Guid.Empty, Constants.ValidName) };

        var repoMock = Substitute.For<IActionItemTypeRepository>();
        repoMock.GetOrderedListAsync(Arg.Any<CancellationToken>())
            .Returns(itemList);

        var appService = new ActionItemTypeService(repoMock, Substitute.For<IActionItemTypeManager>(), Setup.Mapper!,
            Substitute.For<IUserService>(), Setup.FakeCache!, Substitute.For<ILogger<ActionItemTypeService>>());

        // Act
        var result = await appService.GetListAsync();

        // Assert
        result.Should().BeEquivalentTo(itemList);
    }

    [Test]
    public async Task WhenNoItemsExist_ReturnsEmptyList()
    {
        // Arrange
        var repoMock = Substitute.For<IActionItemTypeRepository>();
        repoMock.GetListAsync(Arg.Any<CancellationToken>()).Returns(new List<ActionItemType>());
        var managerMock = Substitute.For<IActionItemTypeManager>();
        var userServiceMock = Substitute.For<IUserService>();

        using var cache = Substitute.For<IMemoryCache>();
        var appService = new ActionItemTypeService(repoMock, managerMock, Setup.Mapper!, userServiceMock,
            Setup.FakeCache!, Substitute.For<ILogger<ActionItemTypeService>>());

        // Act
        var result = await appService.GetListAsync();

        // Assert
        result.Should().BeEmpty();
    }
}
