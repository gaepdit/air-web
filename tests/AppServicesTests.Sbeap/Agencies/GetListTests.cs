using AirWeb.AppServices.Core.EntityServices.Users;
using AirWeb.AppServices.Sbeap.Agencies;
using AirWeb.Domain.Sbeap.Entities.Agencies;
using AppServicesTests.Sbeap.TestData;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace AppServicesTests.Sbeap.Agencies;

public class GetListTests
{
    [Test]
    public async Task WhenItemsExist_ReturnsViewDtoList()
    {
        // Arrange
        var agency = new Agency(Guid.Empty, Constants.ValidName);
        var itemList = new List<Agency> { agency };

        var repoMock = Substitute.For<IAgencyRepository>();
        repoMock.GetOrderedListAsync(Arg.Any<CancellationToken>()).Returns(itemList);

        using var cache = Substitute.For<IMemoryCache>();
        var appService = new AgencyService(repoMock, Substitute.For<IAgencyManager>(), Setup.Mapper!,
            Substitute.For<IUserService>(), cache, Substitute.For<ILogger<AgencyService>>());

        // Act
        var result = await appService.GetListAsync();

        //Assert
        result.Should().BeEquivalentTo(itemList);
    }

    [Test]
    public async Task WhenNoItemsExist_ReturnsEmptyList()
    {
        // Arrange
        var repoMock = Substitute.For<IAgencyRepository>();
        repoMock.GetOrderedListAsync(Arg.Any<CancellationToken>()).Returns(new List<Agency>());

        using var cache = Substitute.For<IMemoryCache>();
        var appService = new AgencyService(repoMock, Substitute.For<IAgencyManager>(), Setup.Mapper!,
            Substitute.For<IUserService>(), cache, Substitute.For<ILogger<AgencyService>>());

        // Act
        var result = await appService.GetListAsync();

        //Assert
        result.Should().BeEmpty();
    }
}
