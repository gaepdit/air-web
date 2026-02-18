using AirWeb.AppServices.Core.EntityServices.Users;
using AirWeb.AppServices.Sbeap.ActionItemTypes;
using AirWeb.Domain.Core.Entities;
using AirWeb.Domain.Sbeap.Entities.ActionItemTypes;

namespace AppServicesSbeapTests.ActionItemTypes;

public class CreateTests
{
    [Test]
    public async Task WhenResourceIsValid_ReturnsId()
    {
        // Arrange
        var item = new ActionItemType(Guid.NewGuid(), TestData.ValidName);

        var repoMock = Substitute.For<IActionItemTypeRepository>();
        var managerMock = Substitute.For<IActionItemTypeManager>();
        managerMock.CreateAsync(Arg.Any<string>(), Arg.Is((string?)null), Arg.Any<CancellationToken>())
            .Returns(item);
        var userServiceMock = Substitute.For<IUserService>();
        userServiceMock.GetCurrentUserAsync().Returns((ApplicationUser?)null);

        var appService = new ActionItemTypeService(Setup.Mapper!, repoMock, managerMock,
            userServiceMock);

        // Act
        var result = await appService.CreateAsync(item.Name);

        // Assert
        result.Should().Be(item.Id);
    }
}
