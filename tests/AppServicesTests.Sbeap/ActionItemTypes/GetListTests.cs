using AirWeb.AppServices.Core.EntityServices.Users;
using AirWeb.AppServices.Sbeap.ActionItemTypes;
using AirWeb.Domain.Sbeap.Entities.ActionItemTypes;

namespace AppServicesSbeapTests.ActionItemTypes;

public class GetListTests
{
    [Test]
    public async Task WhenItemsExist_ReturnsViewDtoList()
    {
        // Arrange
        var actionItemType = new ActionItemType(Guid.Empty, TestData.ValidName);
        var itemList = new List<ActionItemType> { actionItemType };

        var repoMock = Substitute.For<IActionItemTypeRepository>();
        repoMock.GetOrderedListAsync(Arg.Any<CancellationToken>())
            .Returns(itemList);

        var managerMock = Substitute.For<IActionItemTypeManager>();
        var userServiceMock = Substitute.For<IUserService>();

        var appService = new ActionItemTypeService(Setup.Mapper!, repoMock, managerMock, userServiceMock);

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

        var appService = new ActionItemTypeService(Setup.Mapper!, repoMock, managerMock, userServiceMock);

        // Act
        var result = await appService.GetListAsync();

        // Assert
        result.Should().BeEmpty();
    }
}
