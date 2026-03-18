using AirWeb.AppServices.Core.EntityServices.Offices;
using AirWeb.AppServices.Core.EntityServices.Users;
using AirWeb.Domain.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace AppServicesTests.Core.Offices;

public class GetListTests
{
    [Test]
    public async Task WhenItemsExist_ReturnsViewDtoList()
    {
        // Arrange
        var office = new Office(Guid.NewGuid(), SampleText.ValidName);
        var itemList = new List<Office> { office };

        var repoMock = Substitute.For<IOfficeRepository>();
        repoMock.GetOrderedListAsync(Arg.Any<CancellationToken>()).Returns(itemList);

        using var cache = Substitute.For<IMemoryCache>();
        var appService = new OfficeService(Setup.Mapper!, repoMock, Substitute.For<IOfficeManager>(),
            Substitute.For<IUserService>(), Substitute.For<IAuthorizationService>(), cache,
            Substitute.For<ILogger<OfficeService>>());

        // Act
        var result = await appService.GetListAsync();

        // Assert
        result.Should().BeEquivalentTo(itemList);
    }

    [Test]
    public async Task WhenNoItemsExist_ReturnsEmptyList()
    {
        // Arrange
        var repoMock = Substitute.For<IOfficeRepository>();
        repoMock.GetListAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new List<Office>());

        using var cache = Substitute.For<IMemoryCache>();
        var appService = new OfficeService(Setup.Mapper!, repoMock, Substitute.For<IOfficeManager>(),
            Substitute.For<IUserService>(), Substitute.For<IAuthorizationService>(), cache,
            Substitute.For<ILogger<OfficeService>>());

        // Act
        var result = await appService.GetListAsync();

        // Assert
        result.Should().BeEmpty();
    }
}
