using AirWeb.AppServices.Core.EntityServices.Users;
using AirWeb.AppServices.Sbeap.ActionItemTypes;
using AirWeb.Domain.Sbeap.Entities.ActionItemTypes;
using AutoMapper;

namespace AppServicesSbeapTests.ActionItemTypes;

public class FindForUpdateTests
{
    [Test]
    public async Task WhenItemExists_ReturnsViewDto()
    {
        // Arrange
        var actionItemType = new ActionItemType(Guid.Empty, TestData.ValidName);

        var repoMock = Substitute.For<IActionItemTypeRepository>();
        repoMock.FindAsync(actionItemType.Id, Arg.Any<CancellationToken>()).Returns(actionItemType);
        var managerMock = Substitute.For<IActionItemTypeManager>();
        var userServiceMock = Substitute.For<IUserService>();

        var appService = new ActionItemTypeService(Setup.Mapper!, repoMock, managerMock, userServiceMock);

        // Act
        var result = await appService.FindForUpdateAsync(Guid.Empty);

        // Assert
        result.Should().BeEquivalentTo(actionItemType);
    }

    [Test]
    public async Task WhenDoesNotExist_ReturnsNull()
    {
        // Arrange
        var id = Guid.Empty;

        var repoMock = Substitute.For<IActionItemTypeRepository>();
        repoMock.FindAsync(id, Arg.Any<CancellationToken>()).Returns((ActionItemType?)null);
        var managerMock = Substitute.For<IActionItemTypeManager>();
        var mapperMock = Substitute.For<IMapper>();
        var userServiceMock = Substitute.For<IUserService>();

        var appService = new ActionItemTypeService(Setup.Mapper!, repoMock, managerMock, userServiceMock);

        // Act
        var result = await appService.FindForUpdateAsync(Guid.Empty);

        // Assert
        result.Should().BeNull();
    }
}
