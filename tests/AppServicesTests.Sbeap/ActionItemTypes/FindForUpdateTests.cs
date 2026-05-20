using AirWeb.AppServices.Core.EntityServices.Users;
using AirWeb.AppServices.Sbeap.ActionItemTypes;
using AirWeb.Domain.Sbeap.Entities.ActionItemTypes;
using AppServicesTests.Sbeap.TestData;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace AppServicesTests.Sbeap.ActionItemTypes;

public class FindForUpdateTests
{
    [Test]
    public async Task WhenItemExists_ReturnsViewDto()
    {
        // Arrange
        var actionItemType = new ActionItemType(Guid.Empty, Constants.ValidName);

        var repoMock = Substitute.For<IActionItemTypeRepository>();
        repoMock.FindAsync(actionItemType.Id, Arg.Any<CancellationToken>()).Returns(actionItemType);

        using var cache = Substitute.For<IMemoryCache>();
        var appService = new ActionItemTypeService(repoMock, Substitute.For<IActionItemTypeManager>(), Setup.Mapper!,
            Substitute.For<IUserService>(), cache, Substitute.For<ILogger<ActionItemTypeService>>());

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

        using var cache = Substitute.For<IMemoryCache>();
        var appService = new ActionItemTypeService(repoMock, Substitute.For<IActionItemTypeManager>(), Setup.Mapper!,
            Substitute.For<IUserService>(), cache, Substitute.For<ILogger<ActionItemTypeService>>());

        // Act
        var result = await appService.FindForUpdateAsync(Guid.Empty);

        // Assert
        result.Should().BeNull();
    }
}
