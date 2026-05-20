using AirWeb.AppServices.Core.EntityServices.Users;
using AirWeb.AppServices.Sbeap.Agencies;
using AirWeb.Domain.Core.Entities;
using AirWeb.Domain.Sbeap.Entities.Agencies;
using AppServicesTests.Sbeap.TestData;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace AppServicesTests.Sbeap.Agencies;

public class CreateTests
{
    [Test]
    public async Task WhenResourceIsValid_ReturnsId()
    {
        // Arrange
        var item = new Agency(Guid.NewGuid(), Constants.ValidName);

        var repoMock = Substitute.For<IAgencyRepository>();

        var managerMock = Substitute.For<IAgencyManager>();
        managerMock.CreateAsync(Arg.Any<string>(), Arg.Is((string?)null), Arg.Any<CancellationToken>()).Returns(item);

        var userServiceMock = Substitute.For<IUserService>();
        userServiceMock.GetCurrentUserAsync().Returns((ApplicationUser?)null);

        using var cache = Substitute.For<IMemoryCache>();
        var appService = new AgencyService(repoMock, managerMock, Setup.Mapper!, userServiceMock, cache,
            Substitute.For<ILogger<AgencyService>>());

        // Act
        var result = await appService.CreateAsync(item.Name);

        // Assert
        result.Should().Be(item.Id);
    }
}
